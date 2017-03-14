namespace SimpleContainer
{
    /// <summary>
    /// 当 app 退出时调用
    /// </summary>
    public interface IQuitable
    {
        /// <summary>
        /// 当 app 退出时调用
        /// </summary>
        void OnApplicationQuit();
    }
}

