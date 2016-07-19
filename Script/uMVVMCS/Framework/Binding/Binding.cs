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
 * TEMP 类型的 Binding 只能储存类型值，同时不会被储存到 binder
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
        ///  value 属性
        /// </summary>
        public object value
        {
            get
            {
                if(_value is Array) { return ((object[])_value)[0]; }
                return _value;
            }
        }
        protected object _value;
        public object[] valueArray
        {
            get { return (object[])_value; }
        }

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
            _value = _type;
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
            
            if (_constraint != ConstraintType.MULTIPLE)
            {
                _value = o;
            }
            else { AddValue(o); }

            binder.Storing(this);

            return this;
        }

        /// <summary>
        /// 将多个 object 添加到 binding 的 value 属性中
        /// </summary>
        virtual public IBinding To(IList<object> os)
        {
            // 如果约束类型为单例就抛出异常
            if (_constraint != ConstraintType.MULTIPLE)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.CONSTRAINTYPE_NOT_ASSIGNABLE,
                    "[To(IList<object> os)]",
                    "[ConstraintType.SINGLE]"));
            }

            int length = os.Count;
            for (int i = 0; i < length; i++)
            {
                var osi = os[i];
                if (!PassToAdd(osi))
                {
                    throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
                }
                AddValue(osi);
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
        /// 返回一个新的Binding实例,并设置指定类型给 type 属性, BindingType 属性为 TEMP，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind<T>()
        {
            return _binder.Bind<T>();
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton<T>()
        {
            return _binder.BindSingleton<T>();
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory<T>()
        {
            return _binder.BindFactory<T>();
        }

        #endregion

        #region RemoveValue

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值
        /// </summary>
        virtual public IBinding RemoveValue(object o)
        {
            // 过滤空值
            if (o == null) { return this; }

            // 值约束过滤
            if (_constraint == ConstraintType.MULTIPLE)
            {
                if (_bindingType == BindingType.TEMP)
                {
                    Type t = o.GetType();
                    RemoveArrayValue(t);
                }
                RemoveArrayValue(o);
                return this;
            }

            _value = null;

            return this;
        }

        /// <summary>
        /// 从 binding 的 value 属性中移除多个值
        /// </summary>
        virtual public IBinding RemoveValues(IList<object> os)
        {
            // 过滤空值、值约束
            if (os == null || _constraint != ConstraintType.MULTIPLE) { return this; }

            List<object> list = new List<object>(valueArray);

            int length = valueArray.Length;
            int osLength = os.Count;
            int osi = 0;
            for (int i = 0; i < length; i++)
            {
                list.Remove(os[i]);
                length--;
                osi++;
                if (osi >= osLength) { break; }
            }

            _value = list.ToArray();
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

        /// <summary>
        /// 当值约束为 MULTIPLE 时向 value 属性的末尾添加不重复的新元素
        /// </summary>
        virtual public void AddValue(object o)
        {
            // 过滤空值、值约束
            if (o == null || _constraint != ConstraintType.MULTIPLE) { return; }

            if (_value is Array)
            {
                // 过滤同值
                int length = valueArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (o.Equals(valueArray[i])) { return; }
                }

                // 添加元素
                object[] newArray = new object[length + 1];
                valueArray.CopyTo(newArray, 0);
                newArray[length] = o;
                _value = newArray;
            }
            // 如果值还不是数组，直接赋1个新数组
            else { _value = new object[] { o }; }
        }

        /// <summary>
        /// 当值约束为 MULTIPLE 时移除 value 属性中指定的值(如果找不到相同的值则不做处理)
        /// </summary>
        virtual public void RemoveArrayValue(object o)
        {
            // 过滤空值、值约束
            if (o == null || _constraint != ConstraintType.MULTIPLE) { return; }

            // 如果 value 为空或与参数相等，将其置空并返回
            if (o.Equals(_value) || _value == null)
            {
                _value = null;
                return;
            }

            // 遍历数组，将找到的相同的元素移除
            int length = valueArray.Length;
            for (int i = 0; i < length; i++)
            {
                if (o.Equals(valueArray[i]))
                {
                    spliceValueAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 移除数组中指定位置的值
        /// </summary>
        protected void spliceValueAt(int splicePos)
        {
            int mod = 0;
            int length = valueArray.Length;
            object[] newArray = new object[length - 1];

            for (int i = 0; i < length; i++)
            {
                if (i == splicePos)
                {
                    mod = -1;
                    continue;
                }
                newArray[i + mod] = valueArray[i];
            }
            _value = (newArray.Length == 0) ? null : newArray;
        }

        #region set binding property

        /// <summary>
        /// 设置 binding 的值
        /// </summary>
        virtual public IBinding SetValue(object o)
        {
            if (_constraint != ConstraintType.MULTIPLE)
            {
                _value = o;
            }
            else { AddValue(o); }

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
