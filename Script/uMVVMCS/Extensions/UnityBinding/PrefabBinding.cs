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
    public class PrefabBinding
    {
        /// <summary>
        /// 需要实例化的 prefab
        /// </summary>
        public Object prefab { get; private set; }

        /// <summary>
        /// prefab 上的组件
        /// </summary>
        public System.Type type { get; set; }

        #region constructor

        public PrefabBinding(Object prefab, System.Type type)
        {
            this.prefab = prefab;
            this.type = type;
        }

        #endregion
    }
}