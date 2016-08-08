/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public abstract class Command : ICommand, IDisposable 
    {
        /// <summary>
        /// The command dispatcher that dispatched this command.
        /// </summary>
        public ICommandDispatcher dispatcher { get; set; }

        /// <summary>
        /// Indicates whether the command is running.
        /// </summary>
        public bool running { get; set; }

        /// <summary>
        /// command 是否必须在执行后也保持活动 
        /// </summary>
        public bool keepAlive { get; set; }

        /// <summary>
        /// 是否是单例 单例能提高性能，避免重复注入
        /// </summary>
        public virtual bool singleton { get { return true; } }

        /// <summary>
        /// command 对象池预加载数量，默认为1
        /// </summary>
        public virtual int preloadPoolSize { get { return 1; } }

        /// <summary>
        /// command 对象池大小，默认为10
        /// </summary>
        public virtual int maxPoolSize { get { return 10; } }

        /// <summary>
        /// 协程 list
        /// </summary>
        private List<Coroutine> coroutines = new List<Coroutine>(1);

        /// <summary>
        /// command 的执行方法
        /// </summary>
        public abstract void Execute(params object[] parameters);

        /// <summary>
        /// 保留 command，执行后不释放,如要释放可在执行后调用 Release() 方法
        /// </summary>
        public virtual void Retain()
        {
            keepAlive = true;
        }

        /// <summary>
        /// 释放 command.
        /// </summary>
        public virtual void Release()
        {
            keepAlive = false;

            dispatcher.Release(this);
        }

        /// <summary>
        /// 当需要释放 command 时调用
        /// </summary>
        public virtual void Dispose()
        {
            while (coroutines.Count > 0)
            {
                StopCoroutine(coroutines[0]);
            }
        }

        /// <summary>
        /// 等待指定秒数后执行 Action method,并使用其结果开始协程
        /// </summary>
        protected void Invoke(Action method, float time)
        {
            var routine = this.MethodInvoke(method, time);
            StartCoroutine(routine);
        }

        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="routine">Routine to be started.</param>
        /// <returns>The coroutine.</returns>
        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = EventCallerContainerExtension.eventCaller.StartCoroutine(routine);
            this.coroutines.Add(coroutine);
            this.Retain();

            return coroutine;
        }

        /// <summary>
        /// Stops a coroutine.
        /// </summary>
        /// <param name="coroutine">Coroutine to be stopped.</param>
        protected void StopCoroutine(Coroutine coroutine)
        {
            EventCallerContainerExtension.eventCaller.StopCoroutine(coroutine);
            this.coroutines.Remove(coroutine);
        }

        /// <summary>
        /// 等待指定秒数后执行 Action method
        /// </summary>
        private IEnumerator MethodInvoke(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }
    }
}
