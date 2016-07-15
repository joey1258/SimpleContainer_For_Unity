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
    public interface IInfo
    {
        /// <summary>
        /// type 属性
        /// </summary>
        Type type { get; }

        /// <summary>
        ///  value 属性
        /// </summary>
        object value { get; set; }

        /// <summary>
        /// id 属性
        /// </summary>
        object id { get; set; }

        /// <summary>
        /// 注入类型
        /// </summary>
        BindingType bindingType { get; }

        /// <summary>
        /// value 约束 (SINGLE \ MULTIPLE \ POOL)
        /// </summary>
        ConstraintType valueConstraint { get; }

        /// <summary>
        /// 是否已经被注入
        /// </summary>
        bool hasBeenInjected { get; set; }

        /// <summary>
        /// 条件判断委托
        /// </summary>
        Condition condition { get; set; }
    }
}