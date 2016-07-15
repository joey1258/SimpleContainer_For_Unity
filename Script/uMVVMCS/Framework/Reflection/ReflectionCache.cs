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
    /// <summary>
    /// Storage<T> 是为了实现双 Key 检索，这里并不需要两个 Key 检索，所以单独实现
    /// </summary>
    public class ReflectionCache : IReflectionCache
    {
        protected Dictionary<Type, ReflectionInfo> infos;

        protected IReflectionFactory reflectionFactory;

        #region constructor

        public ReflectionCache()
        {
            infos = new Dictionary<Type, ReflectionInfo>();
            reflectionFactory = new ReflectionFactory();
        }

        #endregion

        #region property

        /// <summary>
        /// 用 type 来索引的索引器
        /// </summary>
        public ReflectionInfo this[Type type] { get { return this.GetInfo(type); } }

        #endregion

        #region functions

        /// <summary>
        /// 为缓存字典添加指定类型的反射类实例
        /// </summary>
        /// <param name="type">Type to be added.</param>
        public void Add(Type type)
        {
            if (type == null)
            {
                return;
            }

            if (!this.Contains(type))
            {
                this.infos.Add(type, reflectionFactory.Create(type));
            }
        }

        /// <summary>
        /// 从缓存字典中移除指定类型的实例
        /// </summary>
        /// <param name="type">Type to be removed.</param>
        public void Remove(Type type)
        {
            if (this.Contains(type))
            {
                this.infos.Remove(type);
            }
        }

        /// <summary>
        /// 从缓存字典中获取指定类型的反射类实例
        /// </summary>
        public ReflectionInfo GetInfo(Type type)
        {
            if (!this.Contains(type))
            {
                this.Add(type);
            }

            return this.infos[type];
        }

        /// <summary>
        /// 返回当前缓存字典中是否存在指定类型的实例
        /// </summary>
        public bool Contains(Type type)
        {
            return this.infos.ContainsKey(type);
        }

        /// <summary>
        /// 缓存 binder 中所有 binding 中 value 属性所储存的类型
        /// </summary>
        public void CacheFromBinder(IBinder binder)
        {
            var bindings = binder.GetAllBindings();

            // 遍历所有 binding，如果是 TEMP 类型则缓存其 Type 类型的值；
            // 如果是 SINGLETON 类型，缓存 value 属性的值的类型
            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];

                if (binding.bindingType == BindingType.TEMP && binding.value is Type)
                {
                    this.Add(binding.value as Type);
                }
                else if (binding.bindingType == BindingType.SINGLETON)
                {
                    this.Add(binding.value.GetType());
                }
            }
        }

        #endregion
    }
}