/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

namespace uMVVMCS.DIContainer
{
    /// <summary>
    /// 为 UnityEngine.StateMachineBehaviour 提供注入方法，以有可用 ContextRoot Extension 为前提
    /// </summary>
    public static class StateInjectionExtension
    {
        /// <summary>
        /// 注入方法
        /// </summary>
        public static void Inject(this StateMachineBehaviour script)
        {
            InjectionUtil.Inject(script);
        }
    }
}