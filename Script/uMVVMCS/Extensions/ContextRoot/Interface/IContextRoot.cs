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
        /// 容器数组 (返回 containersData List 中每个元素的 container 属性)(包括所有容器)
        /// </summary>
        IInjectionContainer[] containers { get; }

        /// <summary>
        /// 用新创建的容器和 true 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        IInjectionContainer AddContainer<T>() where T : IInjectionContainer, new();

        /// <summary>
        /// 用 container 和 true 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container);

        /// <summary>
        /// 用 container 和 destroyOnLoad 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container, bool destroyOnLoad);

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