/*
 * Copyright 2016 Sun Ning
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
    public class InjectionSystemException : Exception
    {
        public const string NO_CONSTRUCTORS = "There are no constructors on the type {0}, Is it an interface?";
        public const string PARAMETER_TYPE_ERROR = "Array or IList type parameters, like 'typeof(object[])' or 'typeof(IList<object>)' should be obtains the actual type on the outside of the method {0}";
        public const string SAME_OBJECT = "The object with the same key and id already exists.";

        public InjectionSystemException(string message) : base(message) { }
    }
}