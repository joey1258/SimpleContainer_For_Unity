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
        /// <summary>
        /// 构造函数 设置对象池的类型
        /// </summary>
		public Pool() : base()
        {
            poolType = typeof(T);
        }
        /// <summary>
        /// IPool<T>接口成员，如果对象池中有可用的实例则返回该实例
        /// </summary>
		new public T GetInstance()
        {
            return (T)base.GetInstance();
        }
    }

    /// <summary>
    /// 对象池类
    /// </summary>
	public class Pool : IPool
    {
        /// <summary>
        /// 对象池中储存实例的栈，先进后出。
        /// </summary>
        protected Stack instancesAvailable = new Stack();

        /// <summary>
        /// 从对象池取出的对象的HashSet，无序存储
        /// </summary>
        protected HashSet<object> instancesInUse = new HashSet<object>();

        /// <summary>
        /// 当前由对象池管理的实例总数
        /// </summary>
        protected int _instanceCount;

        /// <summary>
        /// 构造函数 ISemiBinding接口属性 
        /// </summary> 
        public Pool() : base()
        {
            // 设置大小为无限
            size = 0;
            // 约束类型为对象池,uniqueValues为真(二次约束保证SemiBinding永远不会包含复数个相同的值)
            constraint = BindingConstraintType.POOL;
            uniqueValues = true;
            // 设定溢出模式为异常——请求超出固定大小将抛出一个异常
            overflowBehavior = PoolOverflowBehavior.EXCEPTION;
            // 设置扩展模式为当动态对象池扩展时，加入双倍数量到对象池
            inflationType = PoolInflationType.DOUBLE;
        }

        #region IManagedList implementation  实现IManagedList接口成员

        /// 增加一个元素
        virtual public IManagedList Add(object value)
        {
            // 如果参数类型与对象池类型不同就抛出错误
            failIf(value.GetType() != poolType, "Pool Type mismatch. Pools must consist of a common concrete type.\n\t\tPool type: " + poolType.ToString() + "\n\t\tMismatch type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
            // 当前由对象池管理的实例总数累加
            _instanceCount++;
            // 将参数value压入栈中
            instancesAvailable.Push(value);
            // 返回当前对象池实例
            return this;
        }

        /// 增加多个元素
        virtual public IManagedList Add(object[] list)
        {
            //遍历参数中的每一个元素，为其调用add方法，将其压入栈中
            foreach (object item in list)
                Add(item);
            //返回当前对象池实例
            return this;
        }

        /// 移除一个元素
        virtual public IManagedList Remove(object value)
        {
            // 当前由对象池管理的实例总数递减
            _instanceCount--;
            // 调用removeInstance方法，将参数从HashSet中或者栈中删除
            removeInstance(value);
            // 返回当前对象池实例
            return this;
        }

        /// 移除多个元素
        virtual public IManagedList Remove(object[] list)
        {
            // 遍历参数，为每一个元素调用Remove方法移除元素
            foreach (object item in list)
                Remove(item);
            // 返回当前对象池元素
            return this;
        }

        /// 属性，如果对象池中有可用实例则返回一个，否则返回空
        virtual public object value
        {
            get
            {
                return GetInstance();
            }
        }
        #endregion

        #region ISemiBinding region 实现ISemiBinding接口成员  虽然对象池类并没有继承ISemiBinding接口，但对象池储存的对象可能需要用到该接口中的成员

        /// 为真时二次约束保证SemiBinding永远不会包含复数个相同的值
        virtual public bool uniqueValues { get; set; }
        /// 属性，获取或者设置约束，它是一个枚举类型
		virtual public Enum constraint { get; set; }

        #endregion

        #region IPool implementation 实现IPool接口成员

        /// The object Type of the first object added to the pool.
        /// Pool objects must be of the same concrete type. This property enforces that requirement. 
        /// 属性，储存对象池的类型（由第一个放入对象池的对象类型决定）
        public System.Type poolType { get; set; }

        /// 属性，返回当前由对象池管理的实例总数
        public int instanceCount
        {
            get
            {
                return _instanceCount;
            }
        }

        /// 返回一个实例，将其从instancesAvailable栈中推出，并存入instancesInUse HashSet中
        /// 如果对象池是空的，就新建实例，如果不符合任何条件，返回空
        virtual public object GetInstance()
        {
            // Is an instance available?
            // 如果instancesAvailable栈中存有实例
            if (instancesAvailable.Count > 0)
            {
                //将其从栈中推出，并存入instancesInUse HashSet中
                object retv = instancesAvailable.Pop();
                instancesInUse.Add(retv);
                // 返回实例
                return retv;
            }

            // 新建一个临时变量instancesToCreate来储存需要创建的实例数量
            int instancesToCreate = 0;

            //New fixed-size pool. Populate.
            // 如果对象池的大小不是无限
            if (size > 0)
            {
                // 如果实例数量为0
                if (instanceCount == 0)
                {
                    //New pool. Add instances.
                    //设置需要创建的实例数量为对象池的最大数量
                    instancesToCreate = size;
                }
                else// 否则
                {
                    //Illegal overflow. Report and return null 
                    // 如果溢出就抛出一个异常
                    failIf(overflowBehavior == PoolOverflowBehavior.EXCEPTION,
                        "A pool has overflowed its limit.\n\t\tPool type: " + poolType,
                        PoolExceptionType.OVERFLOW);
                    // 如果溢出模式为警告则打印一行信息
                    if (overflowBehavior == PoolOverflowBehavior.WARNING)
                    {
                        Console.WriteLine("WARNING: A pool has overflowed its limit.\n\t\tPool type: " + poolType, PoolExceptionType.OVERFLOW);
                    }
                    // 返回空
                    return null;
                }
            }
            else //否则（对象池为无限大时)
            {
                //Zero-sized pools will expand.
                // 如果实例数量为0或者无限对象池扩展类型为每次增加1个，就将需要创建的数量设置为1
                if (instanceCount == 0 || inflationType == PoolInflationType.INCREMENT)
                {
                    instancesToCreate = 1;
                }
                else // 否则将需要创建的数量设置为当前对象池中对象的总数
                {
                    instancesToCreate = instanceCount;
                }
            }
            // 如果要创建的数量大于0
            if (instancesToCreate > 0)
            {
                // 如果储存用于实现对象池功能的对象实例的变量instanceProvider为空抛出一个错误
                failIf(instanceProvider == null, "A Pool of type: " + poolType + " has no instance provider.", PoolExceptionType.NO_INSTANCE_PROVIDER);
                // 循环调用instanceProvider的GetInstance方法创建对象池同类型对象，并压入instancesAvailable栈中
                for (int a = 0; a < instancesToCreate; a++)
                {
                    object newInstance = instanceProvider.GetInstance(poolType);
                    Add(newInstance);
                }
                // 最后返回一个实例，将其从instancesAvailable栈中推出，并存入instancesInUse HashSet中
                return GetInstance();
            }

            //If not, return null
            return null;
        }

        /// 将一个实例返回到对象池
		virtual public void ReturnInstance(object value)
        {
            // 如果instancesInUse中有参数传入的值
            if (instancesInUse.Contains(value))
            {
                // 如果参数是IPoolable类型
                if (value is IPoolable)
                {
                    //调用IPoolable接口中的重置对象的方法
                    (value as IPoolable).Restore();
                }
                //从instancesInUse移除参数value，并重新压回instancesAvailable栈中
                instancesInUse.Remove(value);
                instancesAvailable.Push(value);
            }
        }

        /// 清空对象池
        virtual public void Clean()
        {
            //清空instancesAvailable栈、重置instancesInUse、设置储存的实例总数为0
            instancesAvailable.Clear();
            instancesInUse = new HashSet<object>();
            _instanceCount = 0;
        }

        /// 属性，返回instancesAvailable栈中元素的总数
        virtual public int available
        {
            get
            {
                return instancesAvailable.Count;
            }
        }

        /// 属性，获取或设置对象池可以储存多少个对象，设置为0表示无限。
        virtual public int size { get; set; }

        /// 溢出警告模式
        virtual public PoolOverflowBehavior overflowBehavior { get; set; }

        /// 无限对象池扩展模式
        virtual public PoolInflationType inflationType { get; set; }

        #endregion

        #region IPoolable implementation  实现IPoolable接口成员

        /// 重置
        public void Restore()
        {
            //清空对象池，设置大小为无限
            Clean();
            size = 0;
        }

        /// 设置为保留对象
        public void Retain()
        {
            retain = true;
        }

        /// 设置为不保留对象
        public void Release()
        {
            retain = false;
        }

        /// 是否保留对象，保留为真
        public bool retain { get; set; }

        #endregion

        /// <summary>
        /// Permanently removes an instance from the Pool
        /// 从对象池中永久删除一个实例
        /// </summary>
        /// In the event that the removed Instance is in use, it is removed from instancesInUse.
        /// Otherwise, it is presumed inactive, and the next available object is popped from
        /// instancesAvailable.
        /// 如果删除的实例正在使用，则它从instancesInUse HashSet中删除，否则从instancesAvailable栈中推出
        /// <param name="value">An instance to remove permanently from the Pool.</param>
        virtual protected void removeInstance(object value)
        {
            // 如果参数的类型和对象池的类型不符抛出一个错误
            failIf(value.GetType() != poolType, "Attempt to remove a instance from a pool that is of the wrong Type:\n\t\tPool type: " + poolType.ToString() + "\n\t\tInstance type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
            // 如果储存使用中的实例的HashSet中有参数传入的实例，从HashSet中删除
            if (instancesInUse.Contains(value))
            {
                instancesInUse.Remove(value);
            }
            //否则从栈中推出
            else
            {
                instancesAvailable.Pop();
            }
        }

        /// 抛出错误信息
        protected void failIf(bool condition, string message, PoolExceptionType type)
        {
            if (condition)
            {
                throw new PoolException(message, type);
            }
        }
    }
}