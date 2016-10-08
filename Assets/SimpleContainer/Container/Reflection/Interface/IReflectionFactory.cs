using System;

namespace ToluaContainer.Container
{
    public interface IReflectionFactory
    {
        /// <summary>
        /// 创建一个指定类型的
        /// </summary>
        ReflectionInfo Create(Type type);
    }
}