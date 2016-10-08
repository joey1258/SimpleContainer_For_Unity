using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.Prefabs
{
	public class PrefabsRoot : ContextRoot
    {
		public override void SetupContainers()
        {
			// 添加容器
			AddContainer<InjectionContainer>()
				.RegisterAOT<UnityContainerAOT>()
				// 实例化指定 prefab 并指定名称、执行注入
				.Bind<Transform>().ToPrefab("04_Prefabs/Cube").As("cube")
				// 实例化地面
				.BindSingleton<GameObject>().ToPrefab("04_Prefabs/Plane");
        }
		
		public override void Init() { }
	}
}