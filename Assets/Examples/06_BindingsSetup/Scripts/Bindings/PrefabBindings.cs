using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.BindingsSetup.Bindings
{
	public class PrefabBindings : IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeA")
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeB")
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeC");
		}		
	}
}