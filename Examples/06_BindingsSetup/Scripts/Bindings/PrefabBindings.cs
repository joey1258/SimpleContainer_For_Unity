using UnityEngine;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.BindingsSetup.Bindings {
	/// <summary>
	/// Bindings for prefabs.
	/// </summary>
	public class PrefabBindings : IBindingsSetup {
		public void SetupBindings(IInjectionContainer container) {
			container
				//Bind the "CubeA" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeA")
				//Bind the "CubeB" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeB")
				//Bind the "CubeC" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeC");
		}		
	}
}