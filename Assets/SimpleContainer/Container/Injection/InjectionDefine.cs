using System;

namespace SimpleContainer.Container
{
    /// <summary>
    /// Resolve 方法的 AOT 委托，在 Resolve 方法实际操作开始之前和完成之后根据类型参数进行前置/后置操作
    /// （修改传入的委托参数 resolutionInstance ）（Resolution : 正式决定，决议）
    /// </summary>
    public delegate bool TypeResolutionHandler(IInjector source, Type type, InjectionInto member, object parentInstance, object id, ref object resolutionInstance);

    /// <summary>
    /// ResolveBinding 方法 AOT 委托，在方法内部对 id 进行完过滤之后，根据 binding 的 bindingType
    /// 对其进行相应的实例化与注入操作的之前进行的前置操作（修改传入的委托参数 binding）
    /// </summary>
    public delegate object BindingEvaluationHandler(IInjector source, ref IBinding binding);

    /// <summary>
    /// ResolveBinding 方法相关委托，在方法内部根据 binding 的 bindingType
    /// 对其进行相应的实例化与注入操作之后进行后置操作（修改传入的委托参数 instance）
    /// </summary>
    public delegate void BindingResolutionHandler(IInjector source, ref IBinding binding, ref object instance);

    /// <summary>
    /// Inject 方法相关委托，在 Inject 方法实际操作开始之前和完成之后根据类型参数进行前置/后置操作
    /// （修改传入的委托参数 instance）
    /// </summary>
    public delegate void InstanceInjectionHandler(IInjector source, ref object instance, ReflectionInfo reflectInfo);

    /// <summary>
    /// 注入方式
    /// </summary>
	public enum InjectionInto
    {
        None,
        Constructor,
        Method,
        Field,
        Property
    }

    /// <summary>
    /// binding 实例化模式
    /// </summary>
    public enum ResolutionMode
    {
        /// <summary>
        /// 不论有没有绑定到容器，尝试对所有的类型执行 resolve（默认模式）
        /// </summary>
        ALWAYS_RESOLVE,
        /// <summary>
        /// 只对绑定到容器的类型进行 resolves，尝试对没有绑定的类型执行 resolves 将返回空
        /// </summary>
        BOUND_ONLY
    }
}