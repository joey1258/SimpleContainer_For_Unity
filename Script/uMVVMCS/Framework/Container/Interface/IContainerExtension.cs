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

using System;

namespace uMVVMCS.DIContainer
{
    public interface IContainerAOT
    {
        /// <summary>
        /// 当容器注入到容器 list 时被调用，当被调用时可以提供任何容器事件
        /// </summary>
        void OnRegister(IInjectionContainer container);

        /// <summary>
        /// 当容器注入到容器 list 时被调用，当被调用时可以退订任何容器事件
        /// </summary>
        void OnUnregister(IInjectionContainer container);
    }
}