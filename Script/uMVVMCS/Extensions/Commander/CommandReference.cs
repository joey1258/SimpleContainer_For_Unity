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

using System;
using uMVVMCS.DIContainer;

namespace uMVVMCS
{
    [Serializable]
    public class CommandReference
    {
        /// <summary>
        /// command namespace
        /// </summary>
        public string commandNamespace;

        /// <summary>
        /// command name
        /// </summary>
        public string commandName;

        /// <summary>
        /// 根据自身的命名空间与名称发送 command.
        /// </summary>
        public void DispatchCommand(params object[] parameters)
        {
            var type = TypeUtils.GetType(this.commandNamespace, this.commandName);
            CommanderUtils.DispatchCommand(type, parameters);
        }
    }
}