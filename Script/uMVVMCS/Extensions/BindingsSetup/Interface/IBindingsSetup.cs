/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

namespace uMVVMCS.DIContainer
{
    public interface IBindingsSetup
    {
        /// <summary>
        /// 为指定的实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        void SetupBindings(IInjectionContainer container);
    }
}

