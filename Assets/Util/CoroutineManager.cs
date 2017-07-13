using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    /// <summary>
    /// 直接 new CoroutineState 是无效的，因此将这个类作为 CoroutineManager 的内部类，
    /// 并用 CoroutineTask 类对其进行一层包装，使其在构造函数中自动通过 CoroutineManager
    /// 创建 CoroutineState，保证其正常运行
    /// </summary>
    public class CoroutineState
    {
        #region property

        /// <summary>
        /// 结束后的最后处理
        /// </summary>
        public Action<CoroutineState> finisher { get; set; }

        /// <summary>
        /// 协程实例
        /// </summary>
        private IEnumerator coroutine;

        /// <summary>
        /// 运行时返回true
        /// </summary>
        public bool running
        {
            get
            {
                return _running;
            }
        }
        private bool _running;

        /// <summary>
        /// 为 true 时需要回收
        /// </summary>
        private bool recycle = false;

        #endregion

        #region constructor

        public CoroutineState(IEnumerator c)
        {
            coroutine = c;
        }

        #endregion

        #region functions

        public void Start()
        {
            _running = true;
            singleton.StartCoroutine(CoroutineLooper());
        }

        public void Stop()
        {
            _running = false;
        }

        /// <summary>
        /// 5.0 版本 IEnumerator 停止后重新开始不再引发错误，因此取消暂停，直接用 Stop()
        /// 方法停止协程即可，之后再次调用 Start() 方法即可从上次的位置的继续
        /// </summary>
        private IEnumerator CoroutineLooper()
        {
            // 如果在一个对象的前期调用协程，协程会立即运行到第一个 yield return 语句处，
            // 如果是 yield return null，就会在同一帧再次被唤醒。如果没有考虑这个细节就
            // 会出现一些奇怪的问题
            yield return null;

            // 如果处于运行状态就保持循环
            while (running)
            {
                // 只有当MoveNext()返回 true时才可以访问 Current
                if (coroutine != null && coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }
                else
                {
                    // 否则代表循环已经结束，停止运行状态
                    _running = false;
                }
            }

            // 跳出循环后处理结束事务,将 IEnumerator coroutine 传递进方法以便后续操作
            if (finisher != null) { finisher(this); }
        }

        #endregion
    }

    #region functions

    /// <summary>
    /// 创建 state 实例
    /// </summary>
    public static CoroutineState CreateState(IEnumerator coroutine)
    {
        if (singleton == null)
        {
            singleton = Utils.UnityUtils.GetComponent<CoroutineManager>(ConstantDefine.DDOLRoot);
        }
        return new CoroutineState(coroutine);
    }

    #endregion

    public static CoroutineManager singleton;
}
