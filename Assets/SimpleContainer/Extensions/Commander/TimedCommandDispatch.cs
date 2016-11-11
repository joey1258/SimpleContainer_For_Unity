using UnityEngine;

namespace SimpleContainer.Container
{
    [AddComponentMenu("SimpleContainer/Timed command dispatch")]
    public class TimedCommandDispatch : CommandDispatch
    {
        /// <summary>
        /// 持续秒数
        /// </summary>
        public float timer;

        /// <summary>
        /// 当脚本激活时带调用
        /// </summary>
        protected void OnEnable()
        {
            Invoke("DispatchCommand", timer);
        }
    }
}