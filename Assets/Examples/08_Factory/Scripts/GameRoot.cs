using SimpleContainer.Container;
using SimpleContainer.Examples.Factory.Commands;

namespace SimpleContainer.Examples.Factory
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
				.SetupBindings("SimpleContainer.Examples.Factory.Bindings")			
				.RegisterCommands("SimpleContainer.Examples.Factory.Commands")
				.GetCommandDispatcher();
		}
		
		public override void Init()
        {
			this.dispatcher.Dispatch<SpawnObjectsCommand>();
		}
	}
}