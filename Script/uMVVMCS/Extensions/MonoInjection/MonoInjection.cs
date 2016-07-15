/*
 * Copyright 2016 Sun Ning
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

using UnityEngine;

namespace uMVVMCS.DIContainer.Extensions
{
    /// <summary>
    /// 为 MonoBehaviour 提供注入方法，默认 ContextRoot Extension 可以使用
    /// </summary>
    public static class MonoInjection
    {
        /// <summary>
        /// 为 MonoBehaviour 执行注入,参数 script 为注入目标
        /// </summary>
        public static void Inject(this MonoBehaviour script)
        {
            InjectionUtil.Inject(script);
        }

    }
}