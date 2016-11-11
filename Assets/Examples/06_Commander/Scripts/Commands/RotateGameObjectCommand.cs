using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.Commander
{
	public class RotateGameObjectCommand : Command, IUpdatable
    {
		protected Transform objectToRotate;
		
		public override void Execute(params object[] parameters) {
			objectToRotate = (Transform)parameters[0];

            // 调用 Retain() 方法来保持 command 在 Execute() 方法执行后继续运行
            // 这使其可以接收 Update 事件。command 将不会释放，如需释放可调用 Release() 方法
            Retain();
		}

		public void Update () {
			objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}