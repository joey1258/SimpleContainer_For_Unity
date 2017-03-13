/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/SimpleContainer
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;
using System.Collections.Generic;

namespace SimpleContainer.Container
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
        ///  value 属性 返回 valueList 的第一个元素
        ///  valueList 返回整个 valueList
        /// </summary>
        object value { get; }
        IList<object> valueList { get; }

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

        /// <summary>
        /// tags 标签
        /// </summary>
        IList<string> tags { get; }

        #endregion

        #region To

        /// <summary>
        /// 将 value 属性设为其自身的 type
        IBinding ToAddress();

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

        #region Tag

        /// <summary>
        /// 设置 binding 的 tags
        /// </summary>
        IBinding Tag(string t);

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
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 ADDRESS，值约束为 MULTIPLE
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
        IBindingFactory MultipleBind(Type[] types, BindingType[] bindingTypes);

        #endregion

        #region RemoveValue

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值，如果删除后值为空，则移除 binding
        /// </summary>
        IBinding RemoveValue(object value);

        /// <summary>
        /// 从 binding 的 value 属性中移除指定的值，如果删除后值为空，则移除 binding
        /// </summary>
        IBinding RemoveValues(object[] values);

        #endregion

        #region RemoveTag

        /// <summary>
        /// 从 binding 中移除多个 tag
        /// </summary>
        IBinding RemoveTag(string t);

        #endregion

        /// <summary>
        /// 设置 binding 的 condition 属性为返回 context.parentInstance 与参数 instance 相等
        /// </summary>
        IBinding ParentInstanceCondition(object instance);

        #region SetProperty

        /// <summary>
        /// 设置 binding 的值(如果是 MULTIPLE 类型则增加，否则覆盖)
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