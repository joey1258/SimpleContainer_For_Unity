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
    }

    public interface IPool
    {
        /// <summary>
        /// 当类需要实现对象池功能时，可以通过 InjectionBinder 实现，也可以通过实现IInstanceProvider接口实现该属性，
        /// 来存放InjectionBinder实例（因为IInjectionBinder接口继承了IInstanceProvider接口）或者IInstanceProvider实例
        /// </summary>
        //IInstanceProvider instanceProvider { get; set; }

        /// <summary>
        /// 属性，储存对象池的类型（由第一个放入对象池的对象类型决定）
        /// </summary>
        Type poolType { get; set; }

        /// <summary>
        /// 如果对象池中有可用的实例就按顺序返回
        /// </summary>
        object GetInstance();

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
        int available { get; }

        /// <summary>
        /// 属性，获取或设置对象池可以储存多少个对象 (设置为0表示无限大小，可以无限制的扩展)
        /// </summary>
        int size { get; set; }

        /// <summary>
        /// 属性，返回当前由对象池管理的实例总数
        /// </summary>
        int instanceCount { get; }

        /// <summary>
        /// 属性，获取或设置当请求超过对象池大小时该如何处理：抛出错误、抛出警告，或者忽略
        /// </summary>
        //PoolOverflowBehavior overflowBehavior { get; set; }

        /// <summary>
        /// 获取或者设置无限大小的对象池的扩展行为类型：添加单倍，或者双倍对象
        /// </summary>
        //PoolInflationType inflationType { get; set; }
    }
}