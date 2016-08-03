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

namespace uMVVMCS
{
    /// <summary>
    /// Mono singleton.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T _Instance = null;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject go = GameObject.Find("Root");
                    if (go == null)
                    {
                        go = new GameObject("Root");
                        DontDestroyOnLoad(go);
                    }
                    _Instance = go.AddComponent<T>();

                }
                return _Instance;
            }
        }

        /// <summary>
        /// 程序退出事件
        /// </summary>
        private void OnApplicationQuit()
        {
            _Instance = null;
        }
    }
}
