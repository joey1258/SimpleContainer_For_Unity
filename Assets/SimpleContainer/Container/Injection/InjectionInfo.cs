using System;

namespace ToluaContainer.Container
{
    public class InjectionInfo
    {
        /// <summary>
        /// 需要注入的成员枚举 （None | Constructor | Field | Property）
        /// </summary>
        public InjectionInto member;

        /// <summary>
        /// 注入成员的类型
        /// </summary>
        public Type memberType;

        /// <summary>
        /// 对应 binding 的 id 属性，储存于Inject等特性中，用于注入时通过比较是否相同来确认身份
        /// </summary>
        public object id;

        /// <summary>
        /// 需要注入的对象的类型
        /// </summary>
        public Type parentType;

        /// <summary>
        /// 需要注入的对象的实例
        /// </summary>
        public object parentInstance;

        /// <summary>
        /// 被注入对象的类型
        /// </summary>
        public Type injectType;
    }
}