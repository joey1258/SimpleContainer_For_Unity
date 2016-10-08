using UnityEditor;
using System;

namespace ToluaContainer.Editors
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