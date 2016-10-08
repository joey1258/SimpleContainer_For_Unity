using UnityEngine;

namespace ToluaContainer
{
    /// <summary>
    /// 为 UnityEngine.StateMachineBehaviour 提供注入方法，以有可用 ContextRoot Extension 为前提
    /// </summary>
    public static class StateInjectionExtension
    {
        /// <summary>
        /// 注入方法
        /// </summary>
        public static void Inject(this StateMachineBehaviour script)
        {
            InjectionUtil.Inject(script);
        }
    }
}