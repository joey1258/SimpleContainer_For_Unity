using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.UsingConditions
{
    public class UsingConditionsRoot : ContextRoot
    {
        public override void SetupContainers()
        {
            // 添加容器
            AddContainer<InjectionContainer>()
                // 注册 AOT 容器扩展
                .RegisterAOT<UnityContainerAOT>()
                // 绑定场景中的两个方块的 Transform 组件，并标记 id 为方块的名称
                .Bind<Transform>().ToGameObject("LeftCube").As("LeftCube")
                .Bind<Transform>().ToGameObject("RightCube").As("RightCube")
                // 新建一个空物体来挂载 GameObjectRotator 组件
                .Bind<RotateController>().ToGameObject();
        }

        public override void Init() { }
    }
}
