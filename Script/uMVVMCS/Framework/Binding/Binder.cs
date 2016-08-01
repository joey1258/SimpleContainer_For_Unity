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
 * 当 binding 为 ADDRESS 类型时其值只能为 Type，其会对相同的值进行过滤（对 typeBindings 进行遍历）
 * 因此 ADDRESS 类型的 binding 早于其它类型的 binding 添加的效率将有别于相反的顺序
 */

using System;
using System.Collections.Generic;
using uMVVMCS;

namespace uMVVMCS.DIContainer
{
    public class Binder : IBinder
    {
        protected IBindingFactory bindingFactory;
        protected Storage<IBinding> bindingStorage;
        protected Dictionary<Type, IList<IBinding>> typeBindings;
        protected Dictionary<object, IList<IBinding>> idBindings;

        #region constructor

        public Binder()
        {
            bindingStorage = new Storage<IBinding>();
            typeBindings = new Dictionary<Type, IList<IBinding>>();
            idBindings = new Dictionary<object, IList<IBinding>>();
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
            bool notNull = (types != null && 
                bindingTypes != null &&
                types.Length != 0 && 
                bindingTypes.Length != 0);

            if (!notNull)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.NULL_PARAMETER,
                    "[IBinding MultipleBind]",
                    "[types] || [bindingTypes]"));
            }

            if(types.Length != bindingTypes.Length)
            {
                throw new BindingSystemException(BindingSystemException.PARAMETERS_LENGTH_ERROR);
            }

            return bindingFactory.MultipleCreate(types, bindingTypes);
        }

        #endregion

        #region GetBinding

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
        virtual public IList<IBinding> GetBindingsByType<T>()
        {
            return GetBindingsByType(typeof(T));
        }

        /// <summary>
        /// 根据类型获取储存容器中的所有同类型 Binding
        /// </summary>
		virtual public IList<IBinding> GetBindingsByType(Type type)
        {
            if (!typeBindings.ContainsKey(type)) { return null; }
            return typeBindings[type];
        }

        /// <summary>
        /// 获取储存容器中所有指定 id 的 binding
        /// </summary>
        virtual public IList<IBinding> GetBindingsById(object id)
        {
            if (!idBindings.ContainsKey(id)) { return null; }
            return idBindings[id];
        }

        /// <summary>
        /// 获取 binder 中所有的 Binding
        /// </summary>
		virtual public IList<IBinding> GetAllBindings()
        {
            List<IBinding> bindingList = new List<IBinding>();

            // 获取 typeBindings 中所有的 binding
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
        virtual public IList<IBinding> GetSameNullIdBinding(IBinding binding)
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
            return bindingStorage[type][id];
        }

        #endregion

        #region Unbind

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindByType<T>()
        {
            UnbindByType(typeof(T));
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindByType(Type type)
        {
            var bindings = GetBindingsByType(type);

            if (bindings != null && bindings.Count != 0)
            {
                // 如果 AOT 前置委托不为空就执行它
                if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

                int length = bindings.Count;
                for (int i = 0; i < length; i++)
                {
                    RemoveBinding(bindings[0]);
                }
                /* 使用 while 在未知情况的综合作用下会死循环（表现为同时测试所有单元时会导致U3D无相应）
                while (bindings.Count > 0)
                {
                    RemoveBinding(bindings[0]);
                }*/

                // 如果 AOT 后置委托不为空就执行它
                if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
            }
        }

        /// <summary>
        /// 根据 id 从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindById(object id)
        {
            var bindings = GetBindingsById(id);

            if (bindings != null && bindings.Count != 0)
            {
                // 如果 AOT 前置委托不为空就执行它
                if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

                int length = bindings.Count;
                for (int i = 0; i < length; i++)
                {
                    // bindings 是直接获取的 typeBindings 中的引用，当 typeBindings 中的引用被
                    // 移除后 bindings 的数量就会跟随着发生变化，GetBindingsByType 方法也是直接返
                    // 回 typeBindings[type],所以一样会随着元素被删除而变化长度，所以每次移除第一个
                    // 元素就可以将所有的元素都移除干净
                    RemoveBinding(bindings[0]);
                }

                // 如果 AOT 后置委托不为空就执行它
                if (afterRemoveBinding != null) { afterRemoveBinding(this, bindings); }
            }
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdBindingByType<T>()
        {
            UnbindNullIdBindingByType(typeof(T));
        }

        /// <summary>
        /// 根据类型从所有容器中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdBindingByType(Type type)
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
                    // 移除后 bindings 的数量就会跟随着发生变化，GetBindingsByType 方法也是直接返
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
            var bindings = new List<IBinding>() { bindingStorage[type][id] };

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null) { beforeRemoveBinding(this, bindings); }

            // 如果获取到了 binding 就进行删除
            if (bindings[0] != null)
            {
                bindingStorage[type].Remove(id);
                idBindings[id].Remove(bindings[0]);
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
                bindingStorage[binding.type].Remove(binding.id);
                idBindings[binding.id].Remove(binding);
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
            if (binding == null)
            {
                throw new BindingSystemException (
                    string.Format(BindingSystemException.NULL_PARAMETER,
                    "[IBinding binding]",
                    "[Storing]"));
            }

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

            if (binding.id == null)
            {
                // 如果尚未被添加过才添加到 typeBindings 中
                if (!exist) { typeBindings[binding.type].Add(binding); }
            }
            // 不为空时将引用储存到 bindingStorage 和 idBindings
            else
            {
                // 如果不存在才添加到 typeBindings
                if (!exist) { typeBindings[binding.type].Add(binding); }

                // 如果已有 type 和 id 都相同的 binding，且它们不是同一个对象,就抛出异常 
                if (bindingStorage[binding.type].Contains(binding.id) &&
                    bindingStorage[binding.type][binding.id] != binding)
                {
                    throw new BindingSystemException(BindingSystemException.SAME_BINDING);
                }

                // 引用添加到 bindingStorage 以便根据 type 和 id 快速检索
                bindingStorage[binding.type][binding.id] = binding;

                // 引用添加到 idBindings 以便获取同 id 的所有 binding
                if (!idBindings.ContainsKey(binding.id))
                {
                    idBindings.Add(binding.id, new List<IBinding>());
                }
                if (!idBindings[binding.id].Contains(binding))
                {
                    idBindings[binding.id].Add(binding);
                }
            }
        }
    }
}