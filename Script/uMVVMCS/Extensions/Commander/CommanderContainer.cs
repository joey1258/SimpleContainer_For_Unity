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

using uMVVMCS.DIContainer;

namespace uMVVMCS
{
    public class CommanderContainer : IContainerExtension
    {
        public void OnRegister(IInjectionContainer container)
        {
            // 绑定一个单例的 ICommandDispatcher 对象，所有命令都通过该对象传播
            container.BindSingleton<ICommandDispatcher>().To<CommandDispatcher>();
            // 绑定命令对象池
            var dispatcher = (CommandDispatcher)container.Resolve<ICommandDispatcher>();
            container.Bind<ICommandPool>().To(dispatcher);
        }

        public void OnUnregister(IInjectionContainer container)
        {
            // 清除命令对象池和 ICommandDispatcher 对象
            container.UnbindByType<ICommandDispatcher>();
            container.UnbindByType<ICommandPool>();

            // 清除所有 commands
            container.UnbindByType<ICommand>();
        }
    }
}