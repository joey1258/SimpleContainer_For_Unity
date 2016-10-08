using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.BindingsSetup
{
	public class GameRoot : ContextRoot {
		public override void SetupContainers()
        {
			AddContainer<InjectionContainer>()
				.RegisterAOT<UnityContainerAOT>()
				.SetupBindings("ToluaContainer.Examples.BindingsSetup.Bindings");
        }
		
		public override void Init() {

		}
	}
}