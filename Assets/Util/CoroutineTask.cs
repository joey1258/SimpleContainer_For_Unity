using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一个 Task 代表一个协程，它可以开始、暂停、结束；
/// 版本5.5中试图重启 Task 已经不会再引发错误
/// </summary>
public class CoroutineTask
{
    #region property

    private CoroutineManager.CoroutineState state;

    /// <summary>
    /// 结束后的最后处理
    /// </summary>
    Action<CoroutineManager.CoroutineState> finisher { get; set; }

    /// <summary>
    /// 运行时返回true
    /// </summary>
    public bool running { get { return state.running; } }

    #endregion

    #region constructor

    public CoroutineTask(IEnumerator c, bool autoStart = true)
    {
        state = CoroutineManager.CreateState(c);
        state.finisher = TaskFinisher;
        if (autoStart) { Start(); }
    }

    public CoroutineTask(
        IEnumerator c,
        Action<CoroutineManager.CoroutineState> f,
        bool autoStart = true)
    {
        state = CoroutineManager.CreateState(c);
        finisher = f;
        state.finisher = TaskFinisher;
        if (autoStart) { Start(); }
    }

    #endregion

    #region functions

    /// <summary>
    /// 开始执行协程
    /// </summary>
    public void Start()
    {
        state.Start();
    }

    /// <summary>
    /// 在协程的 next yield 时中断执行
    /// </summary>
    public void Stop()
    {
        state.Stop();
    }

    /// <summary>
    /// 在协程结束后执行
    /// </summary>
    private void TaskFinisher(CoroutineManager.CoroutineState state)
    {
        if (finisher != null) { finisher(state); }
    }

    #endregion
}
