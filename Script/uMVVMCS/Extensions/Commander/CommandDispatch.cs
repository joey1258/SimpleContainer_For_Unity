/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

using UnityEngine;
using System;

namespace uMVVMCS.DIContainer
{
    [AddComponentMenu("uMVVMCS/Command dispatch")]
    public class CommandDispatch : NamespaceCommandBehaviour
    {
        /// <summary>
        /// 要调用的 command 的类型
        /// </summary>
        protected Type commandType;

        /// <summary>
        /// 当脚步初始化时调用
        /// </summary>
        protected void Awake()
        {
            commandType = TypeUtils.GetType(commandNamespace, commandName);
        }

        /// <summary>
        /// 发送 command.
        /// </summary>
        public void DispatchCommand()
        {
            CommanderUtils.DispatchCommand(commandType);
        }

        /// <summary>
        /// 发送 command.
        /// </summary>
        public void DispatchCommand(params object[] parameters)
        {
            CommanderUtils.DispatchCommand(commandType, parameters);
        }
    }
}