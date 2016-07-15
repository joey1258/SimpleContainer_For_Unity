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
    public class InjectionContext
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