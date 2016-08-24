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
    public interface IContextRoot
    {
        /// <summary>
        /// 将 container添加到 containers List，并默认 destroyOnLoad 为真
        /// </summary>
        IInjectionContainer AddContainer<T>() where T : IInjectionContainer, new();

        /// <summary>
        /// 将 container添加到 containers List，并默认 destroyOnLoad 为真
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container);

        /// <summary>
        /// 将 container添加到 containers List，并设置其 destroyOnLoad 属性
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container, bool destroyOnLoad);

        /// <summary>
        /// Dispose 指定 id 的容器
        /// </summary>
        void Dispose(object id);

        /// <summary>
        /// 设置容器
        /// </summary>
        void SetupContainers();

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }
}