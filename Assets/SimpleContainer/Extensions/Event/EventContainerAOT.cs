using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleContainer.Container;

namespace SimpleContainer
{
    public class EventContainerAOT : IContainerAOT
    {
        /// <summary>
        /// 可释放对象 list
        /// </summary>
        public List<IDisposable> disposable = new List<IDisposable>();

        /// <summary>
        /// 每帧更新对象 list
        /// </summary>
        public List<IUpdatable> updateable = new List<IUpdatable>();

        /// <summary>
        /// 每帧更新完成后调用一次对象 list
        /// </summary>
        public List<ILateUpdatable> lateUpdateable { get; private set; }

        /// <summary>
        /// FixedUpdatable 更新对象 list
        /// </summary>
        public List<IFixedUpdatable> fixedUpdateable { get; private set; }

        /// <summary>
        /// 允许接收 onapplicationfocus 事件
        /// </summary>
        public List<IFocusable> focusable { get; private set; }

        /// <summary>
        /// 允许接收 OnApplicationPause 事件
        /// </summary>
        public List<IPausable> pausable { get; private set; }

        /// <summary>
        /// 当 app 退出时调用
        /// </summary>
        public List<IQuitable> quitable { get; private set; }

        /// <summary>
        /// event
        /// </summary>
        public EventBehaviour eventBehaviour;

        #region constructor

        public EventContainerAOT()
        {
            disposable = new List<IDisposable>();
            updateable = new List<IUpdatable>();
            lateUpdateable = new List<ILateUpdatable>();
            fixedUpdateable = new List<IFixedUpdatable>();
            focusable = new List<IFocusable>();
            pausable = new List<IPausable>();
            quitable = new List<IQuitable>();
        }

        #endregion

        #region functions

        /// <summary>
        /// 注册容器
        /// </summary>
        public void OnRegister(IInjectionContainer container)
        {
            CreateBehaviour(container.id);

            // 将容器添加到 IDisposable list.
            disposable.Add(container);

            // 如果容器中含有 ICommandDispatcher 类型的 binding，且它实现了 IDisposable 接口
            // 就获取它的 ICommandDispatcher 类型实例并将其也添加到 IDisposable list
            var commandDispatches = container.GetTypes<ICommandDispatcher>();
            if (commandDispatches != null && commandDispatches.Count != 0)
            {
                var dispatcher = container.Resolve<ICommandDispatcher>();
                BindInstance(disposable, dispatcher);
            }


            // 添加 AOT 委托
            container.afterAddBinding += OnAfterAddBinding;
            container.afterInstantiate += OnBindingResolution;
        }

        /// <summary>
        /// 注销容器
        /// </summary>
        public void OnUnregister(IInjectionContainer container)
        {
            // 取消 AOT 委托
            container.afterAddBinding -= OnAfterAddBinding;
            container.afterInstantiate -= OnBindingResolution;

            // 释放 list 并销毁组件
            disposable.Clear();
            updateable.Clear();
            lateUpdateable.Clear();
            fixedUpdateable.Clear();
            focusable.Clear();
            pausable.Clear();
            quitable.Clear();
        }

        /// <summary>
        /// 创建 EventBehaviour 并将其挂载在 DDOL 物体上
        private void CreateBehaviour(object containerID)
        {
            if (eventBehaviour == null)
            {
                //Creates a new game object for UpdateableBehaviour.
                var gameObject = new GameObject();
                gameObject.name = String.Format("EventContainerAOT ({0})", containerID);

                //The behaviour should only be removed during unregister.
                MonoBehaviour.DontDestroyOnLoad(gameObject);

                eventBehaviour = gameObject.AddComponent<EventBehaviour>();
                eventBehaviour.aot = this;
            }
        }

        /// <summary>
        /// 处理 binding 添加之后的工作，用于对 SINGLETON 和 MULTITON 类型的 binding 的值
        /// 分别根据其自身类型添加到对应的 list 中去(IDisposable list 或 IUpdatable list)
        /// </summary>
        protected void OnAfterAddBinding(IBinder source, ref IBinding binding)
        {
            if (binding.bindingType == BindingType.SINGLETON ||
                binding.bindingType == BindingType.MULTITON)
            {
                int length = binding.valueList.Count;
                for (int i = 0; i < length; i++)
                {
                    // 如果是 ICommand 对象就直接退出
                    if (binding.valueList[i] is ICommand) { return; }

                    BindInstance(disposable, binding.valueList[i]);
                    BindInstance(updateable, binding.valueList[i]);
                    BindInstance(lateUpdateable, binding.valueList[i]);
                    BindInstance(fixedUpdateable, binding.valueList[i]);
                    BindInstance(focusable, binding.valueList[i]);
                    BindInstance(pausable, binding.valueList[i]);
                    BindInstance(quitable, binding.valueList[i]);
                }
            }
        }

        /// <summary>
        /// 在 ResolveBinding 方法的最后，获取到实例之后、返回实例之前根据 BindingType 以及实例的类型
        /// 对实例进行分类，分别添加到相应的 list 中(IDisposable list 或 IUpdatable list)
        /// </summary>
        protected void OnBindingResolution(IInjector source, ref IBinding binding, ref object instance)
        {
            // 如果是 SINGLETON 或 MULTITON 类型 binding，或是 ICommand 对象就直接退出
            if (binding.bindingType == BindingType.SINGLETON ||
                binding.bindingType == BindingType.MULTITON ||
                instance is ICommand)
            { return; }

            int length = binding.valueList.Count;
            for (int i = 0; i < length; i++)
            {
                BindInstance(disposable, binding.valueList[i]);
                BindInstance(updateable, binding.valueList[i]);
                BindInstance(lateUpdateable, binding.valueList[i]);
                BindInstance(fixedUpdateable, binding.valueList[i]);
                BindInstance(focusable, binding.valueList[i]);
                BindInstance(pausable, binding.valueList[i]);
                BindInstance(quitable, binding.valueList[i]);
            }
        }

        /// <summary>
        /// 将指定类型的实例（参数 object instance）绑定（有则不处理，没有时进行添加）到参数 List<T> instances 中
        /// </summary>
        protected void BindInstance<T>(List<T> instances, object instance)
        {
            if (instance is T && !instances.Contains((T)instance))
            {
                instances.Add((T)instance);
            }
        }

        #endregion
    }
}
