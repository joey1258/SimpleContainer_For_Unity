/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/ToluaContainer
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;
using System.Collections.Generic;
using Utils;

namespace ToluaContainer.Container
{
    public class Binder : IBinder
    {
        protected IBindingFactory bindingFactory;
        protected Dictionary<Type, IList<IBinding>> typeBindings;

        #region constructor

        public Binder()
        {
            typeBindings = new Dictionary<Type, IList<IBinding>>();
            bindingFactory = new BindingFactory(this);
        }

        #endregion

        #region IBinder implementation 

        #region AOT event

        public event BindingAddedHandler beforeAddBinding;
        public event BindingAddedHandler afterAddBinding;
        public event BindingRemovedHandler beforeRemoveBinding;
        public event BindingRemovedHandler afterRemoveBinding;

        #endregion

        #region Bind

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 ADDRESS，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind<T>()
        {
            return Bind(typeof(T), BindingType.ADDRESS);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 属性为 ADDRESS，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind(Type type)
        {
            return Bind(type, BindingType.ADDRESS);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton<T>()
        {
            return bindingFactory.CreateSingle(typeof(T), BindingType.SINGLETON);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 SINGLETON，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindSingleton(Type type)
        {
            return bindingFactory.CreateSingle(type, BindingType.SINGLETON);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory<T>()
        {
            return bindingFactory.CreateSingle(typeof(T), BindingType.FACTORY);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory(Type type)
        {
            return bindingFactory.CreateSingle(type, BindingType.FACTORY);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 MULTITON，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding BindMultiton<T>()
        {
            return bindingFactory.Create(typeof(T), BindingType.MULTITON);
        }

        /// <summary>
        /// 返回一个指定 type 属性的新 Binding 实例，BindingType 为 MULTITON，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding BindMultiton(Type type)
        {
            return bindingFactory.Create(type, BindingType.MULTITON);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并把设置参数分别给 type 和 BindingType ，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind(Type type, BindingType bindingType)
        {
            return bindingFactory.Create(type, bindingType);
        }

        /// <summary>
        /// 创建多个指定类型的 binding，并返回 IBindingFactory
        /// </summary>
        virtual public IBindingFactory MultipleBind(Type[] types, BindingType[] bindingTypes)
        {
            if (types == null || types.Length == 0)
            {
                throw new ArgumentNullException("types");
            }
            if (bindingTypes == null || bindingTypes.Length == 0)
            {
                throw new ArgumentNullException("bindingTypes");
            }

            if (types.Length != bindingTypes.Length)
            {
                throw new Exceptions(Exceptions.PARAMETERS_LENGTH_ERROR);
            }

            return bindingFactory.MultipleCreate(types, bindingTypes);
        }

        #endregion

        #region GetBinding

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
        virtual public IList<IBinding> GetTypes<T>()
        {
            return GetTypes(typeof(T));
        }

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
		virtual public IList<IBinding> GetTypes(Type type)
        {
            if (!typeBindings.ContainsKey(type)) { return null; }
            return typeBindings[type];
        }

        /// <summary>
        /// 获取储存容器中所有指定 id 的 binding
        /// </summary>
        virtual public IList<IBinding> GetIds(object id)
        {
            if (id == null)
            {
                throw new Exceptions(string.Format(Exceptions.PARAMETER_NUll, "id"));
            }

            List<IBinding> idBindings = new List<IBinding>();

            List<Type> keys = new List<Type>(typeBindings.Keys);
            int lenght = keys.Count;
            for(int i = 0; i < lenght; i++)
            {
                int count = typeBindings[keys[i]].Count;
                for (int n = 0; n < count; n++)
                {
                    if (id.Equals(typeBindings[keys[i]][n].id))
                    {
                        idBindings.Add(typeBindings[keys[i]][n]);
                    }
                }
            }

            if (idBindings.Count == 0) { return null; }
            else { return idBindings; }
        }

        /// <summary>
        /// 获取 binder 中所有的 Binding
        /// </summary>
		virtual public IList<IBinding> GetAll()
        {
            List<IBinding> bindingList = new List<IBinding>();

            List<Type> keys = new List<Type>(typeBindings.Keys);
            int length = typeBindings.Count;
            for (int i = 0; i < length; i++)
            {
                bindingList.AddRange(typeBindings[keys[i]]);
            }

            return bindingList;
        }

        /// <summary>
        /// 返回储存容器中除自身以外所有 type 和值都相同的 binding
        /// </summary>
        virtual public IList<IBinding> GetSameNullId(IBinding binding)
        {
            List<IBinding> bindingList = new List<IBinding>();

            int length = typeBindings[binding.type].Count;
            for (int i = 0; i < length; i++)
            {
                if (typeBindings[binding.type][i].id != null || 
                    typeBindings[binding.type][i].constraint != binding.constraint)
                { continue; }

                if (binding.constraint == ConstraintType.MULTIPLE)
                {
                    if (CompareUtils.isSameValueIList(
                        typeBindings[binding.type][i].valueList,
                        binding.valueList) &&
                        !CompareUtils.isSameObject(typeBindings[binding.type][i], binding))
                    {
                        bindingList.Add(typeBindings[binding.type][i]);
                    }
                }
                else
                {
                    if (CompareUtils.isSameObject(
                        typeBindings[binding.type][i].value,
                        binding.value) &&
                        !CompareUtils.isSameObject(typeBindings[binding.type][i], binding))
                    {
                        bindingList.Add(typeBindings[binding.type][i]);
                    }
                }
            }

            return bindingList;
        }

        /// <summary>
        /// 根据类型和id获取储存容器中的 Binding
        /// </summary>
		virtual public IBinding GetBinding<T>(object id)
        {
            return GetBinding(typeof(T), id);
        }

        /// <summary>
        /// 根据类型和id获取储存容器中的 Binding
        /// </summary>
		virtual public IBinding GetBinding(Type type, object id)
        {
            if(id == null)
            {
                throw new Exceptions(string.Format(Exceptions.PARAMETER_NUll, "id"));
            }

            var bindings = typeBindings[type];

            int lenght = bindings.Count;
            for (int i = 0; i < lenght; i++)
            {
                if (id.Equals(bindings[i].id)) { return bindings[i];}
            }

            return null;
        }

        #endregion

        #region Unbind

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindType<T>()
        {
            UnbindType(typeof(T));
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindType(Type type)
        {
            var bindings = GetTypes(type);

            if (bindings != null && bindings.Count != 0)
            {
                // 如果 AOT 前置委托不为空就执行它
                if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

                int length = bindings.Count;
                // 使用 while 不稳定且容易死循环，必须使用稳定的 for
                for (int i = 0; i < length; i++)
                {
                    RemoveBinding(bindings[0]);
                }

                // 如果 AOT 后置委托不为空就执行它
                if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
            }
        }

        /// <summary>
        /// 根据 id 从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindId(object id)
        {
            var bindings = GetIds(id);

            if (bindings != null && bindings.Count != 0)
            {
                // 如果 AOT 前置委托不为空就执行它
                if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

                int length = bindings.Count;
                for (int i = 0; i < length; i++)
                {
                    // bindings 是直接获取的 typeBindings 中的引用，当 typeBindings 中的引用被
                    // 移除后 bindings 的数量就会跟随着发生变化，GetTypes 方法也是直接返
                    // 回 typeBindings[type],所以一样会随着元素被删除而变化长度，所以每次移除第一个
                    // 元素就可以将所有的元素都移除干净
                    RemoveBinding(bindings[i]);
                }

                // 如果 AOT 后置委托不为空就执行它
                if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
            }
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdType<T>()
        {
            UnbindNullIdType(typeof(T));
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdType(Type type)
        {
            var bindings = typeBindings[type];
            IList<IBinding> nullIds = new List<IBinding>();
            int length = bindings.Count;
            for (int i = 0; i < length; i++)
            {
                if (bindings[i].id == null) { nullIds.Add(bindings[i]); }
            }

            if (nullIds.Count != 0)
            {
                // 如果 AOT 前置委托不为空就执行它
                if (beforeRemoveBinding != null) { beforeRemoveBinding(this, nullIds); }

                length = nullIds.Count;
                for (int i = 0; i < length; i++)
                {
                    // bindings 是直接获取的 typeBindings 中的引用，当 typeBindings 中的引用被
                    // 移除后 bindings 的数量就会跟随着发生变化，GetTypes 方法也是直接返
                    // 回 typeBindings[type],所以一样会随着元素被删除而变化长度；而当前的方法中使用
                    // 了一个新的 List 来储存 typeBindings 中的引用，所以删除元素后 nullIds 的长度
                    // 不会发生变化，所以 index 必须随着循环而变化才能正确的删除所有的元素
                    RemoveBinding(nullIds[i]);
                }

                // 如果 AOT 后置委托不为空就执行它
                if (afterRemoveBinding != null) { afterRemoveBinding(this, nullIds); }
            }

            nullIds = null;
        }

        /// <summary>
        /// 根据类型和 id 从所有容器中删除 Binding
        /// </summary>
		virtual public void Unbind<T>(object id)
        {
            Unbind(typeof(T), id);
        }

        /// <summary>
        /// 根据 type 和 id 从所有容器中删除 Binding (type 和 id 不可为空)
        /// </summary>
		virtual public void Unbind(Type type, object id)
        {
            // 如果参数 type 或 id 为空，就直接退出
            if (type == null || id == null) { return; }

            // 为了可以放入 AOT 委托执行而采用 List 形式储存
            var bindings = new List<IBinding>() { GetBinding(type, id) };

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

            // 如果获取到了 binding 就进行删除
            if (bindings[0] != null)
            {
                typeBindings[type].Remove(bindings[0]);
            }
            bindings = null;

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
        }

        /// <summary>
        /// 根据 binding 从所有容器中删除 Binding
        /// </summary>
        virtual public void Unbind(IBinding binding)
        {
            if (binding == null) { return; }

            // 为了可以放入 AOT 委托执行而采用 List 形式储存
            var bindings = new List<IBinding>() { binding };

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

            RemoveBinding(binding);

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
        }

        #endregion

        /// <summary>
        /// 删除 binding 自身
        /// </summary>
        virtual public void RemoveBinding(IBinding binding)
        {
            // 如果参数 binding 为空，就直接退出
            if (binding == null) { return; }

            // 如果 binding 的 id 为空，就移除 typeBindings 中HashCode相同的 binding
            if (binding.id == null)
            {
                typeBindings[binding.type].Remove(binding);
            }
            // 如果 binding 的 id 不为空，从 bindingStorage、typeBindings、idBindings 三处移除
            else
            {
                typeBindings[binding.type].Remove(binding);
            }
        }

        #endregion 

        /// <summary>
        /// 储存 binding
        /// </summary>
        virtual public void Storing(IBinding binding)
        {
            // 如果参数为空,就抛出异常 (原此处的接口和虚类检查移至工厂的创建方法中)
            if (binding == null) { throw new ArgumentNullException("binding"); }

            // 如果 AOT 前置委托不为空就执行它
            if (beforeAddBinding != null) { beforeAddBinding(this, ref binding); }

            // 储存或整理 binding 到对应的容器
            AddBinding(binding);

            // 如果 AOT 后置委托不为空就执行它
            if (afterAddBinding != null) { afterAddBinding(this, ref binding); }
        }

        /// <summary>
        /// 添加或整理 binding 到相应的容器
        /// </summary>
        virtual protected void AddBinding(IBinding binding)
        {
            if (!typeBindings.ContainsKey(binding.type))
            {
                typeBindings.Add(binding.type, new List<IBinding>());
            }

            bool exist = typeBindings[binding.type].Contains(binding);

            // 如果尚未被添加过才添加到 typeBindings 中
            if (!exist) { typeBindings[binding.type].Add(binding); }
        }
    }
}