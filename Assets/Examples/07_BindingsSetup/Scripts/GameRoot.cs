using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.BindingsSetup
{
	public class GameRoot : ContextRoot {
		public override void SetupContainers()
        {
			AddContainer<InjectionContainer>()
				.RegisterAOT<UnityContainerAOT>()
				.SetupBindings("SimpleContainer.Examples.BindingsSetup.Bindings");
        }
		
		public override void Init() {

		}
	}
}