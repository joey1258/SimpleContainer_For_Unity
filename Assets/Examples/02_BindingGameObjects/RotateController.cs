using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.BindingGameObjects
{
    public class RotateController : MonoBehaviour
    {
        [Inject]
        public Transform objectToRotate;

        void Start()
        {
            // 为自身带有 [Inject] 特性的成员进行注入
            this.Inject();
        }

        void Update()
        {
            this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
        }
    }
}
