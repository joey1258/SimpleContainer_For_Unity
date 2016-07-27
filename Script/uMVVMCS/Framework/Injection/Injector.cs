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
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public class Injector : IInjector
    {
        #region Injector AOT Event

        public event TypeResolutionHandler beforeResolve;
        public event TypeResolutionHandler afterResolve;
        public event BindingEvaluationHandler bindingEvaluation;
        public event BindingResolutionHandler bindingResolution;
        public event InstanceInjectionHandler beforeInject;
        public event InstanceInjectionHandler afterInject;

        #endregion

        /// <summary>
        /// 类型反射信息缓存
        /// </summary>
        public IReflectionCache cache { get; protected set; }

        /// <summary>
        /// Binder used to resolved bindings.
        /// </summary>
        public IBinder binder { get; protected set; }

        /// <summary>
        /// binding 实例化模式
        /// </summary>
        public ResolutionMode resolutionMode { get; set; }

        #region constructor

        public Injector(
            IReflectionCache cache, 
            IBinder binder, 
            ResolutionMode resolutionMode)
        {
            this.cache = cache;
            this.binder = binder;
            this.resolutionMode = resolutionMode;

            binder.beforeAddBinding += this.OnBeforeAddBinding;
        }

        #endregion

        #region IInjector implementation 

        #region Resolve

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        virtual public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T), InjectionInto.None, null, null);
        }

        /// <summary>
        /// 为指定类型和 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        public T Resolve<T>(object identifier)
        {
            return (T)this.Resolve(typeof(T), InjectionInto.None, null, identifier);
        }

        /// <summary>
        /// 为指定类型和 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        public object Resolve(Type type, object identifier)
        {
            return this.Resolve(type, InjectionInto.None, null, identifier);
        }

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        virtual public object Resolve(Type type)
        {
            return this.Resolve(type, InjectionInto.None, null, null);
        }

        /// <summary>
        /// 为指定 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        public object Resolve(object identifier)
        {
            var instances = (object[])this.Resolve(null, InjectionInto.None, null, identifier);

            if (instances != null && instances.Length > 0) { return instances[0]; }
            else { return instances; }
        }

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        virtual public T[] ResolveAll<T>()
        {
            var instance = this.Resolve(typeof(T));

            if (instance == null) { return null; }
            else if (!instance.GetType().IsArray)
            {
                // 创建一个实例类型的，长度为1的数组
                var array = Array.CreateInstance(instance.GetType(), 1);
                array.SetValue(instance, 0);
                return (T[])array;
            }
            else { return (T[])instance; }
        }

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        virtual public object[] ResolveAll(Type type)
        {
            var instance = Resolve(type);

            if (instance == null)
            {
                return null;
            }
            else if (!instance.GetType().IsArray)
            {
                var array = Array.CreateInstance(instance.GetType(), 1);
                array.SetValue(instance, 0);
                return (object[])array;
            }
            else {
                return (object[])instance;
            }
        }

        /// <summary>
        /// 为指定类型和 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例数组
        /// </summary>
        public T[] ResolveSpecified<T>(object identifier)
        {
            var instance = Resolve(typeof(T), identifier);

            if (instance == null) { return null; }
            else if (!instance.GetType().IsArray)
            {
                var array = Array.CreateInstance(instance.GetType(), 1);
                array.SetValue(instance, 0);
                return (T[])array;
            }
            else { return (T[])instance; }
        }

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// </summary>
        virtual protected object Resolve(
            Type type,
            InjectionInto member,
            object parentInstance,
            object id)
        {
            object resolution = null;

            // 如果 AOT Resolve 前置委托不为空就执行
            if (this.beforeResolve != null)
            {
                var delegates = this.beforeResolve.GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    var continueExecution = ((TypeResolutionHandler)delegates[i]).Invoke(
                        this,
                        type,
                        member,
                        parentInstance,
                        id,
                        ref resolution);

                    if (!continueExecution) { return resolution; }
                }
            }

            Type inwardType = typeof(object);
            IList<IBinding> bindings = new List<IBinding>();

            // 如果 id 为空将导致 ResolveBinding 方法返回 null，所以就算此处取了 binding 也只是无用功
            if (id != null)
            {
                // 如果类型为空 id 不为空，根据 id 来获取 binding
                if (type == null) { bindings = this.binder.GetBindingsById(id); }
                else
                {
                    // 判断参数 type 是否为数组是因为实参可能会传入类似 typeof(Type[]) 这样的值
                    if (type.IsArray) { type.GetElementType(); }

                    inwardType = type;
                    bindings.Add(this.binder.GetBinding(inwardType, id));
                }
            }

            IList<object> instances = new List<object>();

            // 如果没有获取到 binding，且 ResolutionMode 是 ALWAYS_RESOLVE，就调用 Instantiate 方法
            // 返回参数 type 的执行结果并添加到 instances 中,否则返回空
            if (bindings.Count == 0)
            {
                if (this.resolutionMode == ResolutionMode.ALWAYS_RESOLVE)
                {
                    instances.Add(this.Instantiate(type as Type));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                // 循环调用 ResolveBinding 方法新建实例，并且将新建成功的实例加入到 instances 中去
                for (int i = 0; i < bindings.Count; i++)
                {
                    var instance = this.ResolveBinding(
                        bindings[i],
                        type,
                        member,
                        parentInstance,
                        id);

                    if (instance is Array)
                    {
                        object[] os = (object[])instance;
                        int length = os.Length;

                        for (int n = 0; n < length; n++)
                        {
                            instances.Add(os[n]);
                        }
                    }
                    else { instances.Add(instance); }
                }
            }

            // 如果 type 不为空且不是数组、instances的长度为1，将其第0个元素赋值给 resolution
            if (type != null && !type.IsArray && instances.Count == 1)
            {
                resolution = instances[0];
            }
            // 否则就以数组形式赋值给 resolution(将数组存为 object，使用时需要转回数组)
            else if (instances.Count > 0)
            {
                var array = Array.CreateInstance(inwardType, instances.Count);
                for (int i = 0; i < instances.Count; i++)
                {
                    array.SetValue(instances[i], i);
                }
                resolution = array;
            }

            // 如果 AOT Resolve 后置委托不为空就执行
            if (this.afterResolve != null)
            {
                var delegates = this.afterResolve.GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    var continueExecution = ((TypeResolutionHandler)delegates[i]).Invoke(this,
                        type,
                        member,
                        parentInstance,
                        id,
                        ref resolution);

                    if (!continueExecution)
                    {
                        return resolution;
                    }
                }
            }

            // 返回实例
            return resolution;
        }

        #endregion

        #region Inject

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        public T Inject<T>(T instance) where T : class
        {
            var reflectedInfo = this.cache.GetInfo(instance.GetType());
            return (T)this.Inject(instance, reflectedInfo);
        }

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        public object Inject(object instance)
        {
            var reflectedInfo = this.cache.GetInfo(instance.GetType());
            return this.Inject(instance, reflectedInfo);
        }

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        protected object Inject(object instance, ReflectionInfo reflectedInfo)
        {
            // 如果 AOT Inject 前置委托不为空就执行
            if (this.beforeInject != null)
            {
                this.beforeInject(this, ref instance, reflectedInfo);
            }

            // 如果有需要注入的字段，为其执行字段注入
            if (reflectedInfo.fields.Length > 0)
            {
                this.InjectFields(instance, reflectedInfo.fields);
            }

            // 如果有需要注入的属性，为其执行字段注入
            if (reflectedInfo.properties.Length > 0)
            {
                this.InjectProperties(instance, reflectedInfo.properties);
            }

            // 如果有需要在注入后执行的方法，为其执行该方法
            if (reflectedInfo.methods.Length > 0)
            {
                this.InjectMethods(instance, reflectedInfo.methods);
            }

            // 如果 AOT Inject 后置委托不为空就执行
            if (this.afterInject != null)
            {
                this.afterInject(this, ref instance, reflectedInfo);
            }

            return instance;
        }

        #endregion

        #endregion

        #region Resolve assist

        /// <summary>
        /// 对参数 binding 进行过滤，根据 BindingType 进行实例化和注入操作，并返回其结果(有可能是数组）
        /// </summary>
        virtual protected object ResolveBinding(
            IBinding binding,
            Type type,
            InjectionInto member,
            object parentInstance,
            object id)
        {
            var context = new InjectionContext()
            {
                member = member,
                memberType = type,
                id = id,
                parentType = (parentInstance == null ? null : parentInstance.GetType()),
                parentInstance = parentInstance,
                injectType = binding.type
            };

            // 如果参数binding的条件(BindingCondition委托，接受一个InjectionContext参数)不为空，新建一个InjectionContext实例并设置它的属性
            if (binding.condition != null)
            {
                // 如果参数binding的condition委托的结果是假则返回空
                if (!binding.condition(context)) { return null; }
            }

            // 过滤 id 条件（id 和 binding.id 都不能为空且必须相等），不符合返回空
            if (id == null || binding.id == null) { return null; }
            if (!binding.id.Equals(id)) { return null; }

            // 过滤实例
            object instance = null;

            // 如果 AOT 委托 BindingEvaluationHandler 不为空就执行
            if (this.bindingEvaluation != null)
            {
                var delegates = this.bindingEvaluation.GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    instance = ((BindingEvaluationHandler)delegates[i]).Invoke(this, ref binding);
                }
            }

            if (instance == null)
            {
                int length = binding.valueList.Count;

                switch (binding.bindingType)
                {
                    // 如果实例类型为 TEMP，获取对应的完成注入后的实例
                    // 不保存实例化结果到 binding.value
                    case BindingType.TEMP:
                        if(binding.constraint == ConstraintType.MULTIPLE)
                        {
                            object[] list = new object[length];
                            for (int i = 0; i < length; i++)
                            {
                                list[i] = Instantiate(binding.valueList[i] as Type);
                            }
                            instance = list;
                        }
                        else { instance = Instantiate(binding.value as Type); }
                        break;

                    // 如果是工厂类型，将 binding 的值作为工厂类并获取其create方法的结果
                    // 可以重写对应的接口类方法来实现所需的生成效果
                    case BindingType.FACTORY:
                        instance = (binding.value as IInjectionFactory).Create(context);
                        break;

                    // 如果是单例类型，且 binding 的值是 Type，就实例化该类型并执行注入
                    // 同时保存实例化的结果到 binding.value；如果不是 Type 就直接获取其值
                    case BindingType.SINGLETON:
                        if (binding.value is Type)
                        {
                            binding.To(Instantiate(binding.value as Type));
                        }

                        instance = binding.value;
                        break;

                    // 如果是多例类型，遍历它的所有值,如果值是 Type，就实例化该类型并执行注入
                    // 同时保存实例化的结果到当前元素；如果不是 Type 就直接获取其值
                    case BindingType.MULTITON:
                        object[] instances = new object[length];
                        for (int i = 0; i < length; i++)
                        {
                            if (binding.valueList[i] is Type)
                            {
                                binding.valueList[i] = Instantiate(binding.value as Type);
                            }
                            instances[i] = binding.valueList[i];
                        }

                        instance = instances;
                        break;
                }
            }

            // 如果 AOT 委托 bindingResolution 不为空执行委托
            if (this.bindingResolution != null)
            {
                this.bindingResolution(this, ref binding, ref instance);
            }

            // 返回实例
            return instance;
        }

        /// <summary>
        /// 实例化指定类型并对新实例执行注入，最后返回其结果
        /// </summary>
        virtual protected object Instantiate(Type type)
        {
            var info = this.cache.GetInfo(type);
            object instance = null;

            // 如果所缓存的无参数构造函数和有参数构造函数信息都为空，抛出异常
            if (info.constructor == null && info.paramsConstructor == null)
            {
                throw new InjectionSystemException(
                    string.Format(InjectionSystemException.NO_CONSTRUCTORS,
                    type.ToString()));
            }

            // 如果没有构造函数参数信息，直接用无参数构造函数生成实例
            if (info.constructorParameters.Length == 0)
            {
                instance = info.constructor();
            }
            else
            {
                // 根据缓存的参数类型生成所有所需参数的实例
                object[] parameters = GetParametersFromInfo(
                    null,
                    info.constructorParameters);
                instance = info.paramsConstructor(parameters);
            }

            // 执行注入
            instance = Inject(instance, info);

            return instance;
        }

        /// <summary>
        /// 根据缓存的构造函数参数属性 constructorParameters 实例化并返回所有所需参数 
        /// instance 参数最终会传递到 ResolveBinding 方法的 parentInstance 参数，用于传递 InjectionContext 同名属性的值
        /// </summary>
        virtual protected object[] GetParametersFromInfo(object instance, ParameterInfo[] parametersInfo)
        {
            object[] parameters = new object[parametersInfo.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parametersInfo[i];

                parameters[i] = Resolve(
                    parameterInfo.type,
                    InjectionInto.Constructor,
                    instance,
                    parameterInfo.id);
            }

            return parameters;
        }

        #endregion

        #region Inject assist

        /// <summary>
        /// 字段注入
        /// </summary>
        virtual protected void InjectFields(object instance, SetterInfo[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var valueToSet = this.Resolve(
                    field.type,
                    InjectionInto.Field,
                    instance,
                    field.id);

                field.setter(instance, valueToSet);
            }
        }

        /// <summary>
        /// 属性注入
        /// </summary>
        virtual protected void InjectProperties(object instance, SetterInfo[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var valueToSet = this.Resolve(
                    property.type,
                    InjectionInto.Property,
                    instance,
                    property.id);

                property.setter(instance, valueToSet);
            }
        }

        /// <summary>
        /// 注入到方法
        /// </summary>
        virtual protected void InjectMethods(
            object instance,
            MethodInfo[] methods)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];

                if (method.parameters.Length == 0)
                {
                    method.method(instance);
                }
                else
                {
                    object[] parameters = this.GetParametersFromInfo(
                        instance, 
                        method.parameters);
                    method.paramsMethod(instance, parameters);
                }
            }
        }

        #endregion

        /// <summary>
        /// AddBinding 之前执行的 AOT 方法
        /// </summary>
        virtual protected void OnBeforeAddBinding(IBinder source, ref IBinding binding)
        {
            if (binding.bindingType != BindingType.TEMP)
            {
                // 由于 AOT 委托在 Storing 方法过滤空 binding 之后才执行，所以这里就不重复检查了
                int length = binding.valueList.Count;
                for (int i = 0; i < length; i++)
                {
                    if (binding.valueList[i] is Type)
                    {
                        var value = this.Resolve(binding.valueList[i] as Type);
                        binding.To(value);
                    }
                    else
                    {
                        // hasBeenInjected 判断放在容器类中更为合适，所以这里不做注入状态检查
                        Inject(binding.valueList[i]);
                    }
                }
            }
        }
    }
}