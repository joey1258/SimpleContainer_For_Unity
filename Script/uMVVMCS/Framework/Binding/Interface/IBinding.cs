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
    public interface IBinding
    {
        #region property

        /// <summary>
        /// binder 属性
        /// </summary>
        IBinder binder { get; }

        /// <summary>
        /// type 属性
        /// </summary>
        Type type { get; }

        /// <summary>
        /// value 属性
        ///  valueArray 属性为避免外部需要以数组形式获取时的拆装箱消耗
        /// </summary>
        object value { get; }
        object[] valueArray { get; }

        /// <summary>
        /// id 属性
        /// </summary>
        object id { get; }

        /// <summary>
        /// constraint 属性，(ONE \ MULTIPLE \ POOL)
        /// </summary>
        ConstraintType constraint { get; }

        /// <summary>
        /// bindingType 属性
        /// </summary>
        BindingType bindingType { get; }

        /// <summary>
        /// condition 属性
        /// </summary>
        Condition condition { get; set; }

        #endregion

        #region To

        /// <summary>
        /// 将 value 属性设为其自身的 type
        IBinding ToSelf();

        /// <summary>
        /// 向 value 属性中添加一个类型
        /// </summary>
        IBinding To<T>() where T : class;

        /// <summary>
        /// 向 value 属性中添加一个 object 类型的实例
        /// </summary>
        IBinding To(object instance);

        /// <summary>
        /// 将多个 object 添加到 value 属性中
        /// </summary>
        IBinding To(object[] value);

        #endregion

        #region As

        /// <summary>
        /// 设置 binding 的 name 属性
        /// </summary>
        IBinding As<T>() where T : class;

        /// <summary>
        /// 设置 binding 的 name 属性
        /// </summary>
        IBinding As(object name);

        #endregion

        #region When

        /// <summary>
        /// 设置 binding 的 condition 属性
        /// </summary>
        IBinding When(Condition condition);

        #endregion

        #region Into

        /// <summary>
        /// 设置 binding 的 condition 属性为 context.parentType 与参数 T 相等
        /// </summary>
        IBinding Into<T>() where T : class;

        /// <summary>
        /// 设置 binding 的 condition 属性为 context.parentType 与指定类型相等
        /// </summary>
        IBinding Into(Type type);

        #endregion

        #region ReBind

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 TEMP，值约束为 MULTIPLE
        /// </summary>
        IBinding Bind<T>();
        IBinding Bind(Type type);

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        IBinding BindSingleton<T>();
        IBinding BindSingleton(Type type);

        /// <summary>
        ///  返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 FACTORY，值约束为 SINGLE
        /// </summary>
        IBinding BindFactory<T>();
        IBinding BindFactory(Type type);

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 MULTITON，值约束为 MULTIPLE
        /// </summary>
        IBinding BindMultiton<T>();
        IBinding BindMultiton(Type type);

        /// <summary>
        /// 创建多个指定类型的 binding，并返回 IBindingFactory
        /// </summary>
        IBindingFactory MultipleBind(IList<Type> types, IList<BindingType> bindingTypes);

        #endregion

        #region RemoveValue

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值，如果删除后值为空，则移除 binding
        /// </summary>
        IBinding RemoveValue(object value);

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值，如果删除后值为空，则移除 binding
        /// </summary>
        IBinding RemoveValues(IList<object> values);

        #endregion

        /// <summary>
        /// 设置 binding 的 condition 属性为返回 context.parentInstance 与参数 instance 相等
        /// </summary>
        IBinding ParentInstanceCondition(object instance);

        #region SetProperty

        /// <summary>
        /// 设置 binding 的值
        /// </summary>
        IBinding SetValue(object obj);

        /// <summary>
        /// 设置 binding 的 ConstraintType
        /// </summary>
        IBinding SetConstraint(ConstraintType ct);

        /// <summary>
        /// 设置 binding 的 BindingType
        /// </summary>
        IBinding SetBindingType(BindingType bt);

        #endregion
    }
}