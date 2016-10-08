using System;

namespace ToluaContainer
{
    /// <summary>
    /// [Inject]标记需要注入的对象，可以在构造函数中传入一个 object 类型的值作为 id
    /// 例如：“[Inject(SomeEnum.VALUE)]”
    /// </summary>
    [AttributeUsage
        (AttributeTargets.Constructor | 
        AttributeTargets.Field | 
        AttributeTargets.Property |
        AttributeTargets.Method | 
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
    /// [Priority]标记一个方法的优先级，以便决定执行顺序。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
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