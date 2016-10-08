using UnityEngine;
using ToluaContainer.Container;

public class AssetBundleRoot : ContextRoot
{
    public override void SetupContainers()
    {
        // 添加容器
        AddContainer<InjectionContainer>()
            .RegisterAOT<UnityContainerAOT>()
            // 绑定指定 AssetBundleInfo 资源并实例化为游戏物体
            .Bind<AssetBundleInfo>().ToAssetBundleFromFile(Application.dataPath + "/Examples/05_AssetBundle/Resources/cube.prefab.unity3d").Instantiate("cube")
            // 获取实例化的物体组件并指定 id、执行注入
            .Bind<Transform>().ToGameObject("cube").As("cube")
            // 实例化地面
            .BindSingleton<GameObject>().ToPrefab("04_Prefabs/Plane");
    }

    public override void Init() { }
}
