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
        /// load 时是否摧毁容器
        /// </summary>
        bool destroyOnLoad { get; set; }

        /// <summary>
        /// 容器 id
        /// </summary>
		object id { get; }

        /// <summary>
        /// 反射信息缓存
        /// </summary>
        IReflectionCache cache { get; }

        /// <summary>
        /// 注册容器到 aots list
        /// </summary>
        IInjectionContainer RegisterAOT<T>() where T : IContainerAOT;

        /// <summary>
        /// 注册容器到 aots list 
        /// </summary>
        IInjectionContainer RegisterAOT(IContainerAOT extension);

        /// <summary>
        /// 将一个容器从 aots list 中移除 
        /// </summary>
        IInjectionContainer UnregisterAOT<T>() where T : IContainerAOT;

        /// <summary>
        /// 将一个容器从 aots list 中移除
        /// </summary>
        IInjectionContainer UnregisterAOT(IContainerAOT extension);
    }
}