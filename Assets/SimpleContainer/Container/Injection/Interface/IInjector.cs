using System;

namespace SimpleContainer.Container
{
    public interface IInjector
    {
        /// <summary>
        /// binding 实例化模式
        /// </summary>
        ResolutionMode resolutionMode { get; set; }

        #region Injector AOT Event

        event TypeResolutionHandler beforeResolve;
        event TypeResolutionHandler afterResolve;
        event BindingEvaluationHandler beforeDefaultInstantiate;
        event BindingResolutionHandler afterInstantiate;
        event InstanceInjectionHandler beforeInject;
        event InstanceInjectionHandler afterInject;

        #endregion

        #region Resolve

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        T Resolve<T>();

        /// <summary>
        /// 为指定类型、id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        T Resolve<T>(object identifier);

        /// <summary>
        /// 为指定类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        T[] ResolveAll<T>();

        /// <summary>
        /// 为指定的多个类型的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例
        /// 返回结果可能是 object 或者数组，使用时应加以判断
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        object[] ResolveAll(Type type);

        /// <summary>
        /// 为指定类型和 id 的所有 binding 执行相应的实例化和注入操作，并返回所有新生成的实例数组
        /// 无法在主线程之外正常运行，其他线程请使用 GetBinding 获取 Binding
        /// </summary>
        T[] ResolveSpecified<T>(object identifier);

        #endregion

        #region Inject

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        T Inject<T>(T instance) where T : class;

        /// <summary>
        /// 为实例注入依赖
        /// </summary>
        object Inject(object instance);

        #endregion
    }
}