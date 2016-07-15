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

/*
 * 由于Info的值约束类型默认就是MANY，所以无需特意写一个MANY创建方法。
 */

using System;

namespace uMVVMCS.DIContainer
{
    public class BindingFactory : IBindingFactory
    {
        #region Create default (MANY)

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        virtual public IBinding Create<T>
            (Binder.BindingStoring storing,
            BindingType bindingType)
        {
            return Create(storing, typeof(T), bindingType, ConstraintType.MULTIPLE);
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 MULTIPLE
        /// </summary>
        virtual public IBinding Create
            (Binder.BindingStoring storing,
            Type type,
            BindingType bindingType)
        {
            return Create(storing, type, bindingType, ConstraintType.MULTIPLE);
        }

        #endregion

        #region Create SINGLE

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        virtual public IBinding CreateSingle<T> (
            Binder.BindingStoring storing,
            BindingType bindingType)
        {
            return Create(storing, typeof(T), bindingType, ConstraintType.SINGLE);
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例，ConstraintType 为 SINGLE
        /// </summary>
        virtual public IBinding CreateSingle (
            Binder.BindingStoring storing,
            Type type,
            BindingType bindingType)
        {
            return Create(storing, type, bindingType, ConstraintType.SINGLE);
        }

        #endregion

        #region Create POOL

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding CreatePool<T> (
            Binder.BindingStoring storing,
            BindingType bindingType)
        {
            return Create(storing, typeof(T), bindingType, ConstraintType.POOL);
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding CreatePool (
            Binder.BindingStoring storing,
            Type type,
            BindingType bindingType)
        {
            IBinding binding = new Binding(storing, type, bindingType, ConstraintType.POOL);

            return binding;
        }

        #endregion

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding Create<T> (
            Binder.BindingStoring storing,
            BindingType bindingType,
            ConstraintType constraint)
        {
            return Create(storing, typeof(T), bindingType, constraint);
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding Create (
            Binder.BindingStoring storing,
            Type type,
            BindingType bindingType,
            ConstraintType constraint)
        {
            // 如果是接口或者虚类，抛出错误
            /*if (type.IsInterface || type.IsAbstract)
            {
                throw new BindingSystemException(BindingSystemException.WRONGTYPE);
            }*/

            IBinding binding = new Binding(storing, type, bindingType, constraint);

            return binding;
        }
    }
}