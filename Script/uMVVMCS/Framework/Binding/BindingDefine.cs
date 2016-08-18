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
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    /// <summary>
    /// 条件判断委托
    /// </summary>
    public delegate bool Condition(InjectionContext context);

    /// <summary>
    /// 添加 Binding 的 extension 委托
    /// </summary>
    public delegate void BindingAddedHandler(IBinder source, ref IBinding binding);

    /// <summary>
    /// 移除 Binding 的 extension 委托
    /// </summary>
    public delegate void BindingRemovedHandler(IBinder source, IList<IBinding> bindings);

    /// <summary>
    /// 约束类型
    /// </summary>
	public enum ConstraintType
    {
        /// <summary>
        /// 约束Segment只能携带一个值
        /// </summary>
        SINGLE,
        /// <summary>
        /// 约束Segment可以携带多个值
        /// </summary>
        MULTIPLE,
    }

    /// <summary>
    /// 注入类型
    /// </summary>
    public enum BindingType
    {
        /// <summary> 
        /// binging 的 value 属性为 Type，或 prefab 路径信息类等，用于储存将要多次使用或创建的类型或路径
        /// 实例化的结果不会储存并覆盖到 value
        /// </summary>
        ADDRESS,
        /// <summary> 
        /// bingding 的 value 为类型或实例，如果是类型，Inject 系统会自动为其创建实例并为实例执行注入
        /// 实例将会保存并覆盖到 bingding 的 value，以保证每次获取的都是同一个实例。
        /// </summary>
        SINGLETON,
        /// <summary> 
        /// bingding 的 value 为类型或实例，如果是类型，Inject 系统会自动为其创建实例并为实例执行注入
        /// bingding 的 value 将储存复数个值，注入的实例将会保存并覆盖到指定元素
        /// </summary>
        MULTITON,
        /// <summary> 
        /// bingding 的 value 为工厂类型或者实例，如果是类型，Inject 系统会自动创建实例并注入
        /// 实例将会保存并覆盖到 bingding 的 value，以保证每次获取的都是同一个实例
        /// 而一旦工厂类被实例化，之后就都是通过工厂类的 Create 方法的具体实现来创建实例 
        /// </summary>
        FACTORY
    }
}
