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

using UnityEditor;
using System;

namespace uMVVMCS.Editors
{
    public static class ExecutionOrderUtils
    {
        /// <summary>
        /// 设置执行顺序
        /// </summary>
        public static int SetScriptExecutionOrder(Type type, int order)
        {
            return SetScriptExecutionOrder(type, order, true);
        }

        /// <summary>
        /// 设置执行顺序
        /// </summary>
        public static int SetScriptExecutionOrder(Type type, int order, bool unique)
        {
            var executionOrder = order;
            MonoScript selectedScript = null;

            // 按顺序执行
            var available = false;
            while (!available)
            {
                available = true;

                var scripts = MonoImporter.GetAllRuntimeMonoScripts();
                int length = scripts.Length;
                for (int i = 0; i < length; i++)
                {
                    if (selectedScript == null && scripts[i].GetClass() == type)
                    {
                        selectedScript = scripts[i];
                        if (!unique) { break; }
                    }

                    if (scripts[i].GetClass() != type && MonoImporter.GetExecutionOrder(scripts[i]) == executionOrder)
                    {
                        executionOrder += order;
                        available = false;
                        continue;
                    }
                }
            }

            MonoImporter.SetExecutionOrder(selectedScript, executionOrder);

            return executionOrder;
        }
    }
}