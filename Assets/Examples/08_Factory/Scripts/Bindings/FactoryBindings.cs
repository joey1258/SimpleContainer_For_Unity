using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.Factory.Bindings
{
	public class PrefabsBindings : ToluaContainer.Container.IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container.BindFactory<GameObject>().To<CubeFactory>();
		}		
	}
}