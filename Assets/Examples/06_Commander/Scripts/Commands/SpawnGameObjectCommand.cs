using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.Commander
{
	public class SpawnGameObjectCommand : Command
    {
		[Inject]
		public IInjectionContainer container;

		public override void Execute(params object[] parameters)
        {
			var prefab = container.Resolve<Transform>();
			dispatcher.Dispatch<RotateGameObjectCommand>(prefab);
		}
	}
}