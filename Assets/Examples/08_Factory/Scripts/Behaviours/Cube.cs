using UnityEngine;
using System.Collections;

namespace ToluaContainer.Examples.Factory.Behaviours
{
	[RequireComponent(typeof(Renderer))]
	public class Cube : MonoBehaviour {
		public Color color {
			get { return this.GetComponent<Renderer>().material.color; }
			set
            {
				var material = new Material(Shader.Find("Standard")) { color = value };
				this.GetComponent<Renderer>().material = material;
			}
		}
	}
}