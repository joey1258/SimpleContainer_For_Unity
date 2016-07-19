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
    /// 与 Binder 的区别：创建的所有 binding 的值约束均为 SINGLE
    /// </summary>
    public class InjectionBinder : Binder
    {
        #region Bind

        /// <summary>
        /// 返回一个新的Binding实例,并设置指定类型给 type， BindingType 为 TEMP，值约束为 SINGLE
        /// </summary>
        override public IBinding Bind<T>()
        {
            return Bind(typeof(T), BindingType.TEMP);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type， BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        override public IBinding BindSingleton<T>()
        {
            return bindingFactory.CreateSingle(typeof(T), BindingType.SINGLETON);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type， BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        override public IBinding BindFactory<T>()
        {
            return bindingFactory.CreateSingle(typeof(T), BindingType.FACTORY);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并把设置参数分别给 type 和 BindingType，值约束为 SINGLE
        /// </summary>
        override public IBinding Bind(Type type, BindingType bindingType)
        {
            return bindingFactory.CreateSingle(type, bindingType);
        }

        #endregion
    }
}