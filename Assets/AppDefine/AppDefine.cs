using UnityEngine;
using System.Collections;

public static class AppDefine
{
    /// <summary>
    /// 场景根物体名称（切换场景不保留）
    /// </summary>
    public static string RootObjectName = "SceneRoot";

    /// <summary>
    /// 切换场景时不进行销毁的根物体名称
    /// </summary>
    public static string DDOLRootObjectName = "DDOLRoot";

    /// <summary>
    /// 应用程序名称
    /// </summary>
    public const string AppName = "SimpleContainer";

    /// <summary>
    /// 应用程序前缀
    /// </summary>
    public const string AppPrefix = AppName + "_";

    /// <summary>
    /// Streaming 目录 
    /// </summary>
    public const string AssetDir = "StreamingAssets";
}
