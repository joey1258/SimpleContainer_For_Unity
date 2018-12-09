/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/SimpleContainer_For_Unity
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;
using System.Collections.Generic;

namespace SimpleContainer.Container
{
    public interface IBinder
    {
        #region AOT event

        event BindingAddedHandler beforeAddBinding;
        event BindingAddedHandler afterAddBinding;
        event BindingRemovedHandler beforeRemoveBinding;
        event BindingRemovedHandler afterRemoveBinding;

        #endregion

        #region Bind

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
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 FACTORY，值约束为 SINGLE
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

        #region GetBinding

        /// <summary>
        /// 根据类型和 id 获取储存容器中的 Binding
        /// </summary>
        IBinding GetBinding<T>(object id);

        /// <summary>
        /// 根据类型和 id 获取储存容器中的 Binding
        /// </summary>
        IBinding GetBinding(Type type, object id);

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
        IList<IBinding> GetTypes<T>();

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
        IList<IBinding> GetTypes(Type type);

        /// <summary>
        /// 获取 binder 中所有的 Binding
        /// </summary>
		IList<IBinding> GetAll();

        /// <summary>
        /// 根据是否符合委托的内容获取储存容器中的 Binding
        /// </summary>
		IList<IBinding> GetByDelegate(IsBindingAccordHandler isBindingAccordHandler);

        /// <summary>
        /// 根据类型和是否符合委托的内容获取储存容器中的 Binding
        /// </summary>
		IList<IBinding> GetByDelegate<T>(IsBindingAccordHandler isBindingAccordHandler);

        /// <summary>
        /// 根据类型和是否符合委托的内容获取储存容器中的 Binding
        /// </summary>
		IList<IBinding> GetByDelegate(Type type, IsBindingAccordHandler isBindingAccordHandler);

        #endregion

        #region Unbind

        /// <summary>
        /// 根据类型和 id 从容器中删除 Binding
        /// </summary>
		void Unbind<T>(object id);

        /// <summary>
        /// 根据类型和 id 从所有容器中删除 Binding
        /// </summary>
		void Unbind(Type type, object id);

        /// <summary>
        /// 根据 binding 从容器中删除 Binding
        /// </summary>
        void Unbind(IBinding binding);

        /// <summary>
        /// 根据类型从容器中删除所有同类型 Binding
        /// </summary>
        void UnbindType<T>();

        /// <summary>
        /// 根据类型从容器中删除所有同类型 Binding
        /// </summary>
        void UnbindType(Type type);

        /// <summary>
        /// 从容器中删除值或 condition 与 instance 相同的 binding
        /// </summary>
        void UnbindInstance(object instance);

        /// <summary>
        /// 从容器中删除值或 condition 与 instance 相同的 binding
        /// </summary>
        void UnbindTag(string tag);

        /// <summary>
        /// 根据委托从容器中删除 binding
        /// </summary>
        void UnbindByDelegate(IsBindingAccordHandler isBindingAccordHandler);

        /// <summary>
        /// 根据类型和委托从容器中删除 binding
        /// </summary>
        void UnbindByDelegate<T>(IsBindingAccordHandler isBindingAccordHandler);

        /// <summary>
        /// 根据类型和委托从容器中删除 binding
        /// </summary>
        void UnbindByDelegate(Type type, IsBindingAccordHandler isBindingAccordHandler);

        #endregion

        /// <summary>
        /// 储存 binding
        /// </summary>
        void Storing(IBinding binding);
    }
}