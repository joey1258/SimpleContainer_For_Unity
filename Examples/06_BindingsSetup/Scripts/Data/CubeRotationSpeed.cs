using UnityEngine;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.BindingsSetup
{
	/// <summary>
	/// Cube rotation speed value object.
	/// </summary>
	public class CubeRotationSpeed {		
		public float speed { get; set; }

		public CubeRotationSpeed(float speed) {
			this.speed = speed;
		}
	}
}