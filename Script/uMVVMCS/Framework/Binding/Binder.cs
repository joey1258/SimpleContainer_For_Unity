/*
 * Copyright 2016 Sun Ning
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
using uMVVMCS;

namespace uMVVMCS.DIContainer
{
    public class Binder : IBinder
    {
        /// <summary>
        /// 用于储存 binding 到 binder 字典的委托
        /// </summary>
        public delegate void BindingStoring(IBinding binding);

        protected Storage<IBinding> bindingStorage;
        protected Dictionary<Type, IList<IBinding>> nullIdBindings;
        protected IBindingFactory bindingFactory;

        #region constructor

        public Binder()
        {
            bindingStorage = new Storage<IBinding>();
            nullIdBindings = new Dictionary<Type, IList<IBinding>>();
            bindingFactory = new BindingFactory();
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
        /// 返回一个新的Binding实例,并设置指定类型给 type 属性, BindingType 属性为 TEMP，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind<T>()
        {
            return Bind(typeof(T), BindingType.TEMP);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 SINGLETON，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding BindSingleton<T>()
        {
            return bindingFactory.CreateSingle(Storing, typeof(T), BindingType.SINGLETON);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并设置指定类型给 type, BindingType 为 FACTORY，值约束为 SINGLE
        /// </summary>
        virtual public IBinding BindFactory<T>()
        {
            return bindingFactory.CreateSingle(Storing, typeof(T), BindingType.FACTORY);
        }

        /// <summary>
        /// 返回一个新的Binding实例，并把设置参数分别给 type 和 BindingType ，值约束为 MULTIPLE
        /// </summary>
        virtual public IBinding Bind(Type type, BindingType bindingType)
        {
            return bindingFactory.Create(Storing, type, bindingType);
        }

        /// <summary>
        /// 未完成，待构思
        /// 返回多个新的Binding实例,并把设置参数分别给 type 属性和 BindingType 属性
        /// </summary>
        virtual public IList<IBinding> Bind(Type[] types, BindingType[] bindingTypes)
        {
            var bindings = new List<IBinding>();

            bool notNull = (types != null && bindingTypes != null);
            bool sameLength = (types.Length != 0 && bindingTypes.Length != 0 && types.Length == bindingTypes.Length);

            if (notNull && sameLength)
            {
                int length = types.Length;
                for (int i = 0; i < length; i++)
                {
                    bindings.Add(bindingFactory.Create(Storing, types[i], bindingTypes[i]));
                }
            }

            return bindings;
        }

        #endregion

        #region GetBinding

        /// <summary>
        /// 根据类型获取 nullIdBindings 字典中的所有同类型 Binding
        /// </summary>
        virtual public IList<IBinding> GetNullIdBindingsByType<T>()
        {
            return GetNullIdBindingsByType(typeof(T));
        }

        /// <summary>
        /// 根据类型获取 nullIdBindings 字典中的所有同类型 Binding
        /// </summary>
		virtual public IList<IBinding> GetNullIdBindingsByType(Type type)
        {
            return nullIdBindings[type];
        }

        /// <summary>
        /// 根据类型获取 nullIdBindings 字典和 bindingStorage 中的所有同类型 Binding
        /// </summary>
        virtual public IList<IBinding> GetBindingsByType<T>()
        {
            return GetBindingsByType(typeof(T));
        }

        /// <summary>
        /// 根据类型获取 nullIdBindings 字典和 bindingStorage 中的所有同类型 Binding
        /// </summary>
		virtual public IList<IBinding> GetBindingsByType(Type type)
        {
            List<IBinding> bindings = new List<IBinding>(nullIdBindings[type]);
            bindings.AddRange(bindingStorage[type].Values);
            return bindings;
        }

        /// <summary>
        /// 获取 bindingStorage 中 所有指定 id 的 binding
        /// </summary>
        virtual public IList<IBinding> GetBindingsById(object id)
        {
            List<IBinding> sameIds = new List<IBinding>();

            List<Type> keys = new List<Type>(bindingStorage.Keys);

            int length = bindingStorage.Count;
            for (int i = 0; i < length; i++)
            {
                if (bindingStorage[keys[i]].Contains(id)) { sameIds.Add(bindingStorage[keys[i]][id]); }
            }

            return sameIds;
        }

        /// <summary>
        /// 获取 binder 中所有无 id 的 Binding
        /// </summary>
		virtual public IList<IBinding> GetAllNullIdBindings()
        {
            List<IBinding> bindingList = new List<IBinding>();

            // 获取 nullIdBindings 中所有的 binding
            List<Type> nullIdKeys = new List<Type>(nullIdBindings.Keys);

            int length = nullIdBindings.Count;
            for (int i = 0; i < length; i++)
            {
                bindingList.AddRange(nullIdBindings[nullIdKeys[i]]);
            }

            return bindingList;
        }

        /// <summary>
        /// 获取 binder 中所有的 Binding
        /// </summary>
		virtual public IList<IBinding> GetAllBindings()
        {
            List<IBinding> bindingList = new List<IBinding>();

            // 获取 bindingStorage 中所有的 binding
            List<Type> keys = new List<Type>(bindingStorage.Keys);

            int length = bindingStorage.Count;
            for (int i = 0; i < length; i++)
            {
                bindingList.AddRange(bindingStorage[keys[i]].Values);
            }

            // 获取 nullIdBindings 中所有的 binding
            List<Type> nullIdKeys = new List<Type>(nullIdBindings.Keys);

            length = nullIdBindings.Count;
            for (int i = 0; i < length; i++)
            {
                bindingList.AddRange(nullIdBindings[nullIdKeys[i]]);
            }

            return bindingList;
        }

        /// <summary>
        /// 返回 nullIdBindings 中除自身以外所有 type 和值都相同的 binding
        /// </summary>
        virtual public IList<IBinding> GetSameNullIdBinding(IBinding binding)
        {
            List<IBinding> bindingList = new List<IBinding>();

            int length = nullIdBindings[binding.type].Count;
            for (int i = 0; i < length; i++)
            {
                if(nullIdBindings[binding.type][i].constraint != binding.constraint) { continue; }

                if (binding.constraint == ConstraintType.MULTIPLE)
                {
                    if (CompareUtils.isSameValueArray(
                        nullIdBindings[binding.type][i].valueArray,
                        binding.valueArray) &&
                        !CompareUtils.isSameObject(nullIdBindings[binding.type][i], binding))
                    {
                        bindingList.Add(nullIdBindings[binding.type][i]);
                    }
                }
                else
                {
                    if (CompareUtils.isSameObject(
                        nullIdBindings[binding.type][i].value,
                        binding.value) &&
                        !CompareUtils.isSameObject(nullIdBindings[binding.type][i], binding))
                    {
                        bindingList.Add(nullIdBindings[binding.type][i]);
                    }
                }
            }

            return bindingList;
        }

        /// <summary>
        /// 根据类型和id获取 bindingStorage 中的 Binding
        /// </summary>
		virtual public IBinding GetBinding<T>(object id)
        {
            return GetBinding(typeof(T), id);
        }

        /// <summary>
        /// 根据类型和id获取 bindingStorage 中的 Binding
        /// </summary>
		virtual public IBinding GetBinding(Type type, object id)
        {
            return bindingStorage[type][id];
        }

        #endregion

        #region Unbind

        /// <summary>
        /// 根据类型从 bindingStorage 和 nullIdBindings 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindByType<T>()
        {
            UnbindByType(typeof(T));
        }

        /// <summary>
        /// 根据类型从 bindingStorage 和 nullIdBindings 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindByType(Type type)
        {
            var bindings = GetBindingsByType(type);

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null)
            {
                beforeRemoveBinding(this, bindings);
            }

            bindingStorage.Remove(type);
            nullIdBindings.Remove(type);

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null)
            {
                afterRemoveBinding(this, bindings);
            }
        }

        /// <summary>
        /// 根据类型从 bindingStorage 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindBindingStorageByType<T>()
        {
            UnbindBindingStorageByType(typeof(T));
        }

        /// <summary>
        /// 根据类型从 bindingStorage 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindBindingStorageByType(Type type)
        {
            var bindings = new List<IBinding>(bindingStorage[type].Values);

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null)
            {
                beforeRemoveBinding(this, bindings);
            }

            bindingStorage.Remove(type);

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null)
            {
                afterRemoveBinding(this, bindings);
            }
        }

        /// <summary>
        /// 根据类型从 nullIdBindings 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdBindingByType<T>()
        {
            UnbindNullIdBindingByType(typeof(T));
        }

        /// <summary>
        /// 根据类型从 nullIdBindings 中删除所有同类型 Binding
        /// </summary>
        virtual public void UnbindNullIdBindingByType(Type type)
        {
            var bindings = nullIdBindings[type];

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null)
            {
                beforeRemoveBinding(this, bindings);
            }

            nullIdBindings.Remove(type);

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null)
            {
                afterRemoveBinding(this, bindings);
            }
        }

        /// <summary>
        /// 根据类型和 id 从 bindingStorage 中删除 Binding
        /// </summary>
		virtual public void Unbind<T>(object id)
        {
            Unbind(typeof(T), id);
        }

        /// <summary>
        /// 根据 type 和 id 从 bindingStorage 中删除 Binding (type 和 id 不可为空)
        /// </summary>
		virtual public void Unbind(Type type, object id)
        {
            // 如果参数 type 或 id 为空，就直接退出
            if (type == null || id == null) { return; }

            var bindings = new List<IBinding>() { bindingStorage[type][id] };

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null)
            {
                beforeRemoveBinding(this, bindings);
            }

            // 如果无冲突字典中含有参数key相同的key
            if (bindingStorage.Contains(type))
            {
                if (bindingStorage[type].Contains(id)) { bindingStorage[type].Remove(id); }
            }

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null)
            {
                afterRemoveBinding(this, bindings);
            }
        }

        /// <summary>
        /// 根据 binding 从 bindingStorage 中删除 Binding
        /// </summary>
        virtual public void Unbind(IBinding binding)
        {
            var bindings = new List<IBinding>() { binding };

            // 如果 AOT 前置委托不为空就执行它
            if (beforeRemoveBinding != null)
            {
                beforeRemoveBinding(this, bindings);
            }

            if (binding == null) { return; }
            RemoveBinding(binding);

            // 如果 AOT 后置委托不为空就执行它
            if (afterRemoveBinding != null)
            {
                afterRemoveBinding(this, bindings);
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// 删除指定 binding 中指定的 value 值，如果移除后 value 属性为空或 value 约束为唯一，就移除该 binding (会否牵连其它同值binding？待测)
        /// </summary>
        virtual public void RemoveValue(IBinding binding, object value)
        {
            // 如果参数 binding 为空，或者参数 value 为空，就直接退出
            if (binding == null || value == null) { return; }

            // 如果约束类型为 SINGLE 直接删除 binding （如果 id 为空，将删除所有同值的无 id binding）
            if (binding.constraint == ConstraintType.SINGLE)
            {
                RemoveBinding(binding);
                return;
            }

            // 移除相同的值
            int length = binding.valueArray.Length;
            for (int i = 0; i < length; i++)
            {
                if (binding.valueArray[i] == value)
                {
                    binding.RemoveValue(value);
                    break;
                }
            }

            // 如果移除后 value 长度为 0，就移除 binding （如果 id 为空，将删除所有同值的无 id binding）
            if (binding.valueArray.Length == 0)
            {
                RemoveBinding(binding);
            }
        }

        /// <summary>
        /// 删除指定 binding 中 value 的多个值，如果移除后 value 属性为空或 value 约束为唯一，就移除该 binding
        /// </summary>
        virtual public void RemoveValues(IBinding binding, IList<object> valueList)
        {
            // 如果参数 binding 为空，或者参数 value 为空，就直接退出
            if (binding == null || 
                valueList == null || 
                valueList.Count == 0 ||
                binding.valueArray.Length < valueList.Count ||
                binding.constraint == ConstraintType.SINGLE) { return; }

            int length = binding.valueArray.Length;
            for (int i = 0; i < length; i++)
            {
                if (binding.valueArray[i] == valueList[i])
                {
                    binding.RemoveValue(valueList[i]);
                    break;
                }
            }

            // 如果移除后值为空则移除 binding
            if (binding.valueArray.Length == 0) { RemoveBinding(binding); }
        }

        /// <summary>
        /// 删除 binding 自身
        /// </summary>
        virtual public void RemoveBinding(IBinding binding)
        {
            // 如果参数 binding 为空，就直接退出
            if (binding == null) { return; }

            // 如果 binding 的 id 为空，就移除 nullIdBindings 中HashCode相同的 binding
            if (binding.id == null)
            {
                RemoveSameNullIdBinding(binding);
            }
            // 如果 binding 的 id 不为空，就根据 binding 的 type 和 id 移除 bindingStorage 中的 binding
            else { bindingStorage[binding.type].Remove(binding.id); }
        }

        /// <summary>
        /// 根据 type 和 id 删除 binding （type 和 id 不可为空）
        /// </summary>
        virtual public void RemoveBinding(Type type, object id)
        {
            // 如果参数 type 或 id 为空，就直接退出
            if (type == null || id == null) { return; }

            if (bindingStorage[type].Contains(id)) { bindingStorage[type].Remove(id); }
        }

        /// <summary>
        /// 移除 nullIdBindings 中所有 type 和值都相同的 binding (如没有则不做修改)
        /// </summary>
        virtual public void RemoveSameNullIdBinding(IBinding binding)
        {
            int length = nullIdBindings[binding.type].Count;

            for (int i = 0; i < length; i++)
            {
                if (nullIdBindings[binding.type][i].constraint != binding.constraint) { continue; }

                if (binding.constraint == ConstraintType.MULTIPLE)
                {
                    if (CompareUtils.isSameValueArray(
                        nullIdBindings[binding.type][i].valueArray,
                        binding.valueArray))
                    {
                        nullIdBindings[binding.type].Remove(nullIdBindings[binding.type][i]);
                        length = nullIdBindings[binding.type].Count;
                    }
                }
                else
                {
                    if (CompareUtils.isSameObject(
                    nullIdBindings[binding.type][i].value,
                    binding.value))
                    {
                        nullIdBindings[binding.type].Remove(nullIdBindings[binding.type][i]);
                        length = nullIdBindings[binding.type].Count;
                    }
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 储存binding
        /// </summary>
        virtual protected void Storing(IBinding binding)
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

            // 添加 binding 到相应的字典或 bindingStorage
            AddBinding(binding);

            // 如果 AOT 后置委托不为空就执行它
            if (this.afterAddBinding != null) { this.afterAddBinding(this, ref binding); }
        }

        /// <summary>
        /// 添加 binding 到相应的字典或 bindingStorage
        /// </summary>
        virtual public void AddBinding(IBinding binding)
        {
            // 如果 id 为空且未添加过相同的binding，就储存到 nullIdBindings
            if (binding.id == null)
            {
                if (!nullIdBindings.ContainsKey(binding.type))
                {
                    nullIdBindings.Add(binding.type, new List<IBinding>());
                }

                // 如果 type 和值都相同的 binding 不存在才进行添加
                if (GetSameNullIdBinding(binding).Count == 0)
                {
                    nullIdBindings[binding.type].Add(binding);
                }
            }
            // 不为空就储存到 bindingStorage
            else
            {
                // 如果已有 type 和 id 都相同的 binding，且它们不是同一个对象,就抛出异常 
                if (bindingStorage[binding.type].Contains(binding.id) && 
                    bindingStorage[binding.type][binding.id] != binding)
                {
                    throw new BindingSystemException(BindingSystemException.SAME_BINDING);
                }

                bindingStorage[binding.type][binding.id] = binding;
                
                // 如果无 nullIdBindings 中有相同的 binding 则移除 nullIdBindings 中相同的 binding
                RemoveSameNullIdBinding(binding);
            }
        }
    }
}