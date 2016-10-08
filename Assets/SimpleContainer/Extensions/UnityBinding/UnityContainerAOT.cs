using System;
using UnityEngine;
using Utils;

namespace ToluaContainer.Container
{
    public class UnityContainerAOT : IContainerAOT
    {
        #region IContainerExtension implementation 

        public void OnRegister(IInjectionContainer container)
        {
            container.beforeAddBinding += OnBeforeAddBinding;
            container.beforeDefaultInstantiate += this.OnBindingEvaluation;
        }

        public void OnUnregister(IInjectionContainer container)
        {
            container.beforeAddBinding -= this.OnBeforeAddBinding;
            container.beforeDefaultInstantiate -= this.OnBindingEvaluation;
        }

        #endregion

        /// <summary>
        /// 如果当前 binding 的值是类型且是 MonoBehaviour 派生类就抛出异常
        /// </summary>
        protected void OnBeforeAddBinding(IBinder source, ref IBinding binding)
        {
            if (binding.value is Type &&
                TypeUtils.IsAssignable(typeof(MonoBehaviour), binding.value as Type))
            {
                throw new Exceptions(Exceptions.CANNOT_RESOLVE_MONOBEHAVIOUR);
            }
        }

        /// <summary>
        /// 为 ADDRESS 类型 binding 返回实例化并加载好组件的 gameObject(在 Injector 类的 
        /// ResolveBinding 方法中触发)
        /// </summary>
        protected object OnBindingEvaluation(
            IInjector source, 
            ref IBinding binding)
        {
            if (binding.value is PrefabInfo &&
                binding.bindingType == BindingType.ADDRESS)
            {
                var prefabInfo = (PrefabInfo)binding.value;
                var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);

                if (prefabInfo.type.Equals(typeof(GameObject)))
                {
                    return gameObject;
                }
                else
                {
                    var component = gameObject.GetComponent(prefabInfo.type);

                    if (component == null)
                    {
                        component = gameObject.AddComponent(prefabInfo.type);
                    }

                    return component;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
