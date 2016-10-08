using UnityEngine;
using System;
using Utils;

namespace ToluaContainer.Container
{
    [RequireComponent(typeof(ContextRoot))]
    public class SceneInjector : MonoBehaviour
    {
        private void Awake()
        {
            var contextRoot = GetComponent<ContextRoot>();
            var baseType = (contextRoot.baseBehaviourTypeName == "UnityEngine.MonoBehaviour" ?
                typeof(MonoBehaviour) : TypeUtils.GetType(contextRoot.baseBehaviourTypeName));

            switch (contextRoot.injectionType)
            {
                case ContextRoot.MonoBehaviourInjectionType.Children:
                    {
                        InjectOnChildren(baseType);
                    }
                    break;

                case ContextRoot.MonoBehaviourInjectionType.BaseType:
                    {
                        InjectFromBaseType(baseType);
                    }
                    break;
            }
        }

        /// <summary>
        /// 对当前物体的所有子物体注入
        /// </summary>
        public void InjectOnChildren(Type baseType)
        {
            var sceneInjectorType = GetType();
            var components = GetComponent<Transform>().GetComponentsInChildren(baseType, true);
            foreach (var component in components)
            {
                // 如果组件是 ContextRoot 或者是自身则忽略
                var componentType = component.GetType();
                if (componentType == sceneInjectorType ||
                    TypeUtils.IsAssignable(typeof(ContextRoot), componentType)) continue;

                ((MonoBehaviour)component).Inject();
            }
        }

        /// <summary>
        /// 为所有 MonoBehaviour 注入指定类型
        /// </summary>
        public void InjectFromBaseType(Type baseType)
        {
            var components = (MonoBehaviour[])Resources.FindObjectsOfTypeAll(baseType);

            for (var index = 0; index < components.Length; index++)
            {
                components[index].Inject();
            }
        }
    }
}