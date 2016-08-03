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

namespace uMVVMCS
{
    /// <summary>
    /// Generic C# singleton.
    /// </summary>
    public abstract class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// 单例 instance.
        /// </summary>
        protected static T _Instance = null;

        /// <summary>
        /// 获取 instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 初始化 <see cref="Singleton"/> 类的新实例.
        /// </summary>
        protected Singleton()
        {
            if (_Instance != null)
            {
                throw new UtilsSystemException(
                    string.Format(
                        UtilsSystemException.SingletonInstanceNotNUll,
                        (typeof(T)).ToString()));
            }
            // 初始化单例
            Init();
        }

        /// <summary></summary>
        /// 初始化单例
        /// </summary>
        public virtual void Init() { }
    }
}
