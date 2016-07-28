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
    public abstract class ContextRoot : MonoBehaviour//, IContextRoot
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
            //this.gameObject.AddComponent<SceneInjector>();
            SceneInjector();

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

        #region sceneInjector

        /// <summary>
        /// 
        /// </summary>
        private void SceneInjector()
        {
            // 获取当前物体上的 ContextRoot组件
            // var contextRoot = this.GetComponent<ContextRoot>();

            // 设置基类类型：contextRoot.baseBehaviourTypeName 等于 "UnityEngine.MonoBehaviour"
            // 时就赋值为 typeof(MonoBehaviour)；否则就根据 baseBehaviourTypeName 获取类型来赋值
            var baseType = (baseBehaviourTypeName == "UnityEngine.MonoBehaviour" ?
                typeof(MonoBehaviour) : TypeUtils.GetType(baseBehaviourTypeName));

            switch (injectionType)
            {
                // injectionType 为 Children 对当前 Transform 的所有子节点所挂载的所有组件进行注入
                case ContextRoot.MonoBehaviourInjectionType.Children:
                    InjectOnChildren(baseType);
                    break;

                // 如果 injectionType 为 BaseType 获取所有MONO文件实例并注入指定类型
                case ContextRoot.MonoBehaviourInjectionType.BaseType:
                    InjectFromBaseType(baseType);
                    break;
            }
        }

        /// <summary>
        /// 对当前 Transform 的所有子节点所挂载的所有组件进行注入
        /// </summary>
        public void InjectOnChildren(Type baseType)
        {
            // 获取自身的类型
            var sceneInjectorType = GetType();
            // 获取 Transform 所有子节点中的所有指定类型组件 （数组）
            var components = GetComponent<Transform>().GetComponentsInChildren(baseType, true);

            foreach (var component in components)
            {
                // 如果当前组件是 ContextRoot 或是自身，忽略注入
                var componentType = component.GetType();
                if (componentType == sceneInjectorType ||
                    TypeUtils.IsAssignable(typeof(ContextRoot), componentType)) { continue; }

                ((MonoBehaviour)component).Inject();
            }
        }

        /// <summary>
        /// Performs injection on all behaviours of a given <paramref name="baseType"/>.
        /// 获取所有MONO文件实例并注入指定类型
        /// </summary>
        /// <param name="baseType">Base type to perform injection.</param>
        public void InjectFromBaseType(Type baseType)
        {
            // 获取所有 MonoBehaviour 类型文件
            var components = (MonoBehaviour[])Resources.FindObjectsOfTypeAll(baseType);
            // 循环注入
            for (var index = 0; index < components.Length; index++)
            {
                components[index].Inject();
            }
        }

        #endregion
    }
}