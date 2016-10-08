using ToluaContainer.Container;

namespace ToluaContainer.Examples.HelloWorld
{
    public class HelloWorldRoot : ContextRoot
    {
        public override void SetupContainers()
        {
            // 创建容器
            var container = AddContainer<InjectionContainer>();
            // 绑定一个 ADDRESS 类型 Binding，其值为 HelloWorld（类型） 
            container.Bind<HelloWorld>().ToAddress();
            // Resolve 类型实例并调用 DisplayHelloWorld() 方法，输出 "Hello, world!" 
            container.Resolve<HelloWorld>().DisplayHelloWorld();
        }

        public override void Init() { }
    }
}