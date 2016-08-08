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
using UnityEngine;
using System;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public class EventContainer : IContainerExtension
    {
        /// <summary>
        /// 需要释放的对象 list
        /// </summary>
        public static List<IDisposable> disposable = new List<IDisposable>();

        /// <summary>
        /// 每帧更新的对象 list
        /// </summary>
        public static List<IUpdatable> updateable = new List<IUpdatable>();

        /// <summary>
        /// event
        /// </summary>
        public static EventBehaviour eventBehaviour;

        #region constructor

        public EventContainer()
        {
            //Creates a new game object for UpdateableBehaviour.
            var gameObject = new GameObject("EventCaller");
            eventBehaviour = gameObject.AddComponent<EventBehaviour>();
        }

        #endregion

        public void OnRegister(IInjectionContainer container)
        {
            //Adds the container to the disposable list.
            disposable.Add(container);

            //Checks whether a binding for the ICommandDispatcher exists.
            if (container.ContainsBindingFor<ICommandDispatcher>())
            {
                var dispatcher = container.Resolve<ICommandDispatcher>();
                if (dispatcher is IDisposable)
                {
                    disposable.Add((IDisposable)dispatcher);
                }
            }

            container.afterAddBinding += this.OnAfterAddBinding;
            container.bindingResolution += this.OnBindingResolution;
        }

        public void OnUnregister(IInjectionContainer container)
        {
            container.afterAddBinding -= this.OnAfterAddBinding;
            container.bindingResolution -= this.OnBindingResolution;

            disposable.Clear();
            updateable.Clear();
            MonoBehaviour.Destroy(eventBehaviour);
        }

        /// <summary>
        /// handles the after add binding event.
        /// 
        /// Used to check whether singleton instances should be added to the updater.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="binding">Binding.</param>
        protected void OnAfterAddBinding(IBinder source, ref BindingInfo binding)
        {
            if (binding.instanceType == BindingInstance.Singleton)
            {
                //Do not add commands.
                if (binding.value is ICommand) return;

                if (binding.value is IDisposable && !disposable.Contains((IDisposable)binding.value))
                {
                    disposable.Add((IDisposable)binding.value);
                }
                if (binding.value is IUpdatable && !updateable.Contains((IUpdatable)binding.value))
                {
                    updateable.Add((IUpdatable)binding.value);
                }
            }
        }

        /// <summary>
        /// Handles the binding resolution event.
        /// 
        /// Used to check whether the resolved instance should be added to the updater.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="binding">Binding.</param>
        /// <param name="instance">Instance.</param>
        protected void OnBindingResolution(IInjector source, ref BindingInfo binding, ref object instance)
        {
            //Do not add commands.
            if (binding.instanceType == BindingInstance.Singleton || instance is ICommand) return;

            if (instance is IDisposable && !disposable.Contains((IDisposable)instance))
            {
                disposable.Add((IDisposable)instance);
            }
            if (instance is IUpdatable && !updateable.Contains((IUpdatable)instance))
            {
                updateable.Add((IUpdatable)instance);
            }
        }
    }
}
