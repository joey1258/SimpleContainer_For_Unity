using UnityEngine;
using System.Collections;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.BindingsSetup
{
	/// <summary>
	/// Cube rotator.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		/// <summary>Speed data for the cube.</summary>
		[Inject]
		public CubeRotationSpeed speedData;

		/// <summary>The cached transform object.</summary>
		protected Transform cachedTransform;

		protected void Start() {
			cachedTransform = GetComponent<Transform>();

			//Call "Inject" to inject any dependencies in the component.
			//In a production game, it's useful to place this in a base component.
			this.Inject();
            print(speedData == null);
            print(speedData.speed);
        }

		protected void Update() {
			cachedTransform.Rotate(speedData.speed, speedData.speed, speedData.speed);
		}
	}
}