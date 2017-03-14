using UnityEngine;

namespace SimpleContainer
{
    public class EventBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 所关联的 EventContainerAOT
        /// </summary>
        public EventContainerAOT aot { get; set; }

        protected void Update()
        {
            // 如果游戏暂停则不执行（Mathf.Approximately 方法用于比较浮点值，由于浮点值不精确所以运算时可能产生误差，所以必须使用特殊的方法来比较）
            if (Mathf.Approximately(Time.deltaTime, 0)) { return; }

            for (var i = 0; i < aot.updateable.Count; i++)
            {
                aot.updateable[i].Update();
            }
        }

        protected void LateUpdate()
        {
            // 如果游戏暂停则不执行（Mathf.Approximately 方法用于比较浮点值，由于浮点值不精确所以运算时可能产生误差，所以必须使用特殊的方法来比较）
            if (Mathf.Approximately(Time.deltaTime, 0)) { return; }

            for (var i = 0; i < aot.lateUpdateable.Count; i++)
            {
                aot.lateUpdateable[i].LateUpdate();
            }
        }

        protected void FixedUpdate()
        {
            for (var i = 0; i < aot.fixedUpdateable.Count; i++)
            {
                aot.fixedUpdateable[i].FixedUpdate();
            }
        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            for (var i = 0; i < aot.focusable.Count; i++)
            {
                aot.focusable[i].OnApplicationFocus(hasFocus);
            }
        }

        protected void OnApplicationPause(bool isPaused)
        {
            for (var i = 0; i < aot.pausable.Count; i++)
            {
                aot.pausable[i].OnApplicationPause(isPaused);
            }
        }

        protected void OnApplicationQuit()
        {
            for (var i = 0; i < aot.quitable.Count; i++)
            {
                aot.quitable[i].OnApplicationQuit();
            }
        }

        protected void OnDestroy()
        {
            for (int i = 0; i < aot.disposable.Count; i++)
            {
                aot.disposable[i].Dispose();
            }
        }
    }
}
