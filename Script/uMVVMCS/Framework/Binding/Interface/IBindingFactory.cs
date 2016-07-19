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
    public interface IBindingFactory
    {
        /// <summary>
        /// binding 数组
        /// </summary>
        IBinding[] bindings { get; }

        #region Create default (MANY)

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        IBinding Create<T>(BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        IBinding Create(Type type, BindingType bindingType);

        #endregion

        #region Create SINGLE

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        IBinding CreateSingle<T>(BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        IBinding CreateSingle(Type type, BindingType bindingType);

        #endregion

        #region Create POOL

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        IBinding CreatePool<T>(BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        IBinding CreatePool(Type type, BindingType bindingType);

        #endregion

        /// <summary>
        /// 创建指定类型的多个 Binding 实例，ConstraintType 为 MULTIPLE，并返回 IBindingFactory
        /// </summary>
        IBindingFactory MultipleCreate(IList<Type> types, IList<BindingType> bindingType);

        #region Binding System Function

        /// <summary>
        /// 为多个 binding 添加值,如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        IBindingFactory To(IList<object> os);

        /// <summary>
        /// 设置多个 binding 的 id 属性
        /// </summary>
        IBindingFactory As(IList<object> os);

        /// <summary>
        /// 设置多个 binding 的 condition 属性
        /// 如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        IBindingFactory When(IList<Condition> cs);

        /// <summary>
        /// 设置多个 binding 的 condition 属性为 context.parentType 与指定类型相等
        /// 如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        IBindingFactory Into(IList<Type> ts);

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 TEMP，值约束为 MULTIPLE
        /// </summary>
        IBinding Bind<T>();

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        IBinding BindSingleton<T>();

        /// <summary>
        ///  返回一个新的Binding实例，并设置指定类型给 type 属性和 BindingType 属性为 FACTORY，值约束为 SINGLE
        /// </summary>
        IBinding BindFactory<T>();

        /// <summary>
        /// 创建多个指定类型的 binding，并返回 IBindingFactory
        /// </summary>
        IBindingFactory MultipleBind(IList<Type> types, IList<BindingType> bindingTypes);

        #endregion
    }
}