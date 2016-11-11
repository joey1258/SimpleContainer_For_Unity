namespace SimpleContainer
{
    public interface ICommandPool
    {
        /// <summary>
        /// 将容器中所有 ICommand 对象实例化并缓存到字典
        /// </summary>
        void Pool();

        /// <summary>
        /// 从对象池的字典中获取一个 command
        /// </summary>
        ICommand GetCommandFromPool(System.Type commandType);
    }
}