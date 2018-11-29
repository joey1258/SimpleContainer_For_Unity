using UnityEngine;
using SimpleContainer.Container;
using SimpleContainer.Examples.BindingsSetup.Data;

namespace SimpleContainer.Examples.BindingsSetup.Bindings
{
	[Priority]
	public class DataBindings : IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(0.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeA"))
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(2.0f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeB"))
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(4.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeC"));
		}		
	}
}