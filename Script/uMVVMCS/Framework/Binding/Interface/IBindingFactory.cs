/*
 * Copyright 2016 Sun Ning
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
    public interface IBindingFactory
    {
        #region Create default (MANY)

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        IBinding Create<T>(Binder.BindingStoring storing, BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        IBinding Create(Binder.BindingStoring storing, Type type, BindingType bindingType);

        #endregion

        #region Create SINGLE

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        IBinding CreateSingle<T>(Binder.BindingStoring storing, BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        IBinding CreateSingle(Binder.BindingStoring storing, Type type, BindingType bindingType);

        #endregion

        #region Create POOL

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        IBinding CreatePool<T>(Binder.BindingStoring storing, BindingType bindingType);

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        IBinding CreatePool(Binder.BindingStoring storing, Type type, BindingType bindingType);

        #endregion
    }
}