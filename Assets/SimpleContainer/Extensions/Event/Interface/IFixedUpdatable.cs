namespace SimpleContainer
{
    /// <summary>
    /// 允许一个 object 接受 FixedUpdata 更新
    /// </summary>
    public interface IFixedUpdatable
    {
        /// <summary>
        /// 每次 FixedUpdata 调用
        /// </summary>
        void FixedUpdate();
    }
}