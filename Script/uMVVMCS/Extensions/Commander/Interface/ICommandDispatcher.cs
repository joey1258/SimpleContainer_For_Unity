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
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatches a command by type.
        /// </summary>
        void Dispatch<T>(params object[] parameters) where T : ICommand;

        /// <summary>
        /// Dispatches a command by type.
        /// </summary>
        void Dispatch(Type type, params object[] parameters);

        /// <summary>
        /// Dispatches a command by type after a given time in seconds.
        /// </summary>
        void InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand;

        /// <summary>
        /// Dispatches a command by type after a given time in seconds.
        /// </summary>
        void InvokeDispatch(Type type, float time, params object[] parameters);

        /// <summary>
        /// Releases a command.
        /// </summary>
        void Release(ICommand command);

        /// <summary>
        /// Releases all commands that are running.
        /// </summary>
        void ReleaseAll();

        /// <summary>
        /// Releases all commands that are running.
        /// </summary>
        void ReleaseAll<T>() where T : ICommand;

        /// <summary>
        /// Releases all commands that are running.
        /// </summary>
        void ReleaseAll(Type type);

        /// <summary>
        /// Checks whether a given command of <typeparamref name="T"/> is registered.
        /// </summary>
        bool ContainsRegistration<T>() where T : ICommand;

        /// <summary>
        /// Checks whether a given command of <paramref name="type"/> is registered.
        /// </summary>
        bool ContainsRegistration(Type type);

        /// <summary>
        /// Gets all commands registered in the command dispatcher.
        /// </summary>
        Type[] GetAllRegistrations();
    }
}

