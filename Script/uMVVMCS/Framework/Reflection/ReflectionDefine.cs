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
    /// <summary>
    /// 无参数构造函数委托
    /// </summary>
    public delegate object Constructor();

    /// <summary>
    /// 有参数构造函数委托
    /// </summary>
    public delegate object ParamsConstructor(object[] parameters);

    /// <summary>
    /// 为一个实例回调一个无参数的 post 注入方法
    /// </summary>
    public delegate void PostConstructor(object instance);

    /// <summary>
    /// 为一个实例回调一个有参数的 post 注入方法
    /// </summary>
    public delegate void ParamsPostConstructor(object instance, object[] parameters);

    /// <summary>
    /// 为一个属性或字段回调一个设置注入方法
    /// </summary>
    public delegate void Setter(object instance, object value);

    /// <summary>
    /// Setter 信息类
    /// </summary>
    public class SetterInfo : ParameterInfo
    {
        /// <summary>
        /// Setter 方法
        /// </summary>
        public Setter setter;

        #region constructor

        public SetterInfo(Type type, object id, Setter setter) : base(type, id)
        {
            this.setter = setter;
        }

        #endregion
    }

    /// <summary>
    /// 参数信息类
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// Setter 类型
        /// </summary>
        public Type type;

        /// <summary>
        /// id
        /// </summary>
        public object id;

        #region constructor

        public ParameterInfo(Type type, object id)
        {
            this.type = type;
            this.id = id;
        }

        #endregion
    }

    /// <summary>
    /// 注入完成后执行方法信息类
    /// </summary>
    public class PostConstructorInfo
    {
        /// <summary>
        /// 无参数方法
        /// </summary>
        public PostConstructor postConstructor;

        /// <summary>
        /// 带参数方法
        /// </summary>
        public ParamsPostConstructor paramsPostConstructor;

        /// <summary>
        /// 方法参数信息
        /// </summary>
        public ParameterInfo[] parameters;

        #region constructor

        public PostConstructorInfo(ParameterInfo[] parameters)
        {
            this.parameters = parameters;
        }

        #endregion
    }
}