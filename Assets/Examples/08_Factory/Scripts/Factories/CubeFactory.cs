using UnityEngine;
using SimpleContainer.Container;
using SimpleContainer.Examples.Factory.Behaviours;

namespace SimpleContainer.Examples.Factory.Bindings
{
	public class CubeFactory : IInjectionFactory
    {
		protected const int MAX_COLUMNS = 6;
		protected IInjectionContainer container;
		protected int currentLine;
		protected int currentColumn;

		public CubeFactory(IInjectionContainer container)
        {
			this.container = container;
			this.container.Bind<Cube>().ToPrefab("08_Factory/Cube");
		}

		public object Create(InjectionInfo context)
        {
			var cube = this.container.Resolve<Cube>();

			var rotator = cube.gameObject.AddComponent<Rotator>();
			rotator.speed = Random.Range(0.05f, 5.0f);

			cube.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));

			var transform = cube.GetComponent<Transform>();
			transform.position = new Vector3(1.5f * this.currentColumn++, -1.5f * this.currentLine, 0);

			if (this.currentColumn >= MAX_COLUMNS) {
				this.currentLine++;
				this.currentColumn = 0;
			}

			return cube.gameObject;
		}
	}
}