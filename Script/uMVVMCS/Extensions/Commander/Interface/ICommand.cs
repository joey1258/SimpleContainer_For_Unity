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
        /// <summary>The command dispatcher that dispatched this command.</summary>
        ICommandDispatcher dispatcher { get; set; }
        /// <summary>Indicates whether the command is running.</summary>
        bool running { get; set; }
        /// <summary>Indicates whether the command must be kept alive even after its execution.</summary>
        bool keepAlive { get; set; }
        /// <summary>
        /// Indicates whether this command is a singleton (there's only one instance of it).
        /// 
        /// Singleton commands improve performance and are the recommended approach when, for every execution
        /// of a command, there's no need to reinject dependencies and/or all parameters the command needs
        /// are passed through the <code>Execute()</code> method.
        /// </summary>
        bool singleton { get; }
        /// <summary>The quantity of the command to preload on pool.</summary>
        int preloadPoolSize { get; }
        /// <summary>The maximum size pool for this command.</summary>
        int maxPoolSize { get; }

        /// <summary>
        /// Executes the command.
        /// <param name="parameters">Command parameters.</param>
        void Execute(params object[] parameters);

        /// <summary>
        /// Retains the command as in use, not disposing it after execution.
        /// 
        /// Always call Release() after the command has terminated.
        /// </summary>
        void Retain();

        /// <summary>
        /// Release this command.
        /// </summary>
        void Release();
    }
}
