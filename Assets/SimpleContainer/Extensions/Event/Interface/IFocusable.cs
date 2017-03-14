namespace SimpleContainer
{
    /// <summary>
    /// 允许一个 object 接受 OnApplicationFocus 信息
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        /// 每次应用焦点变化时调用
        /// </summary>
        void OnApplicationFocus(bool hasFocus);
    }
}

