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
    public class CommanderContainer : IContainerAOT
    {
        public void OnRegister(IInjectionContainer container)
        {
            // 绑定一个单例的 ICommandDispatcher binding
            container.BindSingleton<ICommandDispatcher>().To<CommandDispatcher>();
            // 实例化 binding value，所有命令都通过该实例化后的 value 传播
            var dispatcher = (CommandDispatcher)container.Resolve<ICommandDispatcher>();
            // 将实例化后的 binding value 作为值绑定一条 ICommandPool 类型的 binding
            // 此时 container 中将有两条 binding，都为单例类型，且值都为 dispatcher，只有类型不同
            container.Bind<ICommandPool>().To(dispatcher);
        }

        public void OnUnregister(IInjectionContainer container)
        {
            // Unbind ICommandDispatcher 类型和 ICommandPool 类型 binding（清除 dispatcher)
            container.UnbindByType<ICommandDispatcher>();
            container.UnbindByType<ICommandPool>();

            // 清除所有 commands
            container.UnbindByType<ICommand>();
        }
    }
}