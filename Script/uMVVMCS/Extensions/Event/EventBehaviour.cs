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
using uMVVMCS.DIContainer;

namespace uMVVMCS
{
    public class EventBehaviour : MonoBehaviour
    {
        protected void Update()
        {
            // 如果游戏暂停则不执行（Mathf.Approximately 方法用于比较浮点值，由于浮点值不精确所以运算时可能产生误差，所以必须使用特殊的方法来比较）
            if (Mathf.Approximately(Time.deltaTime, 0)) { return; }

            for (var objIndex = 0; objIndex < EventContainerAOT.updateable.Count; objIndex++)
            {
                EventContainerAOT.updateable[objIndex].Update();
            }
        }

        /// <summary>
        /// 当组件被销毁时调用，销毁 disposable list 中的所有元素
        /// </summary>
        protected void OnDestroy()
        {
            int length = EventContainerAOT.disposable.Count;
            for (int i = 0; i < length; i++)
            {
                EventContainerAOT.disposable[i].Dispose();
            }

            EventContainerAOT.disposable.Clear();
            EventContainerAOT.updateable.Clear();
        }
    }
}
