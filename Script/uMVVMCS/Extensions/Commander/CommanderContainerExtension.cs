/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

using System;

namespace uMVVMCS.DIContainer
{
    public static class CommanderContainerExtension
    {
        /// <summary>
        /// 在容器中实例化 ICommandDispatcher 对象
        /// </summary>
        public static ICommandDispatcher GetCommandDispatcher(this IInjectionContainer container)
        {
            return container.Resolve<ICommandDispatcher>();
        }

        /// <summary>
        /// 在容器中绑定一个以指定类型为值，ICommand 为 type 的 binding
        /// </summary>
        public static IInjectionContainer RegisterCommand<T>(this IInjectionContainer container) where T : ICommand, new()
        {
            container.RegisterCommand(typeof(T));

            return container;
        }

        /// <summary>
        /// 在容器中绑定一个以指定类型为值，ICommand 为 type 的 binding.
        /// 执行后,可调用 PoolCommands 方法来储存所有 command 到对象池.
        /// RegisterCommands 方法用于使 command 准备进入对象池.
        /// </summary>
        public static IInjectionContainer RegisterCommand(this IInjectionContainer container, Type type)
        {
            if (!type.IsClass && type.IsAssignableFrom(typeof(ICommand)))
            {
                throw new CommandException(CommandException.TYPE_NOT_A_COMMAND);
            }

            container.Bind<ICommand>().To(type);

            return container;
        }

        /// <summary>
        /// 在容器中绑定指定命名空间中的所有类型为值，ICommand 为 type 的多个 binding，再将其存入对象池
        /// </summary>
        public static IInjectionContainer RegisterCommands(this IInjectionContainer container, string namespaceName)
        {
            container.RegisterCommands(namespaceName, true);

            return container;
        }

        /// <summary>
        /// 在容器中绑定指定命名空间中的所有类型为值，ICommand 为 type 的多个 binding，再将其存入对象池
        /// </summary>
        public static IInjectionContainer RegisterCommands(this IInjectionContainer container,
            string namespaceName,
            bool includeChildren)
        {
            var commands = TypeUtils.GetAssignableTypes(typeof(ICommand), namespaceName, includeChildren);

            if (commands.Length > 0)
            {
                for (var i = 0; i < commands.Length; i++)
                {
                    var commandType = commands[i];
                    if (!commandType.IsAbstract)
                    {
                        container.Bind<ICommand>().To(commandType);
                    }
                }

                PoolCommands(container);
            }

            return container;
        }

        /// <summary>
        /// 将容器内的所有 commands 实例化、注入并存入对象池
        /// </summary>
        public static IInjectionContainer PoolCommands(this IInjectionContainer container)
        {
            var commandPool = container.Resolve<ICommandPool>();
            commandPool.Pool();

            return container;
        }
    }
}