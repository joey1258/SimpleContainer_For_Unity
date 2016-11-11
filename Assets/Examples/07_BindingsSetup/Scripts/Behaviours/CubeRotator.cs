using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.BindingsSetup.Behaviours
{
	public class CubeRotator : MonoBehaviour
    {
		[Inject]
		public Data.CubeRotationSpeed speedData;

		protected Transform cachedTransform;

		protected void Start()
        {
			cachedTransform = GetComponent<Transform>();
			this.Inject();
        }

		protected void Update()
        {
			cachedTransform.Rotate(speedData.speed, speedData.speed, speedData.speed);
		}
	}
}