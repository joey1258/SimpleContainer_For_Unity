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
using System.Collections.Generic;
using System.Reflection;

namespace uMVVMCS.DIContainer
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