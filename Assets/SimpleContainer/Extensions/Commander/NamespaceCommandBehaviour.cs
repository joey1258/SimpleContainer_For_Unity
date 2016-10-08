using UnityEngine;

namespace ToluaContainer.Container
{
    [AddComponentMenu("")]
    public abstract class NamespaceCommandBehaviour : MonoBehaviour
    {
        /// <summary>
        /// command 命名空间
        /// </summary>
        public string commandNamespace;

        /// <summary>
        /// command 名
        /// </summary>
        public string commandName;
    }
}