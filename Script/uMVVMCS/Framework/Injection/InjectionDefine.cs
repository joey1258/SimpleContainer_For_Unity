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
    /// <summary>
    /// Resolve 方法的 aots 委托，在 Resolve 方法实际操作开始之前和完成之后根据类型参数进行前置/后置操作
    /// （修改传入的委托参数 resolutionInstance ）（Resolution : 正式决定，决议）
    /// </summary>
    public delegate bool TypeResolutionHandler(IInjector source, Type type, InjectionInto member, object parentInstance, object id, ref object resolutionInstance);

    /// <summary>
    /// ResolveBinding 方法 aots 委托，在方法内部对 id 进行完过滤之后，根据 binding 的 bindingType
    /// 对其进行相应的实例化与注入操作的之前进行的前置操作（修改传入的委托参数 binding）
    /// </summary>
    public delegate object BindingEvaluationHandler(IInjector source, ref IBinding binding);

    /// <summary>
    /// ResolveBinding 方法相关委托，在方法内部根据 binding 的 bindingType
    /// 对其进行相应的实例化与注入操作之后进行后置操作（修改传入的委托参数 instance）
    /// </summary>
    public delegate void BindingResolutionHandler(IInjector source, ref IBinding binding, ref object instance);

    /// <summary>
    /// Inject 方法相关委托，在 Inject 方法实际操作开始之前和完成之后根据类型参数进行前置/后置操作
    /// （修改传入的委托参数 instance）
    /// </summary>
    public delegate void InstanceInjectionHandler(IInjector source, ref object instance, ReflectionInfo reflectInfo);

    /// <summary>
    /// 注入方式
    /// </summary>
	public enum InjectionInto
    {
        None,
        Constructor,
        Field,
        Property
    }

    /// <summary>
    /// binding 实例化模式
    /// </summary>
    public enum ResolutionMode
    {
        /// <summary>
        /// 不论有没有绑定到容器，尝试对所有的类型执行 resolve（默认模式）
        /// </summary>
        ALWAYS_RESOLVE,
        /// <summary>
        /// 只对绑定到容器的类型进行 resolves，尝试对没有绑定的类型执行 resolves 将返回空
        /// </summary>
        BOUND_ONLY
    }
}