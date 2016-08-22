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

using System;

namespace uMVVMCS.DIContainer
{
    public static class BindingsSetup
    {
        #region Inner Class

        /// <summary>
        /// 设置 binding 的优先级
        /// </summary>
        private class PrioritizedSetup : IComparable<PrioritizedSetup>
        {
            public IBindingsSetup setup;
            public int priority;

            #region IComparable implementation 

            public int CompareTo(PrioritizedSetup other)
            {
                if (other == null) { return 1; }
                else { return -priority.CompareTo(other.priority); }
            }

            #endregion
        }

        #endregion

        #region functions

        /// <summary>
        /// 为指定的实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        public static IInjectionContainer SetupBindings<T>(this IInjectionContainer container) where T : IBindingsSetup, new()
        {
            container.SetupBindings(typeof(T));

            return container;
        }

        /// <summary>
        /// 为指定的实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, Type type)
        {
            var setup = container.Resolve(type);
            container.SetupBindings((IBindingsSetup)setup);

            return container;
        }

        /// <summary>
        /// 为指定的实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, IBindingsSetup setup)
        {
            setup.SetupBindings(container);

            return container;
        }

        /// <summary>
        /// 为指定命名空间（包括子空间）中实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先
        /// 级排序，最后按顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, string namespaceName)
        {
            container.SetupBindings(namespaceName, true);

            return container;
        }

        /// <summary>
        /// 为指定命名空间中实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container,
             string namespaceName,
             bool includeChildren)
        {
            // 获取指定命名空间中实现了 IBindingsSetup 接口的类型数组
            var setups = TypeUtils.GetAssignableTypes(
                typeof(IBindingsSetup), namespaceName, includeChildren);
            // 新建一个和获取到的类型数组同等长度的内部类数组
            var prioritizedSetups = new PrioritizedSetup[setups.Length];

            for (var i = 0; i < setups.Length; i++)
            {
                // 使用指定容器获取类型的经过注入后的类型实例
                var setup = (IBindingsSetup)container.Resolve(setups[i]);
                var attributes = setup.GetType().GetCustomAttributes(typeof(Priority), true);

                // 如果获取到了[Priority]特性，就将类型的实例和其优先级数字新建为一个新的内部类加入数组
                if (attributes.Length > 0)
                {
                    var bindingPriority = attributes[0] as Priority;
                    prioritizedSetups[i] = new PrioritizedSetup()
                    {
                        setup = setup,
                        priority = bindingPriority.priority
                    };
                }
                else
                {
                    // 如果没有获取到，就用实例和优先级数字0来新建一个内部类加入数组
                    prioritizedSetups[i] = new PrioritizedSetup()
                    {
                        setup = setup,
                        priority = 0
                    };
                }
            }

            // 对数组进行排序
            Array.Sort(prioritizedSetups);
            // 逐一执行 setup 对象所实现的 SetupBindings 方法
            for (var setupIndex = 0; setupIndex < prioritizedSetups.Length; setupIndex++)
            {
                prioritizedSetups[setupIndex].setup.SetupBindings(container);
            }

            return container;
        }

        #endregion
    }
}