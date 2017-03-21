using System;

namespace SimpleContainer.Container
{
    public interface IInjectionContainer : IBinder, IInjector, IDisposable
    {
        /// <summary>
        /// 初始化容器
        /// </summary>
        void Init();

        /// <summary>
        /// load 时是否摧毁容器
        /// </summary>
        bool destroyOnLoad { get; set; }

        /// <summary>
        /// 容器 id
        /// </summary>
		object id { get; }

        /// <summary>
        /// 反射信息缓存
        /// </summary>
        IReflectionCache cache { get; }

        /// <summary>
        /// 注册容器到 aots list
        /// </summary>
        IInjectionContainer RegisterAOT<T>() where T : IContainerAOT;

        /// <summary>
        /// 注册容器到 aots list 
        /// </summary>
        IInjectionContainer RegisterAOT(IContainerAOT extension);

        /// <summary>
        /// 将一个容器从 aots list 中移除 
        /// </summary>
        IInjectionContainer UnregisterAOT<T>() where T : IContainerAOT;

        /// <summary>
        /// 将一个容器从 aots list 中移除
        /// </summary>
        IInjectionContainer UnregisterAOT(IContainerAOT extension);

        /// <summary>
        /// 从 aots list 中获取指定容器
        /// </summary>
        T GetAOT<T>() where T : IContainerAOT;

        /// <summary>
        /// 从 aots list 中获取指定容器
        /// </summary>
        IContainerAOT GetAOT(Type type);

        /// <summary>
        /// 反回 aots list 中是否含有指定容器
        /// </summary>
        bool HasAOT<T>();

        /// <summary>
        /// 反回 aots list 中是否含有指定容器
        /// </summary>
        bool HasAOT(Type type);
    }
}