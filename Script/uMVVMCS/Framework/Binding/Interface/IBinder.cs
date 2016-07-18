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
    public interface IBinder
    {
        #region AOT event

        event BindingAddedHandler beforeAddBinding;
        event BindingAddedHandler afterAddBinding;
        event BindingRemovedHandler beforeRemoveBinding;
        event BindingRemovedHandler afterRemoveBinding;

        #endregion

        #region Bind

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 TEMP，值约束为 MULTIPLE
        /// </summary>
        IBinding Bind<T>();

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 SINGLETON，值约束为 MULTIPLE
        /// </summary>
        IBinding BindSingleton<T>();

        /// <summary>
        ///  返回一个新的Binding实例，并设置指定类型给 type 属性和 BindingType 属性为 FACTORY，值约束为 SINGLE
        /// </summary>
        IBinding BindFactory<T>();

        /// <summary>
        /// 未完成，待构思
        /// 返回多个新的Binding实例,并把设置参数分别给 type 和 BindingType 
        /// </summary>
        //  IList<IBinding> Bind(Type[] types, BindingType[] bindingTypes);

        #endregion

        #region GetBinding

        /// <summary>
        /// 根据类型获取 typeBindings 字典和 bindingStorage 中的所有同类型 Binding
        /// </summary>
        IList<IBinding> GetBindingsByType<T>();

        /// <summary>
        /// 根据类型获取 typeBindings 字典和 bindingStorage 中的所有同类型 Binding
        /// </summary>
		IList<IBinding> GetBindingsByType(Type type);

        /// <summary>
        /// 获取 bindingStorage 中所有指定 id 的 binding
        /// </summary>
        IList<IBinding> GetBindingsById(object id);

        /// <summary>
        /// 获取 binder 中所有的 Binding
        /// </summary>
		IList<IBinding> GetAllBindings();

        /// <summary>
        /// 返回 typeBindings 中除自身以外所有 type 和值都相同的 binding
        /// </summary>
        IList<IBinding> GetSameNullIdBinding(IBinding binding);

        /// <summary>
        /// 根据类型和id获取 bindingStorage 中的 Binding
        /// </summary>
		IBinding GetBinding<T>(object id);

        /// <summary>
        /// 根据类型和id获取 bindingStorage 中的 Binding
        /// </summary>
		IBinding GetBinding(Type type, object id);

        #endregion

        #region Unbind

        /// <summary>
        /// 根据类型从 bindingStorage 和 typeBindings 中删除所有同类型 Binding
        /// </summary>
        void UnbindByType<T>();

        /// <summary>
        /// 根据类型从 bindingStorage 和 typeBindings 中删除所有同类型 Binding
        /// </summary>
        void UnbindByType(Type type);

        /// <summary>
        /// 根据类型从 bindingStorage 中删除所有同类型 Binding
        /// </summary>
        void UnbindBindingStorageByType<T>();

        /// <summary>
        /// 根据类型从 bindingStorage 中删除所有同类型 Binding
        /// </summary>
        void UnbindBindingStorageByType(Type type);

        /// <summary>
        /// 根据类型从 typeBindings 中删除所有同类型 Binding
        /// </summary>
        void UnbindNullIdBindingByType<T>();

        /// <summary>
        /// 根据类型从 typeBindings 中删除所有同类型 Binding
        /// </summary>
        void UnbindNullIdBindingByType(Type type);

        /// <summary>
        /// 根据类型和 id 从 bindingStorage 中删除 Binding
        /// </summary>
		void Unbind<T>(object id);

        /// <summary>
        /// 根据类型和 id 从 bindingStorage 中删除 Binding
        /// </summary>
		void Unbind(Type type, object id);

        /// <summary>
        /// 根据 binding 从 bindingStorage 中删除 Binding
        /// </summary>
        void Unbind(IBinding binding);

        #endregion

        #region Remove

        /// <summary>
        /// 删除指定 binding 中指定的 value 值，如果移除后 value 属性为空或 value 约束为唯一，就移除该 binding
        /// </summary>
        void RemoveValue(IBinding binding, object value);

        /// <summary>
        /// 删除指定 binding 中 value 的多个值，如果移除后 value 属性为空或 value 约束为唯一，就移除该 binding
        /// </summary>
        void RemoveValues(IBinding binding, IList<object> values);

        /// <summary>
        /// 删除 binding 自身
        /// </summary>
        void RemoveBinding(IBinding binding);

        /// <summary>
        /// 根据 type 和 id 删除 binding （type 和 id 不可为空）
        /// </summary>
        void RemoveBinding(Type type, object id);

        #endregion
    }
}