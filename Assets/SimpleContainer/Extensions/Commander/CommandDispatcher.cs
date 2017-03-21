using System;
using System.Collections;
using System.Collections.Generic;
using SimpleContainer.Container;
using Utils;

namespace SimpleContainer
{
    public class CommandDispatcher : IDisposable, ICommandDispatcher, ICommandPool
    {
        /// <summary>
        /// 可用 commands 字典，包括了单例和 pool
        /// </summary>
		protected Dictionary<Type, object> commands;

        /// <summary>
        /// commandDispatcher 的容器
        /// </summary>
        protected IInjectionContainer container;

        /// <summary>
        /// commandDispatcher 的容器
        /// </summary>
        protected EventContainerAOT eventContainerAOT;

        /// <summary>
        /// 需要注册的 commands 的类型
        /// </summary>
        protected IList<Type> commandsToRegister;

        #region constructor

        public CommandDispatcher(IInjectionContainer container)
        {
            commands = new Dictionary<Type, object>();
            this.container = container;

            eventContainerAOT = this.container.GetAOT<EventContainerAOT>();
            if (eventContainerAOT == null)
            {
                this.container.RegisterAOT<EventContainerAOT>();
                eventContainerAOT = this.container.GetAOT<EventContainerAOT>();
            }
        }

        public void Init()
        {
            for (int i = 0; i < commandsToRegister.Count; i++)
            {
                RegisterCommand(commandsToRegister[i]);
            }
        }

        #endregion

        #region Dispatch

        /// <summary>
        /// 发送一个指定类型的 command
        /// </summary>
        public void Dispatch<T>(params object[] parameters) where T : ICommand
        {
            Dispatch(typeof(T), parameters);
        }

        /// <summary>
        /// 发送一个指定类型的 command
        /// </summary>
        public void Dispatch(Type type, params object[] parameters)
        {
            // 如果可用 command 字典用含有指定类型的 command
            if (ContainsCommands(type))
            {
                ICommand command = null;
                // 从字典中获取指定类型的 command
                var item = commands[type];
                // 如果是 ICommand 对象（这就代表对象是单例模式）
                if (item is ICommand)
                {
                    command = (ICommand)item;
                }
                else
                {
                    // 否则就是对象池模式
                    command = GetCommandFromPool(type, (List<ICommand>)item);
                    container.Inject(command);
                }

                // 设置 command.dispatcher 为当前，将 command.running 设为真，再将参数 parameters 传人并执行
                command.dispatcher = this;
                command.running = true;
                command.Execute(parameters);

                // 如果 command.keepAlive 为真
                if (command.keepAlive)
                {
                    //如果命令实现了 IUpdatable 接口，并且 EventContainerAOT 的 IUpdatable list 中还没有添加该 command 就进行添加
                    if (command is IUpdatable && !eventContainerAOT.updateable.Contains((IUpdatable)command))
                    {
                        eventContainerAOT.updateable.Add((IUpdatable)command);
                    }
                }
                else
                {
                    // 不为真则释放 command
                    Release(command);
                }
            }
            // 如果不含有该 command 就抛出异常
            else
            {
                throw new Exceptions(
                    string.Format(Exceptions.NO_COMMAND_FOR_TYPE, type));
            }
        }

        #endregion

        #region InvokeDispatch

        /// <summary>
        /// 通过 EventContainerAOT.eventBehaviour 在等待指定秒后发送一个 command 
        /// </summary>
        public void InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand
        {
            StartCoroutine(DispatchInvoke(typeof(T), time, parameters));
        }

        /// <summary>
        /// 通过 EventContainerAOT.eventBehaviour 在等待指定秒后发送一个 command 
        /// </summary>
        public void InvokeDispatch(Type type, float time, params object[] parameters)
        {
            StartCoroutine(DispatchInvoke(type, time, parameters));
        }

        /// <summary>
        /// 等待指定秒后发送一个 command 
        /// </summary>
        protected IEnumerator DispatchInvoke(Type type, float time, params object[] parameters)
        {
            yield return new UnityEngine.WaitForSeconds(time);
            Dispatch(type, parameters);
        }

        #endregion

        #region Release

        /// <summary>
        /// 释放 command.
        /// </summary>
        public void Release(ICommand command)
        {
            // 如果 command.running 不为真直接返回
            if (!command.running) { return; }

            // 如果 command 实现了 IUpdatable 接口，且添加到了 EventContainerAOT 的 IUpdatable list 就进行移除
            if (command is IUpdatable && eventContainerAOT.updateable.Contains((IUpdatable)command))
            {
                eventContainerAOT.updateable.Remove((IUpdatable)command);
            }
            // 如果 command 实现了 IDisposable 接口, 就调用 Dispose 方法
            if (command is IDisposable)
            {
                ((IDisposable)command).Dispose();
            }
            // 设 running 和 keepAlive 为假
            command.running = false;
            command.keepAlive = false;
        }

        /// <summary>
        /// 释放所有 command
        /// </summary>
        public void ReleaseAll()
        {
            List<object> values = new List<object>(commands.Values);
            int length = values.Count;
            for (int i = 0; i < length; i++)
            {
                // 如果当前元素是 ICommand 对象表示为单例模式
                if (values[i] is ICommand)
                {
                    Release((ICommand)values[i]);
                }
                // 否则为对象池模式，循环释放对象池中的对象
                else
                {
                    var pool = (List<ICommand>)values[i];
                    for (int n = 0; n < pool.Count; n++)
                    {
                        Release((ICommand)pool[n]);
                    }
                }
            }
        }

        /// <summary>
        /// 释放指定类型的所有 command
        /// </summary>
        public void ReleaseAll<T>() where T : ICommand
        {
            ReleaseAll(typeof(T));
        }

        /// <summary>
        /// 释放指定类型的所有 command
        /// </summary>
        public void ReleaseAll(Type type)
        {
            List<object> values = new List<object>(commands.Values);
            int length = values.Count;
            for (int i = 0; i < length; i++)
            {
                if (values[i] is ICommand && values[i].GetType().Equals(type))
                {
                    Release((ICommand)values[i]);
                }
                else
                {
                    var pool = (List<ICommand>)values[i];
                    for (int n = 0; n < pool.Count; n++)
                    {
                        var command = (ICommand)pool[n];

                        if (command.GetType().Equals(type))
                        {
                            Release(command);
                        }
                    }
                }
            }
        }

        #endregion

        #region ContainsCommands

        /// <summary>
        /// 返回 commands 字典中是否含有指定类型的 command
        /// </summary>
        public bool ContainsCommands<T>() where T : ICommand
        {
            return commands.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 返回 commands 字典中是否含有指定类型的 command
        /// </summary>
        public bool ContainsCommands(Type type)
        {
            return commands.ContainsKey(type);
        }

        #endregion

        /// <summary>
        /// 返回字典中的所有 key（类型）
        /// </summary>
        public Type[] GetAllRegistrations()
        {
            List<Type> keys = new List<Type>(commands.Keys);
            return keys.ToArray();
        }

        /// <summary>
        /// Starts a coroutine.
        /// </remarks>
        public UnityEngine.Coroutine StartCoroutine(IEnumerator routine)
        {
            return eventContainerAOT.eventBehaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// Stops a coroutine.
        /// </summary>
        public void StopCoroutine(UnityEngine.Coroutine coroutine)
        {
            eventContainerAOT.eventBehaviour.StopCoroutine(coroutine);
        }

        #region GetCommandFromPool

        /// <summary>
        /// 将指定类型的 Command 储存到 Pool
        /// </summary>
        public void PoolCommand(Type commandType)
        {
            var command = (ICommand)container.Resolve(commandType);
            if (commands.ContainsKey(commandType)) return;

            if (command.singleton)
            {
                commands.Add(commandType, command);
            }
            else
            {
                var commandPool = new List<ICommand>(command.preloadPoolSize);

                commandPool.Add(command);

                if (command.preloadPoolSize > 1)
                {
                    for (int i = 1; i < command.preloadPoolSize; i++)
                    {
                        commandPool.Add((ICommand)container.Resolve(commandType));
                    }
                }

                commands.Add(commandType, commandPool);
            }
        }

        /// <summary>
        /// 从对象池的字典中获取一个 command
        /// </summary>
        public ICommand GetCommandFromPool(Type commandType)
        {
            ICommand command = null;

            if (commands.ContainsKey(commandType))
            {
                var item = commands[commandType];
                command = GetCommandFromPool(commandType, (List<ICommand>)item);
            }

            return command;
        }

        /// <summary>
        /// 从对象池的字典中获取一个 command
        /// </summary>
        public ICommand GetCommandFromPool(Type type, List<ICommand> pool)
        {
            ICommand command = null;

            // 获取参数 pool 中的第一个 running 不为真的 ICommand 对象
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].running)
                {
                    command = pool[i];
                    break;
                }
            }

            // 如果没有获取到，在未达到对象池的上限的情况下实例化一个新的实例
            if (command == null)
            {
                // 如果参数 pool 不为空且数量大于对象池的上限就抛出错误
                if (pool.Count > 0 && pool.Count >= pool[0].maxPoolSize)
                {
                    throw new Exceptions(
                        string.Format(Exceptions.MAX_POOL_SIZE, pool[0].ToString()));
                }

                // 获取实例化结果并添加到对象池
                command = (ICommand)container.Resolve(type);
                pool.Add(command);
            }

            return command;
        }

        #endregion

        /// <summary>
        /// 释放 CommandDispatcher 中的所有资源，当 CommandDispatcher 用完后调用。Dispose 方法
        /// 调用后必须释放所有 CommandDispatcher 的引用以便 GC 回收eclaim the memory that the CommandDispatcher was occupying.
        /// </remarks>
        public void Dispose()
        {
            ReleaseAll();
            commands.Clear();
        }

        /// <summary>
        /// 注册一个指定类型的 command.
        /// </summary>
        private void RegisterCommand(Type commandType)
        {
            if (!commandType.IsClass && commandType.IsAssignableFrom(typeof(ICommand)))
            {
                throw new Exception("TYPE_NOT_A_COMMAND");
            }

            if (!commandType.IsAbstract)
            {
                container.Bind<ICommand>().To(commandType);
                PoolCommand(commandType);
            }
        }

        /// <summary>
        /// 添加一个指定类型的 Command
        /// </summary>
        public void AddCommand(Type type)
        {
            commandsToRegister.Add(type);
        }
    }
}
