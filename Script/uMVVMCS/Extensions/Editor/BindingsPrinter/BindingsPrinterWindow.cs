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

using UnityEngine;
using UnityEditor;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Editors
{
    public class BindingsPrinterWindow : EditorWindow
    {
        /// <summary>
        /// 窗口边缘值
        /// </summary>
        private const float WINDOW_MARGIN = 10.0f;

        /// <summary>
        /// 当前编辑器窗口
        /// </summary>
        private static BindingsPrinterWindow window;

        /// <summary>
        /// 当前卷轴位置
        /// </summary>
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/uMVVMCS/Bindings Printer")]
        protected static void Init()
        {
            // 获取当前屏幕中 SceneView 类型的第一个 EditorWindow，标题为 Bindings Printer
            window = GetWindow<BindingsPrinterWindow>("Bindings Printer", typeof(SceneView));
        }

        protected void OnGUI()
        {
            // 如果 Init() 没有获取到 EditorWindow 就以无参数的形式再获取一次
            if (!window)
            {
                window = GetWindow<BindingsPrinterWindow>();
            }

            // 要求窗口必须在运行状态下打开
            if (!Application.isPlaying)
            {
                // 插入一个自适应的空间
                GUILayout.FlexibleSpace();
                GUILayout.Label("Please execute the bindings printer on Play Mode", EditorStyles.message);
                // 插入一个自适应的空间
                GUILayout.FlexibleSpace();
                return;
            }

            // 如果 ContextRoot 的 containersData 不能为空
            if (ContextRoot.containersData == null || ContextRoot.containersData.Count == 0)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("There are no containers in the current scene", EditorStyles.message);
                GUILayout.FlexibleSpace();
                return;
            }

            // 添加窗口组件
            GUILayout.BeginHorizontal();
            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.BeginVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.Label("uMVVMCS Bindings Printer", EditorStyles.title);
            GUILayout.Label("Displays all bindings of all available containers", EditorStyles.containerInfo);

            // 显示容器及其中的 binding
            for (int i = 0; i < ContextRoot.containersData.Count; i++)
            {
                var data = ContextRoot.containersData[i];
                var bindings = data.container.GetAllBindings();

                GUILayout.Space(20f);
                GUILayout.Label("CONTAINER", EditorStyles.containerInfo);
                GUILayout.FlexibleSpace();
                GUILayout.Label(
                    string.Format(
                        "{0} (index: {1}, {2})",
                        data.container.GetType().FullName, i,
                        (data.destroyOnLoad ? "destroy on load" : "singleton")
                    ),
                    EditorStyles.title
                );

                GUILayout.FlexibleSpace();
                GUILayout.Space(10f);

                // 添加缩进
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.BeginVertical();

                for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
                {
                    var binding = bindings[bindingIndex];

                    GUILayout.Label(binding.ToString(), EditorStyles.bindings);
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}