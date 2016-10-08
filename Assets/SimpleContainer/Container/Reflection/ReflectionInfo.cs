using System;

namespace ToluaContainer.Container
{
    public class ReflectionInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public Constructor constructor { get; set; }

        /// <summary>
        /// 有参数构造函数
        /// </summary>
        public ParamsConstructor paramsConstructor { get; set; }

        /// <summary>
        /// 构造函数的参数
        /// </summary>
        public ParameterInfo[] constructorParameters { get; set; }

        /// <summary>
        /// 接受注入的方法
        /// </summary>
        public MethodInfo[] methods { get; set; }

        /// <summary>
        /// 接受注入的公共属性
        /// </summary>
        public SetterInfo[] properties { get; set; }

        /// <summary>
        /// 接受注入的公共字段
        /// </summary>
        public SetterInfo[] fields { get; set; }
    }
}