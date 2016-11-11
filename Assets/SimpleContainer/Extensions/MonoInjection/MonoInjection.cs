using UnityEngine;

namespace SimpleContainer.Container
{
    /// <summary>
    /// 为 MonoBehaviour 提供注入方法，默认 ContextRoot Extension 可以使用
    /// </summary>
    public static class MonoInjection
    {
        /// <summary>
        /// 为 MonoBehaviour 执行注入,参数 script 为注入目标
        /// </summary>
        public static void Inject(this MonoBehaviour script)
        {
            InjectionUtil.Inject(script);
        }

    }
}