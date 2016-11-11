using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.Factory.Bindings
{
	public class PrefabsBindings : SimpleContainer.Container.IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container.BindFactory<GameObject>().To<CubeFactory>();
		}		
	}
}