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

using UnityEngine;

namespace uMVVMCS.DIContainer
{
    [AddComponentMenu("uMVVMCS/Timed command dispatch")]
    public class TimedCommandDispatch : CommandDispatch
    {
        /// <summary>
        /// 持续秒数
        /// </summary>
        public float timer;

        /// <summary>
        /// 当脚本激活时带调用
        /// </summary>
        protected void OnEnable()
        {
            Invoke("DispatchCommand", timer);
        }
    }
}