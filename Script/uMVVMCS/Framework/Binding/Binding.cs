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
 * 一般来说，binding 的 type 是其自身 value （类型或者实例）的同类或者父类 但不建议将 value 直接设为
 * 值类型的具体值,如：
 * ==================
 * bind<int>().to(1);
 * ==================
 * 等,容易导致查找相同无 id binding 时将类型相同、值也恰好相同的两个不同用途的 binding 被认为是相同的
 * 一个，同时TEMP 类型的 Binding 也不可以储存值类型的实际值，在该类型中也不存在任何存实际值的意义
 *
 * 如需要储存值类型的实际值，可以使用 SINGLETON 类型，如要储存多个，可以配合如数组等结构转为 object 储存
 *
 * Binding 默认 value 属性同样的值只能有1个，如果需要多个同样的值，可以将值约束设为 POOL（目前尚未实现）
 *
 * 当 bind TEMP 类型 binding 时，即使 To 了多个同类型的不同实例给多个 binding，binder最终也只会保持1个
 * 因为 TEMP 类型 binding 在保存时实例会自动被替换为其自身 Type（值类型）而被判断为相同而只保留1个
 */

using System;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public class Binding : IBinding
    {
        /// <summary>
        /// 用于储存 binding 到 binder 字典的委托
        /// </summary>
        public Binder.BindingStoring storing;

        protected IInfo bindingInfo;

        #region constructor

        public Binding(Binder.BindingStoring s, Type t, BindingType bt)
        {
            storing = s;

            bindingInfo = new Info(t, bt);
        }

        public Binding(Binder.BindingStoring s, Type t, BindingType bt, ConstraintType c)
        {
            storing = s;

            bindingInfo = new Info(t, bt, c);
        }

        #endregion

        #region IBinding implementation 

        #region property

        /// <summary>
        /// type 属性，返回 bindingInfo 的type值
        /// </summary>
        public Type type
        {
            get { return bindingInfo.type; }
        }

        /// <summary>
        ///  value 属性，返回 bindingInfo 的value值
        /// </summary>
        public object value
        {
            get
            {
                if(bindingInfo.value is Array) { return ((object[])bindingInfo.value)[0]; }
                return bindingInfo.value;
            }
        }
        public object[] valueArray
        {
            get { return (object[])bindingInfo.value; }
        }

        /// <summary>
        /// id 属性，返回 bindingInfo 的id值
        /// </summary>
        public object id
        {
            get { return bindingInfo.id; }
        }

        /// <summary>
        /// constraint 属性，(ONE \ MULTIPLE \ POOL)
        /// </summary>
        public ConstraintType constraint
        {
            get { return bindingInfo.valueConstraint; }
        }

        /// <summary>
        /// bindingType 属性
        /// </summary>
        public BindingType bindingType
        {
            get { return bindingInfo.bindingType; }
        }

        /// <summary>
        /// condition 属性
        /// </summary>
        public Condition condition
        {
            get { return bindingInfo.condition; }
            set { bindingInfo.condition = value; }
        }

        #endregion

        #region functions

        #region To

        /// <summary>
        /// 将 binding 所存储的 Info 实例中的 value 属性设为其自身的 type
        /// </summary>
        virtual public IBinding ToSelf()
        {
            // if (bindingInfo.valueConstraint == ConstraintType.POOL)逻辑等 Pool 类实现后再补充
            if (bindingInfo.valueConstraint != ConstraintType.MULTIPLE)
            {
                bindingInfo.value = bindingInfo.type;
            }
            else { bindingInfo.value = new object[] { bindingInfo.type }; }
            
            if (storing != null) { storing(this); }

            return this;
        }

        /// <summary>
        /// 向 binding 所存储的 Info 实例中的 value 属性中添加一个类型
        /// </summary>
        virtual public IBinding To<T>() where T : class
        {
            return To(typeof(T));
        }

        /// <summary>
        /// 向 binding 所存储的 Info 实例中的 value 属性中添加一个object
        /// </summary>
        virtual public IBinding To(object o)
        {
            if (!PassToAdd(ref o))
            {
                throw new BindingSystemException(BindingSystemException.VALUE_NOT_ASSIGNABLE);
            }
            
            if (bindingInfo.valueConstraint != ConstraintType.MULTIPLE)
            {
                bindingInfo.value = o;
            }
            else { AddValue(o); }

            if (storing != null) { storing(this); }

            return this;
        }

        /// <summary>
        /// 将多个 object 添加到 binding 的所存储的 Info 实例中的 value 属性中
        /// </summary>
        virtual public IBinding To(IList<object> os)
        {
            // 如果约束类型为单例就抛出异常
            if (bindingInfo.valueConstraint != ConstraintType.MULTIPLE)
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
                if (!PassToAdd(ref osi))
                {
                    throw new BindingSystemException(BindingSystemException.VALUE_NOT_ASSIGNABLE);
                }
                AddValue(osi);
            }

            if (storing != null) { storing(this); }

            return this;
        }

        /// <summary>
        /// 直接设置一个值给 binding 的所存储的 Info 实例而不进行兼容检查
        /// </summary>
        virtual public IBinding SetValue(object o)
        {
            if (bindingInfo.valueConstraint != ConstraintType.MULTIPLE)
            {
                bindingInfo.value = o;
            }
            else { AddValue(o); }

            if (storing != null) { storing(this); }

            return this;
        }

        #endregion

        #region As

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 id 属性
        /// </summary>
        virtual public IBinding As<T>() where T : class
        {
            return As(typeof(T));
        }

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 id 属性
        /// </summary>
        virtual public IBinding As(object o)
        {
            bindingInfo.id = (o == null) ? null : o;

            if (storing != null) { storing(this); }

            return this;
        }

        #endregion

        #region When

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性
        /// </summary>
        virtual public IBinding When(Condition c)
        {
            bindingInfo.condition = c;

            return this;
        }

        #endregion

        #region Into

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性为 context.parentType 与参数 T 相等
        /// </summary>
        virtual public IBinding Into<T>() where T : class
        {
            bindingInfo.condition = context => context.parentType == typeof(T);

            return this;
        }

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性 context.parentType 与指定类型相等
        /// </summary>
        virtual public IBinding Into(Type t)
        {
            bindingInfo.condition = context => context.parentType == t;

            return this;
        }

        #endregion

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性为返回 context.parentInstance 与参数 i 相等
        /// </summary>
        virtual public IBinding ParentInstanceCondition(object i)
        {
            bindingInfo.condition = context => context.parentInstance == i;

            return this;
        }

        /// <summary>
        /// 从 binding 所存储的 Info 实例中的 value 属性中移除指定的值
        /// </summary>
        virtual public IBinding RemoveValue(object o)
        {
            // 过滤空值
            if (o == null) { return this; }

            // 值约束过滤
            if(bindingInfo.valueConstraint == ConstraintType.MULTIPLE)
            {
                if (bindingInfo.bindingType == BindingType.TEMP)
                {
                    Type t = o.GetType();
                    RemoveArrayValue(t);
                }
                RemoveArrayValue(o);
                return this;
            }

            bindingInfo.value = null;
            return this;
        }

        /// <summary>
        /// 从 binding 所存储的 Info 实例中的 value 属性中移除多个值
        /// </summary>
        virtual public IBinding RemoveValues(IList<object> os)
        {
            // 过滤空值、值约束
            if (os == null || bindingInfo.valueConstraint != ConstraintType.MULTIPLE) { return this; }

            List<object> list = new List<object>(valueArray);

            // 为 TEMP 类型 binding 获取 Type List
            List<Type> types = new List<Type>();
            int length = os.Count;
            if (bindingInfo.bindingType == BindingType.TEMP)
            {
                for (int i = 0; i < length; i++)
                {
                    if (os[i] is Type) { types.Add(os[i] as Type); }
                    else { types.Add(os[i].GetType()); }
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (bindingInfo.bindingType == BindingType.TEMP) { list.Remove(types[i]); }
                else { list.Remove(os[i]); }
            }

            bindingInfo.value = list.ToArray();
            return this;
        }

        #endregion

        #endregion

        /// <summary>
        /// 返回 是否符合添加当前要求
        /// </summary>
        virtual public bool PassToAdd(ref object v)
        {
            // 如果是 TEMP 类型且 value 是值类型的值，返回 false；否则如果是实例，获取实例的类型替代原值
            if (bindingInfo.bindingType == BindingType.TEMP)
            {
                if (v is Type)
                {
                    return TypeUtils.IsAssignable(bindingInfo.type, (v as Type));
                }
                else
                {
                    if (v is ValueType) { return false; }
                    v = v.GetType();
                    return TypeUtils.IsAssignable(bindingInfo.type, (v as Type));
                }
            }

            // 如果是工厂类型，返回参数 v 是否继承 IInjectionFactory
            if (bindingInfo.bindingType == BindingType.FACTORY)
            {
                if (v is Type)
                {
                    return TypeUtils.IsAssignable(typeof(IInjectionFactory), (v as Type));
                }

                return TypeUtils.IsAssignable(typeof(IInjectionFactory), v.GetType());
            }

            // 如果 binding 是 TEMP 类型，返回自身 type 与参数 v 是否是同类或继承关系
            if (v is Type) { return TypeUtils.IsAssignable(bindingInfo.type, (v as Type)); }

            return TypeUtils.IsAssignable(bindingInfo.type, v.GetType());
        }

        /// <summary>
        /// 当值约束为 MULTIPLE 时向 value 属性的末尾添加不重复的新元素
        /// </summary>
        virtual public void AddValue(object o)
        {
            // 过滤空值、值约束
            if (o == null || bindingInfo.valueConstraint != ConstraintType.MULTIPLE) { return; }

            if (bindingInfo.value is Array)
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
                bindingInfo.value = newArray;
            }
            // 如果值还不是数组，直接赋1个新数组
            else { bindingInfo.value = new object[] { o }; }
        }

        /// <summary>
        /// 当值约束为 MULTIPLE 时移除 value 属性中指定的值(如果找不到相同的值则不做处理)
        /// </summary>
        virtual public void RemoveArrayValue(object o)
        {
            // 过滤空值、值约束
            if (o == null || bindingInfo.valueConstraint != ConstraintType.MULTIPLE) { return; }

            // 如果 value 为空或与参数相等，将其置空并返回
            if (o.Equals(bindingInfo.value) || bindingInfo.value == null)
            {
                bindingInfo.value = null;
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
            bindingInfo.value = (newArray.Length == 0) ? null : newArray;
        }
    }
}
