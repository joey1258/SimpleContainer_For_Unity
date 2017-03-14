namespace SimpleContainer
{
    /// <summary>
    /// 允许接收 OnApplicationPause 事件
    /// </summary>
    public interface IPausable
    {
        /// <summary>
        /// OnApplicationPause 事件调用
        /// </summary>
        void OnApplicationPause(bool isPaused);
    }
}

