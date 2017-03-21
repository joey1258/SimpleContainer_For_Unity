namespace SimpleContainer
{
    public interface ICommandPool
    {
        /// <summary>
        /// 添加一个指定类型的 Command
        /// </summary>
        void AddCommand(System.Type type);

        /// <summary>
        /// 将指定类型的 Command 储存到 Pool
        /// </summary>
		void PoolCommand(System.Type commandType);

        /// <summary>
        /// 从对象池的字典中获取一个 command
        /// </summary>
        ICommand GetCommandFromPool(System.Type commandType);
    }
}