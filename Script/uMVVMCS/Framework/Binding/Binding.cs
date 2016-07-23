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
 * 一般来说，binding 的 type 是其自身 value （类型或者实例）的同类或者父类
 * id 用于快速获取 binding，如果需要同1个类型的多个实例，可以将其以数组的形式保存在同一个 binding
 * TEMP 类型的 Binding 只能储存类型值，同时不会被储存到 binder
 * 去除值约束只保留单列与复数两个类型，去除同样必须保存为单例的 POOL 类型
 */

using System;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public class Binding : IBinding
    {
        #region constructor

        public Binding(IBinder b, Type t)
        {
            _binder = b;

            _type = t;
        }

        public Binding(IBinder b, Type t, BindingType bt)
        {
            _binder = b;

            _type = t;

            _bindingType = bt;
        }

        public Binding(IBinder b, Type t, BindingType bt, ConstraintType c)
        {
            _binder = b;

            _type = t;

            _bindingType = bt;

            _constraint = c;

            _value = new List<object>();
        }

        #endregion

        #region IBinding implementation 

        #region property

        /// <summary>
        /// binder 属性
        /// </summary>
        public IBinder binder
        {
            get { return _binder; }
        }
        protected IBinder _binder;

        /// <summary>
        /// type 属性
        /// </summary>
        public Type type
        {
            get { return _type; }
        }
        protected Type _type;

        /// <summary>
        ///  value 属性 返回 valueList 的第一个元素
        ///  valueList 返回整个 valueList
        /// </summary>
        public object value
        {
            get
            {
                if(_value == null) { _value = new List<object>(); }
                return _value[0];
            }
        }
        public IList<object> valueList
        {
            get
            {
                if (_value == null) { _value = new List<object>(); }
                return _value;
            }
        }
        protected IList<object> _value;

        /// <summary>
        /// id 属性
        /// </summary>
        public object id
        {
            get { return _id; }
            set { _id = value; }
        }
        protected object _id;

        /// <summary>
        /// constraint 属性，(ONE \ MULTIPLE \ POOL)
        /// </summary>
        public ConstraintType constraint
        {
            get { return _constraint; }
        }
        protected ConstraintType _constraint;

        /// <summary>
        /// bindingType 属性
        /// </summary>
        public BindingType bindingType
        {
            get { return _bindingType; }
        }
        protected BindingType _bindingType;

        /// <summary>
        /// condition 属性
        /// </summary>
        public Condition condition { get; set; }

        #endregion

        #region functions

        #region To

        /// <summary>
        /// 将 value 属性设为其自身的 type
        /// </summary>
        virtual public IBinding ToSelf()
        {
            // 每个 binding 只有一个 type，所以绑定到自身也必然只有一个值
            _constraint = ConstraintType.SINGLE;
            _value = new List<object>() { _type };

            binder.Storing(this);

            return this;
        }

        /// <summary>
        /// 向 binding 的 value 属性中添加一个类型
        /// </summary>
        virtual public IBinding To<T>() where T : class
        {
            return To(typeof(T));
        }

        /// <summary>
        /// 向 binding 的 value 属性中添加一个object
        /// </summary>
        virtual public IBinding To(object o)
        {
            if (_bindingType == BindingType.TEMP && !(o is Type))
            {
                _bindingType = BindingType.SINGLETON;
                _constraint = ConstraintType.SINGLE;
            }

            if (!PassToAdd(o))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            bool add = true;
            int count = _value.Count;
            for (int n = 0; n < count; n++)
            {
                if (_value[n] == o) { add = false; }
            }

            if (_constraint == ConstraintType.SINGLE || _value == null)
            {
                if (add) { _value = new List<object>() { o }; }
            }
            else
            {
                if (add) { _value.Add(o); }
            }

            binder.Storing(this);

            return this;
        }

        /// <summary>
        /// 将多个 object 添加到 binding 的 value 属性中
        /// </summary>
        virtual public IBinding To(object[] os)
        {
            if (_bindingType == BindingType.TEMP)
            {
                _bindingType = BindingType.MULTITON;
                _constraint = ConstraintType.MULTIPLE;
            }

            // 如果值约束不为复数就抛出异常
            if (_constraint == ConstraintType.SINGLE)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.BINDINGTYPE_NOT_ASSIGNABLE,
                    "[To(object[] os)]",
                    _bindingType));
            }

            int length = os.Length;
            for (int i = 0; i < length; i++)
            {
                bool add = true;
                int count = _value.Count;
                for (int n = 0; n < count; n++)
                {
                    if(_value[n] == os[i]) { add = false; }
                }
                
                if (!PassToAdd(os[i]))
                {
                    throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
                }

                if (add) { _value.Add(os[i]); }
            }

            binder.Storing(this);

            return this;
        }

        #endregion

        #region As

        /// <summary>
        /// 设置 binding 的 id 属性
        /// </summary>
        virtual public IBinding As<T>() where T : class
        {
            return As(typeof(T));
        }

        /// <summary>
        /// 设置 binding 的 id 属性
        /// </summary>
        virtual public IBinding As(object o)
        {
            _id = (o == null) ? null : o;
            binder.Storing(this);

            return this;
        }

        #endregion

        #region When

        /// <summary>
        /// 设置 binding 的 condition 属性
        /// </summary>
        virtual public IBinding When(Condition c)
        {
            condition = c;

            return this;
        }

        #endregion

        #region Into

        /// <summary>
        /// 设置 binding 的 condition 属性为 context.parentType 与参数 T 相等
        /// </summary>
        virtual public IBinding Into<T>() where T : class
        {
            condition = context => context.parentType == typeof(T);

            return this;
        }

        /// <summary>
        /// 设置 binding 的 condition 属性为 context.parentType 与指定类型相等
        /// </summary>
        virtual public IBinding Into(Type t)
        {
            condition = context => context.parentType == t;

            return this;
        }

        #endregion

        #region ReBind

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 TEMP，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind<T>()
        {
            return _binder.Bind<T>();
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 TEMP，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind(Type type)
        {
            return _binder.Bind(type);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton<T>()
        {
            return _binder.BindSingleton<T>();
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton(Type type)
        {
            return _binder.BindSingleton(type);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory<T>()
        {
            return _binder.BindFactory<T>();
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory(Type type)
        {
            return _binder.BindFactory(type);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 MULTITON，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding BindMultiton<T>()
        {
            return _binder.BindMultiton<T>();
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 MULTITON，值约束为 MULTIPLE
        virtual public IBinding BindMultiton(Type type)
        {
            return _binder.BindMultiton(type);
        }

        /// <summary>
        /// 创建多个指定 type 属性的 binding，并返回 IBindingFactory
        /// </summary>
        virtual public IBindingFactory MultipleBind(Type[] types, BindingType[] bindingTypes)
        {
            return _binder.MultipleBind(types, bindingTypes);
        }

        #endregion

        #region RemoveValue

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值，如果删除后值为空，则移除 binding
        /// </summary>
        virtual public IBinding RemoveValue(object o)
        {
            // 过滤空值
            if (o == null) { return this; }

            // 值约束过滤
            if (_constraint == ConstraintType.MULTIPLE)
            {
                _value.Remove(o);
                if(_value.Count == 0) { binder.Unbind(this); }
                return this;
            }

            _value = null;
            binder.Unbind(this);
            return this;
        }

        /// <summary>
        /// 从 binding 的 value 属性中移除多个值，如果删除后值为空，则移除 binding
        /// </summary>
        virtual public IBinding RemoveValues(object[] os)
        {
            // 过滤空值、值约束、要删除的值比已储存的值更多的情况
            if (os == null || os.Length > _value.Count ||
                _constraint != ConstraintType.MULTIPLE)
            { return this; }

            int length = os.Length;
            for (int i = 0; i < length; i++)
            {
                _value.Remove(os[i]);
            }

            if(_value.Count == 0)
            {
                _value = null;
                binder.Unbind(this);
                return this;
            }
            
            return this;
        }

        #endregion

        /// <summary>
        /// 设置 binding 的 condition 属性为返回 context.parentInstance 与参数 i 相等
        /// </summary>
        virtual public IBinding ParentInstanceCondition(object i)
        {
            condition = context => context.parentInstance == i;

            return this;
        }

        #endregion

        #endregion

        /// <summary>
        /// 返回 是否符合添加当前要求
        /// </summary>
        virtual public bool PassToAdd(object v)
        {
            // 如果是工厂类型，返回参数 v 是否继承 IInjectionFactory
            if (_bindingType == BindingType.FACTORY)
            {
                if (v is Type)
                {
                    return TypeUtils.IsAssignable(typeof(IInjectionFactory), (v as Type));
                }

                return TypeUtils.IsAssignable(typeof(IInjectionFactory), v.GetType());
            }

            // 如果 binding 是 TEMP 类型，返回自身 type 与参数 v 是否是同类或继承关系
            if (v is Type) { return TypeUtils.IsAssignable(_type, (v as Type)); }

            return TypeUtils.IsAssignable(_type, v.GetType());
        }

        #region set binding property

        /// <summary>
        /// 设置 binding 的值
        /// </summary>
        virtual public IBinding SetValue(object o)
        {
            if (_constraint != ConstraintType.MULTIPLE)
            {
                _value = new List<object>() { o };
            }
            else { _value.Add(o); }

            binder.Storing(this);

            return this;
        }

        /// <summary>
        /// 设置 binding 的 ConstraintType
        /// </summary>
        virtual public IBinding SetConstraint(ConstraintType ct)
        {
            _constraint = ct;
            return this;
        }

        /// <summary>
        /// 设置 binding 的 BindingType
        /// </summary>
        virtual public IBinding SetBindingType(BindingType bt)
        {
            _bindingType = bt;
            return this;
        }

        #endregion
    }
}
