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
    public interface IInjectionContainer : IBinder, IInjector, IDisposable
    {
        /// <summary>
        /// 容器 id
        /// </summary>
		object id { get; }

        /// <summary>
        /// 反射信息缓存
        /// </summary>
        IReflectionCache cache { get; }

        /// <summary>
        /// 注册容器到 AOT list
        /// </summary>
        IInjectionContainer RegisterExtension<T>() where T : IContainerExtension;

        /// <summary>
        /// 注册容器到 AOT list 
        /// </summary>
        IInjectionContainer RegisterExtension(IContainerExtension extension);

        /// <summary>
        /// 将一个容器从 AOT list 中移除 
        /// </summary>
        IInjectionContainer UnregisterAOT<T>() where T : IContainerExtension;

        /// <summary>
        /// 将一个容器从 AOT list 中移除
        /// </summary>
        IInjectionContainer UnregisterAOT(IContainerExtension extension);
    }
}