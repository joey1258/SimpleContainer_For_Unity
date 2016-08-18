using UnityEngine;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.Commander
{
	public class GameRoot : ContextRoot
    {
		protected ICommandDispatcher dispatcher;

		public override void SetupContainers()
        {
			// 添加容器
			var container = AddContainer<InjectionContainer>();

			container
				// 注册所需的容器扩展
				.RegisterAOT<CommanderContainer>()
				.RegisterAOT<EventContainer>()
				.RegisterAOT<UnityContainer>()
                // 注册 "uMVVMCS.Examples.Commander" 命名空间下的所有 Command
                .RegisterCommands("uMVVMCS.Examples.Commander")
				// 绑定 prefab
				.Bind<Transform>().ToPrefab("05_Commander/Prism");
		
			// 获取 commandDispatcher 以便在 Init() 方法中调用
			dispatcher = container.GetCommandDispatcher();
		}
		
		public override void Init()
        {
			dispatcher.Dispatch<SpawnGameObjectCommand>();
		}
	}
}