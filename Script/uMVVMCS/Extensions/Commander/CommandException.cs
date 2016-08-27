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

namespace uMVVMCS.DIContainer
{
    public class CommandException : Exception
    {
        public const string TYPE_NOT_A_COMMAND = "The type is not a command.";
        public const string MAX_POOL_SIZE = "Reached max pool size for command {0}.";
        public const string NO_COMMAND_FOR_TYPE = "no command registered for the type {0}.";

        #region constructor

        public CommandException(string message) : base(message) { }

        #endregion
    }
}