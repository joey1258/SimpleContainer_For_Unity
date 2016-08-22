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

using System;

namespace uMVVMCS.DIContainer
{
    public class BindingSystemException : Exception
    {
        public const string SAME_BINDING = "The binding with the same key and id already exists.";
        public const string PARAMETERS_LENGTH_ERROR = "Parameter length is not correct";
        public const string TYPE_NOT_ASSIGNABLE = "The type or instance is not assignable to binding";
        public const string BINDINGTYPE_NOT_ASSIGNABLE = "ParameterlessMethod {0} does not allow for {1} type of binding.";
        public const string TYPE_NOT_FACTORY = "The type doesn't implement IFactory.";
        public const string RESOURCES_LOAD_FAILURE = "Resources Load Failure! path: {0}";

        public BindingSystemException() : base() { }
        public BindingSystemException(string message) : base(message) { }
    }
}