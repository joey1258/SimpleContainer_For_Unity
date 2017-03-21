using SimpleContainer.Container;

namespace SimpleContainer
{
    public class CommanderContainerAOT : IContainerAOT
    {
        public void Init(IInjectionContainer container)
        {
            container.Resolve<ICommandDispatcher>().Init();
        }

        public void OnRegister(IInjectionContainer container)
        {
            // 创建一条 CommandDispatcher 实例
            CommandDispatcher dispatcher = new CommandDispatcher(container);
            // 将实例绑定到绑定一个单例的 ICommandDispatcher binding
            container.BindSingleton<ICommandDispatcher>().To(dispatcher);
            // 再将实例绑定到一个 ICommandPool binding，此时 container 中将有两条 binding
            // 都为单例类型，且值都为 dispatcher，只有类型不同
            container.BindSingleton<ICommandPool>().To(dispatcher);
        }

        public void OnUnregister(IInjectionContainer container)
        {
            // Unbind ICommandDispatcher 类型和 ICommandPool 类型 binding（清除 dispatcher)
            container.UnbindType<ICommandDispatcher>();
            container.UnbindType<ICommandPool>();

            // 清除所有 commands
            container.UnbindType<ICommand>();
        }
    }
}