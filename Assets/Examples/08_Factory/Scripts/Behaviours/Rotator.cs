using UnityEngine;
using System.Collections;

namespace SimpleContainer.Examples.Factory.Behaviours
{
	public class Rotator : MonoBehaviour
    {
		public float speed;
		protected Transform cachedTransform;

		protected void Start()
        {
			this.cachedTransform = this.GetComponent<Transform>();
		}
		
		protected void Update()
        {
			this.cachedTransform.Rotate(this.speed, this.speed, this.speed);
		}
	}
}