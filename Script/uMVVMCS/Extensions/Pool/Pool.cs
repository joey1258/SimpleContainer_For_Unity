/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    /// <summary>
    /// 泛型对象池类
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class Pool<T> : Pool, IPool<T>
    {
        #region constructor

        /// <summary>
        /// 构造函数 设置对象池的类型
        /// </summary>
        public Pool() : base()
        {
            type = typeof(T);
        }

        #endregion

        /// <summary>
        /// IPool<T>接口成员，如果对象池中有可用的实例则返回该实例
        /// </summary>
		new public T GetInstance()
        {
            return (T)base.GetInstance(true, true);
        }

        /// <summary>
        /// IPool<T>接口成员，如果对象池中有可用的实例则返回该实例
        /// </summary>
        new public T GetInstance(bool doublue)
        { return (T)base.GetInstance(doublue, true); }

        /// <summary>
        /// IPool<T>接口成员，如果对象池中有可用的实例则返回该实例
        /// </summary>
        new public T GetInstance(bool doublue, bool throwException)
        { return (T)base.GetInstance(doublue, throwException); }
    }

    /// <summary>
    /// 对象池类
    /// </summary>
	public class Pool : IPool
    {
        /// <summary>
        /// 对象池中储存实例的栈，先进后出。
        /// </summary>
        protected Stack instancesUnUse = new Stack();

        /// <summary>
        /// 从对象池取出的对象的 HashSet，无序存储
        /// </summary>
        protected HashSet<object> instancesInUse = new HashSet<object>();

        #region property

        /// <summary>
        /// 储存对象池的类型（由第一个放入对象池的对象类型决定）
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// 构造新实例用的构造函数数组
        /// </summary>
        public Object[] parameters { get; set; }

        /// <summary>
        /// 如果对象池中有可用实例则返回一个，否则返回空
        /// </summary>
        public object value
        {
            get
            {
                return GetInstance();
            }
        }

        /// <summary>
        /// 返回当前由对象池管理的实例总数
        /// </summary>
        public int instanceCount
        {
            get
            {
                return _instanceCount;
            }
        }
        protected int _instanceCount;

        /// <summary>
        /// 返回instancesAvailable栈中元素的总数
        /// </summary>
        public int availableCount
        {
            get
            {
                return instancesUnUse.Count;
            }
        }

        /// <summary>
        /// 获取或设置对象池可以储存多少个对象，设置为0表示无限。
        /// </summary>
        virtual public int size { get; set; }

        /// <summary>
        /// 是否保留对象，保留为真
        /// </summary>
        public bool retain { get; set; }

        #endregion

        #region constructor

        public Pool()
        {
            // 设置大小为无限
            size = 0;
        }

        #endregion

        #region IPool implementation  实现IPool接口成员

        /// <summary>
        /// 增加一个元素
        /// </summary>
        public IPool Add(object value)
        {
            // 如果参数类型与对象池类型不同就抛出错误
            if (value.GetType() != type)
            {
                throw new PoolSystemException(PoolSystemException.TYPE_NOT_ASSIGNABLE);
            }

            // 当前由对象池管理的实例总数累加
            _instanceCount++;
            // 将参数 value 压入栈中
            instancesUnUse.Push(value);
            
            return this;
        }

        /// <summary>
        /// 增加多个元素
        /// </summary>
        public IPool Add(object[] list)
        {
            // 遍历参数中的每一个元素，为其调用add方法，将其压入栈中
            int length = list.Length;
            for (int i = 0; i < length; i++)
            {
                Add(list[i]);
            }
            
            return this;
        }

        /// <summary>
        /// 移除一个元素
        /// </summary>
        public IPool Remove(object value)
        {
            // 当前由对象池管理的实例总数递减
            _instanceCount--;
            // 调用removeInstance方法，将参数从HashSet中或者栈中删除
            removeInstance(value);
            
            return this;
        }

        /// <summary>
        /// 移除多个元素
        /// </summary>
        public IPool Remove(object[] list)
        {
            // 遍历参数，为每一个元素调用Remove方法移除元素
            int length = list.Length;
            for (int i = 0; i < length; i++)
            {
                Remove(list[i]);
            }
            
            return this;
        }

        #endregion

        #region 


        #endregion

        #region IPool implementation 实现IPool接口成员

        /// <summary>
        /// 返回一个实例，将其从 instancesAvailable 栈中推出，并存入 HashSet 中
        /// 如果对象池是空的，就新建实例，如果不符合任何条件，返回空
        /// </summary>
        public object GetInstance()
        { return GetInstance(true, true); }

        /// <summary>
        /// 返回一个实例，将其从 instancesAvailable 栈中推出，并存入 HashSet 中
        /// 如果对象池是空的，就新建实例，如果不符合任何条件，返回空
        /// </summary>
        public object GetInstance(bool doublue)
        { return GetInstance(doublue, true); }

        /// <summary>
        /// 返回一个实例，将其从 instancesAvailable 栈中推出，并存入 HashSet 中
        /// 如果对象池是空的，就新建实例，如果不符合任何条件，返回空
        /// </summary>
        public object GetInstance(bool doublue, bool throwException)
        {
            int instancesToCreate = 1;
            if (doublue){ instancesToCreate = instanceCount; }

            #region 当闲置实例栈中有闲置实例时将其栈顶的元素腿出，并返回、加入使用中实例栈

            // 如果 instancesAvailable 栈中存有实例
            if (instancesUnUse.Count > 0)
            {
                // 将其从栈中推出，并存入 instancesInUse 中
                object retv = instancesUnUse.Pop();
                instancesInUse.Add(retv);
                // 返回实例
                return retv;
            }

            #endregion

            #region 如果没有闲置实例，先对对象池的大小和需要创建的数量进行过滤

            // 如果对象池的大小不是无限（size 为 0 代表对象池无限大）
            if (size > 0)
            {
                // 如果实例数量为 0 代表是一个需要实例化实例的新对象池
                if (instanceCount == 0)
                {
                    //设置需要创建的实例数量为对象池的最大数量
                    instancesToCreate = size;
                }
                else
                {
                    if (throwException) { throw new PoolSystemException(PoolSystemException.POOL_REQUESTS_EXCESS); }
                }
            }

            #endregion

            #region 创建指定数量的实例并压入栈内，最后返回栈顶元素

            // 如果要创建的数量大于0
            if (instancesToCreate > 0)
            {
                // 如果储存用于实现对象池功能的对象实例的变量instanceProvider为空抛出一个错误
                if (parameters == null | parameters.Length == 0)
                {
                    // 循环创建对象池同类型对象(构造函数无参数)并压入instancesAvailable栈中
                    for (int a = 0; a < instancesToCreate; a++)
                    {
                        object newInstance = Activator.CreateInstance(type);
                        Add(newInstance);
                    }
                }
                else
                {
                    // 循环创建对象池同类型对象(构造函数有参数)并压入instancesAvailable栈中
                    for (int a = 0; a < instancesToCreate; a++)
                    {
                        object newInstance = Activator.CreateInstance(type, parameters);
                        Add(newInstance);
                    }
                }

                // 最后返回一个实例，将其从instancesAvailable栈中推出，并存入instancesInUse HashSet中
                return GetInstance();
            }

            #endregion

            // 如果数量为0，则返回空
            return null;
        }

        /// <summary>
        /// 将一个实例返回到对象池
        /// </summary>
		public void ReturnInstance(object value)
        {
            // 如果instancesInUse中有参数传入的值
            if (instancesInUse.Contains(value))
            {
                // 如果参数是IPoolable类型
                if (value is IPool)
                {
                    //调用IPoolable接口中的重置对象的方法
                    (value as IPool).Restore();
                }
                //从instancesInUse移除参数value，并重新压回instancesAvailable栈中
                instancesInUse.Remove(value);
                instancesUnUse.Push(value);
            }
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clean()
        {
            //清空instancesAvailable栈、重置instancesInUse、设置储存的实例总数为0
            instancesUnUse.Clear();
            instancesInUse = new HashSet<object>();
            _instanceCount = 0;
        }

        #endregion

        #region IPoolable implementation  实现IPoolable接口成员

        /// <summary>
        /// 重置
        /// </summary>
        public void Restore()
        {
            //清空对象池，设置大小为无限
            Clean();
            size = 0;
        }

        /// <summary>
        /// 设置为保留对象
        /// </summary>
        public void Retain()
        {
            retain = true;
        }

        /// <summary>
        /// 设置为不保留对象
        /// </summary>
        public void Release()
        {
            retain = false;
        }

        #endregion

        /// <summary>
        /// 从对象池中删除一个实例
        /// </summary>
        protected void removeInstance(object value)
        {
            // 如果参数的类型和对象池的类型不符抛出一个错误
            if(value.GetType() != type)
            {
                throw new PoolSystemException(PoolSystemException.TYPE_NOT_ASSIGNABLE);
            }

            // 如果还在使用中就先从使用 HashSet 移除
            if (instancesInUse.Contains(value))
            {
                instancesInUse.Remove(value);
            }
            // 从栈中删除
            else
            {
                instancesUnUse.Pop();
            }
        }
    }
}