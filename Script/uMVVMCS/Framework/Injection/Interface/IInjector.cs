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
    public interface IInjector
    {
        /// <summary>
        /// binding 实例化模式
        /// </summary>
        ResolutionMode resolutionMode { get; set; }

        #region Injector AOT Event

        event TypeResolutionHandler beforeResolve;
        event TypeResolutionHandler afterResolve;
        event BindingEvaluationHandler bindingEvaluation;
        event BindingResolutionHandler bindingResolution;
        event InstanceInjectionHandler beforeInject;
        event InstanceInjectionHandler afterInject;

        #endregion

        #region Resolve

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        T Resolve<T>();

        /// <summary>
        /// 为指定类型、id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// </summary>
        T Resolve<T>(object identifier);

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        T[] ResolveAll<T>();

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        object[] ResolveAll(Type type);

        /// <summary>
        /// 为指定类型和 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例数组
        /// </summary>
        T[] ResolveSpecified<T>(object identifier);

        #endregion

        #region Inject

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        T Inject<T>(T instance) where T : class;

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        object Inject(object instance);

        #endregion
    }
}