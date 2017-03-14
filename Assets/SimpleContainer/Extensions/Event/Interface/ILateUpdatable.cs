namespace SimpleContainer
{
    /// <summary>
    /// 允许一个 object 接受每帧更新完成后调用一次
    /// </summary>
    public interface ILateUpdatable
    {
        /// <summary>
        /// 每帧更新完成后调用一次
        /// </summary>
        void LateUpdate();
    }
}