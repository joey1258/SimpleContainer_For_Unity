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
using System;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public abstract class ContextRoot : MonoBehaviour, IContextRoot
    {
        #region Inner Class

        /// <summary>
        /// 注入容器数据内部类
        /// </summary>
        public class InjectionContainerData
        {
            /// <summary>
            /// 注入容器
            /// </summary>
            public IInjectionContainer container;

            /// <summary>
            /// load 时是否摧毁容器
            /// </summary>
            public bool destroyOnLoad;

            #region constructor

            public InjectionContainerData(IInjectionContainer container, bool destroyOnLoad)
            {
                this.container = container;
                this.destroyOnLoad = destroyOnLoad;
            }

            #endregion
        }

        /// <summary>
        /// MonoBehaviour injection type.
        /// </summary>
        [Serializable]
        public enum MonoBehaviourInjectionType
        {
            // 手动
            Manual,
            // 子类
            Children,
            // 基类
            BaseType
        }

        #endregion

        #region property

        /// <summary>
        /// Tooltip 特性将会使鼠标悬停时浮现提示文字
        /// MonoBehaviour 注入类型
        /// </summary>
        [Tooltip("Type of injection on MonoBehaviours."), HideInInspector]
        public MonoBehaviourInjectionType injectionType;

        /// <summary>
        /// 在场景中进入注入的 behaviour 基类名称
        /// </summary>
        [Tooltip("Name of the base behaviour type to perform scene injection."), HideInInspector]
        public string baseBehaviourTypeName;

        /// <summary>
        /// 内部容器数据类 list
        /// </summary>
        public static List<InjectionContainerData> containersData { get; set; }

        /// <summary>
        /// 容器数组 (返回 containersData List 中每个元素的 container 属性)(包括所有容器)
        /// </summary>
        public IInjectionContainer[] containers
        {
            get
            {
                int length = containersData.Count;

                if (_containers == null ||
                    _containers.Length == 0 ||
                    _containers.Length != length)
                {
                    _containers = new IInjectionContainer[length];
                    for (var i = 0; i < containersData.Count; i++)
                    {
                        _containers[i] = containersData[i].container;
                    }

                    ContainersStoring();
                }

                return _containers;
            }
        }
        protected IInjectionContainer[] _containers;

        /// <summary>
        /// 容器仓库 (储存 containersData List 中 id 不为空的binding，并用 type 和 id 索引)
        /// </summary>
        public static Storage<IInjectionContainer> containersStorage { get; protected set; }

        #endregion

        #region functions

        virtual protected void Awake()
        {
            containersStorage = new Storage<IInjectionContainer>();
            // 如果容器数据list为空，设置它的长度为1
            if (containersData == null)
            {
                containersData = new List<InjectionContainerData>(1);
            }

            SetupContainers();

            // 缓存所有容器中的 bindings 的类型信息
            CacheBindings();
        }

        virtual protected void Start()
        {
            // SceneInjector 应该比其它任何 Start 方法都早执行
            gameObject.AddComponent<SceneInjector>();

            Init();
        }

        virtual protected void OnDestroy()
        {
            // 释放 containersData List 中所有 destroyOnLoad 属性为真的容器中的 binder 和缓存
            // 并将其从 containersData List 中移除
            for (var i = 0; i < containersData.Count; i++)
            {
                if (!containersData[i].destroyOnLoad) continue;

                containersData[i].container.Dispose();
                containersData.Remove(containersData[i]);
                i--;
            }
        }

        /// <summary>
        /// 用新创建的容器和 true 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        virtual public IInjectionContainer AddContainer<T>() where T : IInjectionContainer, new()
        {
            var container = Activator.CreateInstance<T>();
            return AddContainer(container, true);
        }

        /// <summary>
        /// 用 container 和 true 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        virtual public IInjectionContainer AddContainer(IInjectionContainer container)
        {
            return AddContainer(container, true);
        }

        /// <summary>
        /// 用 container 和 destroyOnLoad 创建一个 InjectionContainerData，并添加到 containersData List
        /// </summary>
        virtual public IInjectionContainer AddContainer(IInjectionContainer container, bool destroyOnLoad)
        {
            containersData.Add(new InjectionContainerData(container, destroyOnLoad));

            return container;
        }

        /// <summary>
        /// 设置容器
        /// </summary>
        public abstract void SetupContainers();

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 缓存所有容器中所有 binding 的 value 属性所储存的类型信息
        /// </summary>
        private void CacheBindings()
        {
            for (var i = 0; i < containersData.Count; i++)
            {
                var container = containersData[i].container;
                container.cache.CacheFromBinder(container);
            }
        }

        /// <summary>
        /// 整理储存所有容器
        /// </summary>
        virtual protected void ContainersStoring()
        {
            for (var i = 0; i < containersData.Count; i++)
            {
                _containers[i] = containersData[i].container;

                if (_containers[i].id != null)
                {
                    if (!containersStorage[this.GetType()].Contains(_containers[i].id))
                    {
                        containersStorage[this.GetType()][_containers[i].id] = _containers[i];
                    }
                    else
                    {
                        throw new InjectionSystemException(InjectionSystemException.SAME_OBJECT);
                    }
                }
            }
        }

        #endregion
    }
}