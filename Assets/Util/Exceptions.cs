/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/SimpleContainer_For_Unity
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;

namespace Utils
{
    public class Exceptions : Exception
    {
        // 参数相关
        public const string PARAMETER_NUll = "Parameter {0} is null!";
        public const string PARAMETERS_LENGTH_ERROR = "Parameter length is not correct";
        public const string PARAMETER_TYPE_ERROR = "Array or IList type parameters, like 'typeof(object[])' or 'typeof(IList<object>)' should be obtains the actual type on the outside of the method {0}";

        // 类型相关
        public const string TYPE_NOT_ASSIGNABLE = "The type or instance is not assignable to binding";
        public const string NON_SPECIFIED_TYPE = "The type must be {0}";

        // binding 相关
        public const string SAME_BINDING = "The binding with the same key and id already exists.";
        public const string BINDINGTYPE_NOT_ASSIGNABLE = "ParameterlessMethod {0} does not allow for {1} type of binding.";

        // load 资源相关
        public const string RESOURCES_LOAD_FAILURE = "Resources Load Failure! path: {0}";
        public const string ASSETBUNDLE_LOAD_FAILURE = "AssetBundle Load Failure! url: {0}";
        public const string RESOURCE_LOAD_FAILURE = "Resource Load Failure : {0} !";

        // 注入相关
        public const string NO_CONSTRUCTORS = "There are no constructors on the type {0}, Is it an interface?";
        public const string CANNOT_RESOLVE_MONOBEHAVIOUR = "A MonoBehaviour cannot be resolved directly.";

        // binding 的值相关
        public const string GAMEOBJECT_IS_NULL = "GameObject is null";
        public const string VALUE_ISNOT_PREFAB = "The value must be PrefabInfo.";
        public const string SAME_OBJECT = "The object with the same key and id already exists.";

        public Exceptions() : base() { }
        public Exceptions(string message) : base(message) { }
    }
}