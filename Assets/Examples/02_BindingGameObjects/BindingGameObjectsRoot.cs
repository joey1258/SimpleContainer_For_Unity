using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.BindingGameObjects
{
    public class BindingGameObjectsRoot : ContextRoot
    {
        public override void SetupContainers()
        {
            // 添加容器
            AddContainer<InjectionContainer>()
                // 注册 AOT 容器扩展
                .RegisterAOT<UnityContainerAOT>()
                // 绑定 Transform 组件到 gameObject "Cube"
                .Bind<Transform>().ToGameObject("Cube")
                // 绑定 GameObjectRotator 脚本到一个与脚本同名的新的空物体来控制 Cube 转动
                .Bind<RotateController>().ToGameObject()
                ;
        }

        public override void Init() { }
    }
}
