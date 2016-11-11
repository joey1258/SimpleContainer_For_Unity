using UnityEngine;
using SimpleContainer.Container;

public class Root : ContextRoot {

    // Use this for initialization
    public override void SetupContainers () {
		var container = AddContainer<InjectionContainer>()
		.Bind<Hello>().To<HelloA>().As("A")
		.Bind<Hello>().To<HelloB>().As("B")
		.Bind<PrintHelloWorld>().ToGameObject();
	
	}

    // Update is called once per frame
    public override void Init () {
	
	}
}
