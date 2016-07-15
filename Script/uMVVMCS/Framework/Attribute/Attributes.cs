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
 * id 对应 binding 的 id 属性，binding 的 id 属性只为快速在字典中获取，从而避免 for 或者 foreach
 */

using System;

namespace uMVVMCS
{
    /// <summary>
    /// [Inject]标记需要注入的对象，可以在构造函数中传入一个 object 类型的值作为 id
    /// 例如：“[Inject(SomeEnum.VALUE)]”
    /// </summary>
    [AttributeUsage
        (AttributeTargets.Field |
        AttributeTargets.Property | 
        AttributeTargets.Parameter, 
        AllowMultiple = false, 
        Inherited = true)]
    public class Inject : Attribute
    {
        public object id;

        #region constructor

        public Inject() { id = null; }

        public Inject(object o) { id = o; }

        #endregion
    }

    /// <summary>
    /// [Construct]特性标首选构造函数。如果没有加上这个属性，Reflector 将会挑选一个参数最短的构造函数。
    /// 很明显的，如果只有一个构造函数，就没有必要使用这个特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public class Construct : Attribute { }

    /// <summary>
    /// [PostConstruct]标记一个方法，使它在注入后被立即调用。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PostConstruct : Attribute { }

    /// <summary>
    /// [Priority]标记一个方法的优先级，以便决定执行顺序。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class Priority : Attribute
    {
        public int priority;

        #region constructor

        public Priority() { }

        public Priority(int p) { priority = p; }

        #endregion
    }

    /// <summary>
    /// 标记 MonoBehaviour 只能从指定 id 的容器中获得注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class InjectFromContainer : Attribute
    {
        public object id;

        #region constructor

        public InjectFromContainer(object id) { this.id = id; }

        #endregion
    }
}