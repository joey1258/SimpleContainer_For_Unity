using UnityEngine;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.BindingsSetup
{
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			AddContainer<InjectionContainer>()
				//Register any extensions the container may use.
				.RegisterAOT<UnityContainer>()
				//Setups bindings from a namespace.
				.SetupBindings("uMVVMCS.Examples.BindingsSetup.Bindings");

            /*print(((CubeRotationSpeed)containers[0].GetBinding<CubeRotationSpeed>(1).value).speed);
            print(((CubeRotationSpeed)containers[0].GetBinding<CubeRotationSpeed>(2).value).speed);
            print(((CubeRotationSpeed)containers[0].GetBinding<CubeRotationSpeed>(3).value).speed);*/
        }
		
		public override void Init() {

		}
	}
}