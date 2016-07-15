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

namespace uMVVMCS.DIContainer
{
    public interface IReflectionCache
    {
        /// <summary>
        /// 为当前缓存的 ReflectInfo 实现类型索引
        /// </summary>
        ReflectionInfo this[Type type] { get; }

        /// <summary>
        /// 添加一个类型的缓存
        /// </summary>
        void Add(Type type);

        /// <summary>
        /// 移除一个类型的缓存
        /// </summary>
        void Remove(Type type);

        /// <summary>
        /// 获取指定类型的 ReflectInfo 类型缓存
        /// </summary>
        ReflectionInfo GetInfo(Type type);

        /// <summary>
        /// 查询指定类型的缓存是否存在
        /// </summary>
        bool Contains(Type type);

        /// <summary>
        /// 为 binder 中的所有 binding 生成缓存
        /// </summary>
        void CacheFromBinder(IBinder binder);
    }
}