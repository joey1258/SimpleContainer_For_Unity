using ToluaContainer.Container;
using ToluaContainer.Examples.Factory.Commands;

namespace ToluaContainer.Examples.Factory
{
	public class GameRoot : ContextRoot
    {
		protected ICommandDispatcher dispatcher;

		public override void SetupContainers()
        {
			this.dispatcher = this.AddContainer<InjectionContainer>()		
				.RegisterAOT<CommanderContainerAOT>()
				.RegisterAOT<EventContainerAOT>()
				.RegisterAOT<UnityContainerAOT>()
				.SetupBindings("ToluaContainer.Examples.Factory.Bindings")			
				.RegisterCommands("ToluaContainer.Examples.Factory.Commands")
				.GetCommandDispatcher();
		}
		
		public override void Init()
        {
			this.dispatcher.Dispatch<SpawnObjectsCommand>();
		}
	}
}