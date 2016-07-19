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
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public class BindingFactory : IBindingFactory
    {
        public IBinder binder;

        /// <summary>
        /// binding 数组
        /// </summary>
        public IBinding[] bindings
        {
            get { return _bindings; }
        }
        protected IBinding[] _bindings;

        #region constructor

        public BindingFactory(IBinder binder) { this.binder = binder; }

        #endregion

        #region Create

        #region Create default (MANY)

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 MULTIPLE
        /// </summary>
        virtual public IBinding Create<T>(BindingType bindingType)
        {
            return Create(typeof(T), bindingType, ConstraintType.MULTIPLE);
        }

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 MULTIPLE
        /// </summary>
        virtual public IBinding Create(Type type, BindingType bindingType)
        {
            return Create(type, bindingType, ConstraintType.MULTIPLE);
        }

        #endregion

        #region Create SINGLE

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 SINGLE
        /// </summary>
        virtual public IBinding CreateSingle<T>(BindingType bindingType)
        {
            return Create(typeof(T), bindingType, ConstraintType.SINGLE);
        }

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 SINGLE
        /// </summary>
        virtual public IBinding CreateSingle(Type type, BindingType bindingType)
        {
            return Create(type, bindingType, ConstraintType.SINGLE);
        }

        #endregion

        #region Create POOL

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 POOL
        /// </summary>
        virtual public IBinding CreatePool<T>(BindingType bindingType)
        {
            return Create(typeof(T), bindingType, ConstraintType.POOL);
        }

        /// <summary>
        /// 创建并返回指定类型的 Binding 实例，ConstraintType 为 POOL
        /// </summary>
        virtual public IBinding CreatePool(Type type, BindingType bindingType)
        {
            return Create(type, bindingType, ConstraintType.POOL);
        }

        #endregion

        /// <summary>
        /// 创建指定类型的多个 Binding 实例，ConstraintType 为 MULTIPLE，并返回 IBindingFactory
        /// </summary>
        virtual public IBindingFactory MultipleCreate(
            IList<Type> types,
            IList<BindingType> bindingType)
        {
            int length = types.Count;
            _bindings = new IBinding[length];

            for (int i = 0; i < length; i++)
            {
                ConstraintType cti = ConstraintType.MULTIPLE;
                if (bindingType[i] != BindingType.TEMP) { cti = ConstraintType.SINGLE; }

                bindings[i] = Create(
                    types[i],
                    bindingType[i],
                    cti);
            }

            return this;
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding Create<T>(BindingType bindingType, ConstraintType constraint)
        {
            return Create(typeof(T), bindingType, constraint);
        }

        /// <summary>
        /// 创建并返回指定类型的Binding实例
        /// </summary>
        virtual public IBinding Create(
            Type type,
            BindingType bindingType,
            ConstraintType constraint)
        {
            IBinding binding = new Binding(binder, type, bindingType, constraint);

            return binding;
        }

        #endregion

        #region Binding System Function

        /// <summary>
        /// 为多个 binding 添加值，如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        virtual public IBindingFactory To(IList<object> os)
        {
            // 不允许参数长度大于 bindings 长度
            if (os.Count > bindings.Length)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            int length = bindings.Length;
            int osLength = os.Count;
            int osi = 0;
            for (int i = 0; i < length; i++)
            {
                bindings[i].To(os[osi]);
                if (osi < osLength - 1) { osi++; }
            }

            return this;
        }

        /// <summary>
        /// 设置多个 binding 的 id 属性
        /// </summary>
        virtual public IBindingFactory As(IList<object> os)
        {
            // 由于 id 必须是唯一的，所以如果参数和 binding的数量不同则将抛出异常
            if (os.Count != bindings.Length)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            int length = bindings.Length;
            int osLength = os.Count;
            int osi = 0;
            for (int i = 0; i < length; i++)
            {
                bindings[i].As(os[osi]);
                if (osi < osLength - 1) { osi++; }
            }

            return this;
        }

        /// <summary>
        /// 设置多个 binding 的 condition 属性
        /// 如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        virtual public IBindingFactory When(IList<Condition> cs)
        {
            // 不允许参数长度大于 bindings 长度
            if (cs.Count > bindings.Length)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            int length = bindings.Length;
            int csLength = cs.Count;
            int csi = 0;
            for (int i = 0; i < length; i++)
            {
                bindings[i].condition = cs[csi];
                if (csi < csLength - 1) { csi++; }
            }

            return this;
        }

        /// <summary>
        /// 设置多个 binding 的 condition 属性为 context.parentType 与指定类型相等
        /// 如果参数长度短于 binding 数量，参数的最后一个元素将被重复使用
        /// </summary>
        virtual public IBindingFactory Into(IList<Type> ts)
        {
            // 不允许参数长度大于 bindings 长度
            if (ts.Count > bindings.Length)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            int length = bindings.Length;
            int tsLength = ts.Count;
            int tsi = 0;
            for (int i = 0; i < length; i++)
            {
                bindings[i].condition = context => context.parentType == ts[tsi];
                if (tsi < tsLength - 1) { tsi++; }
            }

            return this;
        }

        /// <summary>
        /// 返回一个新的Binding实例,并设置指定类型给 type, BindingType 为 TEMP，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind<T>()
        {
            return binder.Bind<T>();
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton<T>()
        {
            return binder.Bind<T>();
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory<T>()
        {
            return binder.Bind<T>();
        }

        /// <summary>
        /// 创建多个指定类型的 binding，并返回 IBindingFactory
        /// </summary>
        virtual public IBindingFactory MultipleBind(IList<Type> types, IList<BindingType> bindingTypes)
        {
            bool notNull = (types != null &&
                bindingTypes != null &&
                types.Count != 0 &&
                bindingTypes.Count != 0);

            if (!notNull)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.NULL_PARAMETER,
                    "[IBinding MultipleBind]",
                    "[types] || [bindingTypes]"));
            }

            if (types.Count != bindingTypes.Count)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            return MultipleCreate(types, bindingTypes);
        }

        #endregion
    }
}