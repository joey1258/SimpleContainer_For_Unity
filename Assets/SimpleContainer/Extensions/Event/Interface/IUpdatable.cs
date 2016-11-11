namespace SimpleContainer
{
    /// <summary>
    /// 允许一个 object 接受 updates 更新
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// 每帧调用
        /// </summary>
        void Update();
    }
}

