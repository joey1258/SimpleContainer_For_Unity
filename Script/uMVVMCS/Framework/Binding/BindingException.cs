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
        public const string NULL_PARAMETER = "The parameter {0} of method {1} cannot be null.";
        public const string PARAMETERS_LENGTH_ERROR = "Parameter length is not correct";
        public const string TYPE_NOT_ASSIGNABLE = "The type or instance is not assignable to binding";
        public const string BINDINGTYPE_NOT_ASSIGNABLE = "Method {0} does not allow for {1} type of binding.";
        public const string TYPE_NOT_FACTORY = "The type doesn't implement IFactory.";
        public const string TYPE_NOT_OBJECT = "The type must be UnityEngine.Object.";
        public const string TYPE_NOT_COMPONENT = "The type must be UnityEngine.Component.";
        public const string GAMEOBJECT_IS_NULL = "GameObject is null";
        public const string PREFAB_IS_NULL = "prefab is null";
        public const string RESOURCE_IS_NULL = "resource is null";

        public BindingSystemException() : base() { }
        public BindingSystemException(string message) : base(message) { }
    }
}