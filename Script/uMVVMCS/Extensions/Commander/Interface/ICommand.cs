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
using System.Collections.Generic;

namespace uMVVMCS.DIContainer
{
    public interface ICommand
    {
        /// <summary>
        /// commandDispatcher
        /// </summary>
        ICommandDispatcher dispatcher { get; set; }

        /// <summary>
        /// command 是否在运行中
        /// </summary>
        bool running { get; set; }

        /// <summary>
        /// Indicates whether the command must be kept alive even after its execution.
        /// </summary>
        bool keepAlive { get; set; }

        /// <summary>
        /// 是否是单例 单例能提高性能，避免重复注入
        /// </summary>
        bool singleton { get; }

        /// <summary>
        /// command 对象池预加载数量，默认为1
        /// </summary>
        int preloadPoolSize { get; }

        /// <summary>
        /// command 对象池大小，默认为10
        /// </summary>
        int maxPoolSize { get; }

        /// <summary>
        /// command 的执行方法
        /// </summary>
        void Execute(params object[] parameters);

        /// <summary>
        /// 保留 command，执行后不释放,如要释放可在执行后调用 Release() 方法
        /// </summary>
        void Retain();

        /// <summary>
        /// 释放 command.
        /// </summary>
        void Release();
    }
}
