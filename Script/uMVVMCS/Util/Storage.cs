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

namespace uMVVMCS
{
    /// <summary>
    /// 双key储存结构，为避免误覆盖屏蔽了Add方法；
    /// 根据 type 取值时自动创建空值，无需 Contains 判断；
    /// 根据 type 和 id 设置自动创建空值，时无需 Contains 判断，但取值时仍需与字典一样先 Contains 判断以免错误。
    /// </summary>
    public class Storage<T>
    {
        public Items<T> this[Type type] { get { return Get(type); } }

        public int Count { get { return storage.Count; } }

        public Dictionary<Type, Items<T>>.KeyCollection Keys { get { return storage.Keys; } }

        public Dictionary<Type, Items<T>>.ValueCollection Values { get { return storage.Values; } }

        protected Dictionary<Type, Items<T>> storage;

        #region constructor

        public Storage() { storage = new Dictionary<Type, Items<T>>(); }

        #endregion

        /// <summary>
        /// 添加指定的类型
        /// </summary>
        protected void Add(Type type)
        {
            if (type == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER, 
                    "[Type type]",
                    "[Add]"));
            }

            if (!Contains(type))
            {
                storage.Add(type, new Items<T>());
            }
        }

        /// <summary>
        /// 移除指定类型的 BindingStorage
        /// </summary>
        public void Remove(Type type)
        {
            if (type == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (Contains(type))
            {
                storage.Remove(type);
            }
        }

        /// <summary>
        /// 获取当前是否储存有指定类型的 BindingStorage，传入参数为空时也返回 false
        /// </summary>
        public bool Contains(Type type)
        {
            if (type == null) { return false; }

            return storage.ContainsKey(type);
        }

        /// <summary>
        /// 获取指定类型的 BindingStorage
        /// </summary>
        public Items<T> Get(Type type)
        {
            if (type == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (!Contains(type)) { Add(type); }

            return storage[type];
        }

    }

    public class Items<T>
    {
        public T this[object id]
        {
            get { return Get(id); }
            set { Set(id, value); }
        }

        public int Count { get { return items.Count; } }

        public Dictionary<object, T>.KeyCollection Keys { get { return items.Keys; } }

        public Dictionary<object, T>.ValueCollection Values { get { return items.Values; } }

        protected Dictionary<object, T> items;

        #region constructor

        public Items() { items = new Dictionary<object, T>(); }

        #endregion

        /// <summary>
        /// 添加指定 ID 
        /// </summary>
        protected void Add(object id)
        {
            if (id == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (!Contains(id)) { items.Add(id, default(T)); }
        }

        /// <summary>
        /// 移除指定 ID 的 IBinding
        /// </summary>
        public void Remove(object id)
        {
            if (id == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (Contains(id)) { items.Remove(id); }
        }

        /// <summary>
        /// 获取当前是否储存有指定 ID 的 IBinding，传入参数为空时也返回 false
        /// </summary>
        public bool Contains(object id)
        {
            if (id == null) { return false; }

            return items.ContainsKey(id);
        }

        /// <summary>
        /// 根据 ID 获取 IBinding
        /// </summary>
        public T Get(object id)
        {
            if (id == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (!Contains(id)) { return default(T); }

            return items[id];
        }

        /// <summary>
        /// 将指定 ID 的 IBinding 存入 items
        /// </summary>
        public void Set(object id, T value)
        {
            if(id == null || value == null)
            {
                throw new UtilsSystemException(
                    string.Format(UtilsSystemException.NULL_PARAMETER,
                    "[Type type]",
                    "[Add]"));
            }

            if (!Contains(id)) { Add(id); }

            items[id] = value;
        }
    }
}