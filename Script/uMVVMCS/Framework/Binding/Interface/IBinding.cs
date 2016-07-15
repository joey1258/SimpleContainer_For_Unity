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
    public interface IBinding
    {
        /// <summary>
        /// type 属性，返回 bindingInfo 的type值
        /// </summary>
        Type type { get; }

        /// <summary>
        /// value 属性，返回 bindingInfo 的value值
        /// </summary>
        object value { get; }
        object[] valueArray { get; }

        /// <summary>
        /// name 属性，返回 bindingInfo 的name值
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

        /// <summary>
        /// 将 binding 所存储的 Info 实例中的 value 属性设为其自身的 type
        IBinding ToSelf();

        /// <summary>
        /// 向 binding 所存储的 Info 实例中的 value 属性中添加一个类型
        /// </summary>
        IBinding To<T>() where T : class;

        /// <summary>
        /// 向 binding 所存储的 Info 实例中的 value 属性中添加一个 object 类型的实例
        /// </summary>
        IBinding To(object instance);

        /// <summary>
        /// 将多个 object 添加到 binding 的所存储的 Info 实例中的 value 属性中
        /// </summary>
        IBinding To(System.Collections.Generic.IList<object> value);

        /// <summary>
        /// 直接设置一个值给 binding 的所存储的 Info 实例,但没有进行兼容检查
        /// </summary>
        IBinding SetValue(object obj);

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 name 属性
        /// </summary>
        IBinding As<T>() where T : class;

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 name 属性
        /// </summary>
        IBinding As(object name);

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性
        /// </summary>
        IBinding When(Condition condition);

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性为 context.parentType 与参数 T 相等
        /// </summary>
        IBinding Into<T>() where T : class;

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性 context.parentType 与指定类型相等
        /// </summary>
        IBinding Into(Type type);

        /// <summary>
        /// 设置 binding 所存储的 Info 实例中的 condition 属性为返回 context.parentInstance 与参数 instance 相等
        /// </summary>
        IBinding ParentInstanceCondition(object instance);

        /// <summary>
        /// 从 binding 所存储的 Info 实例中的 value 属性中移除指定的值
        /// </summary>
        IBinding RemoveValue(object value);

        /// <summary>
        /// 从 binding 所存储的 Info 实例中的 value 属性中移除指定的值
        /// </summary>
        IBinding RemoveValues(System.Collections.Generic.IList<object> values);
    }
}