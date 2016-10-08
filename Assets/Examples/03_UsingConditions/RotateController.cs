using UnityEngine;

namespace ToluaContainer.Examples.UsingConditions
{
    public class RotateController : MonoBehaviour
    {
        [Inject("LeftCube")]
        public Transform LeftCube;

        void Start() { }

        void Update()
        {
            // 在 c# 中使 LeftCube 旋转
            LeftCube.Rotate(1.0f, 1.0f, 1.0f);
        }
    }
}

