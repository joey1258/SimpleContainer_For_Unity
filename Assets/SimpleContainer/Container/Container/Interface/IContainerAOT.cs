namespace ToluaContainer.Container
{
    public interface IContainerAOT
    {
        /// <summary>
        /// 当容器注入到容器 list 时被调用，当被调用时可以提供任何容器事件
        /// </summary>
        void OnRegister(IInjectionContainer container);

        /// <summary>
        /// 当容器注入到容器 list 时被调用，当被调用时可以退订任何容器事件
        /// </summary>
        void OnUnregister(IInjectionContainer container);
    }
}