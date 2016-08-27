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
    public interface IPool<T> : IPool
    {
        new T GetInstance();
        new T GetInstance(bool doublue);
        new T GetInstance(bool doublue, bool throwException);
    }

    public interface IPool
    {
        /// <summary>
        /// 属性，储存对象池的类型（由第一个放入对象池的对象类型决定）
        /// </summary>
        Type type { get; set; }

        /// <summary>
        /// 如果对象池中有可用的实例就按顺序返回
        /// </summary>
        object GetInstance();
        object GetInstance(bool doublue);
        object GetInstance(bool doublue, bool throwException);

        /// <summary>
        /// 将一个实例返回到对象池 (如果被释放的实例实现了IPoolable接口，那么应该调用Release()方法)
        /// </summary>
        void ReturnInstance(object value);

        /// <summary>
        /// 清空对象池
        /// </summary>
        void Clean();

        /// <summary>
        /// 返回non-committed实例的数量
        /// </summary>
        int availableCount { get; }

        /// <summary>
        /// 属性，获取或设置对象池可以储存多少个对象 (设置为0表示无限大小，可以无限制的扩展)
        /// </summary>
        int size { get; set; }

        /// <summary>
        /// 属性，返回当前由对象池管理的实例总数
        /// </summary>
        int instanceCount { get; }

        /// <summary>
        /// 重置
        /// </summary>
        void Restore();
    }
}