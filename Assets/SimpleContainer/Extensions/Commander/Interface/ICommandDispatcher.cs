using System;
using System.Collections;
using UnityEngine;

namespace SimpleContainer
{
    public interface ICommandDispatcher
    {
        void Init();

        /// <summary>
        /// 发送一个指定类型的 command
        /// </summary>
        void Dispatch<T>(params object[] parameters) where T : ICommand;

        /// <summary>
        /// 发送一个指定类型的 command
        /// </summary>
        void Dispatch(Type type, params object[] parameters);

        /// <summary>
        /// 通过 EventContainerAOT.eventBehaviour 在等待指定秒后发送一个 command 
        /// </summary>
        void InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand;

        /// <summary>
        /// 通过 EventContainerAOT.eventBehaviour 在等待指定秒后发送一个 command 
        /// </summary>
        void InvokeDispatch(Type type, float time, params object[] parameters);

        /// <summary>
        /// 释放 command
        /// </summary>
        void Release(ICommand command);

        /// <summary>
        /// 释放所有 command
        /// </summary>
        void ReleaseAll();

        /// <summary>
        /// 释放指定类型的所有 command
        /// </summary>
        void ReleaseAll<T>() where T : ICommand;

        /// <summary>
        /// 释放指定类型的所有 command
        /// </summary>
        void ReleaseAll(Type type);

        /// <summary>
        /// 返回 commands 字典中是否含有指定类型的 command
        /// </summary>
        bool ContainsCommands<T>() where T : ICommand;

        /// <summary>
        /// 返回 commands 字典中是否含有指定类型的 command
        /// </summary>
        bool ContainsCommands(Type type);

        /// <summary>
        /// 返回字典中的所有 key（类型）
        /// </summary>
        Type[] GetAllRegistrations();

        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// Stops a coroutine.
        /// </summary>
        void StopCoroutine(Coroutine coroutine);
    }
}

