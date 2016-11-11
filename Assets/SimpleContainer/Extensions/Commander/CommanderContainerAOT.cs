using SimpleContainer.Container;

namespace SimpleContainer
{
    public class CommanderContainerAOT : IContainerAOT
    {
        public void OnRegister(IInjectionContainer container)
        {
            // 绑定一个单例的 ICommandDispatcher binding
            container.BindSingleton<ICommandDispatcher>().To<CommandDispatcher>();
            // 实例化 binding value，所有命令都通过该实例化后的 value 传播
            var dispatcher = (CommandDispatcher)container.Resolve<ICommandDispatcher>();
            // 将实例化后的 binding value 作为值绑定一条 ICommandPool 类型的 binding
            // 此时 container 中将有两条 binding，都为单例类型，且值都为 dispatcher，只有类型不同
            container.Bind<ICommandPool>().To(dispatcher);
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