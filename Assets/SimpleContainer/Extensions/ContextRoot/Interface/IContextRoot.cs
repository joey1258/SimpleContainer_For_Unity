namespace ToluaContainer.Container
{
    public interface IContextRoot
    {
        /// <summary>
        /// 将 container添加到 containers List，并默认 destroyOnLoad 为真
        /// </summary>
        IInjectionContainer AddContainer<T>() where T : IInjectionContainer, new();

        /// <summary>
        /// 将 container添加到 containers List，并默认 destroyOnLoad 为真
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container);

        /// <summary>
        /// 将 container添加到 containers List，并设置其 destroyOnLoad 属性
        /// </summary>
        IInjectionContainer AddContainer(IInjectionContainer container, bool destroyOnLoad);

        /// <summary>
        /// Dispose 指定 id 的容器
        /// </summary>
        void Dispose(object id);

        /// <summary>
        /// 设置容器
        /// </summary>
        void SetupContainers();

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }
}