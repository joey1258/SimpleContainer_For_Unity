namespace ToluaContainer.Container
{
    public interface IBindingsSetup
    {
        /// <summary>
        /// 为指定的实现了 IBindingsSetup 接口的类型在容器中实例化并注入，再按优先级排序，最后按
        /// 顺序执行它们自身所实现的 SetupBindings 方法
        /// </summary>
        void SetupBindings(IInjectionContainer container);
    }
}

