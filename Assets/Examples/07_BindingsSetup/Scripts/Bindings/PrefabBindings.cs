using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.BindingsSetup.Bindings
{
	public class PrefabBindings : IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container
				.BindSingleton<Transform>().ToPrefab("07_BindingsSetup/CubeA")
				.BindSingleton<Transform>().ToPrefab("07_BindingsSetup/CubeB")
				.BindSingleton<Transform>().ToPrefab("07_BindingsSetup/CubeC");
		}		
	}
}