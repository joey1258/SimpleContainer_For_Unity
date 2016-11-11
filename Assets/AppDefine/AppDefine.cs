using UnityEngine;
using System.Collections;
using System.Net;
using SimpleContainer.Container;
using System.Text;

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

    /// <summary>
    /// 测试更新地址（请根据自己的测试服务器地址进行更改）
    /// </summary>
    public static IPAddress ipAddress = IPAddress.Parse("192.168.232.145");

    /// <summary>
    /// 端口号
    /// </summary>
    public static int[] prot = new int[] { 1666, 3657, };

    /// <summary>
    /// Socket服务器端口
    /// </summary>
    public static int SocketPort = 0;

    /// <summary>
    /// 网络超时设定
    /// </summary>
    public static int sendTimeout = 1000;
    public static int receiveTimeout = 1000;
    public static int asyncSendTimeout = 1500;
    public static int asyncReceiveTimeout = 1500;

    /// <summary>
    /// 默认编码
    /// </summary>
    public static Encoding defaultEncode = Encoding.UTF8;

    /// <summary>
    /// Debug 模式（开启后 DataPath 根据 Application.dataPath 返回路径）
    /// </summary>
    public const bool DebugMode = false;

    /// <summary>
    /// AssetBundle 文件扩展名
    /// </summary>
    public const string ExtName = ".unity3d";

    /// <summary>
    /// 用户ID
    /// </summary>
    public static string UserId = string.Empty;

    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }
            if (AppDefine.DebugMode)
            {
                return Application.dataPath + "/" + AssetDir + "/";
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1) + game + "/";
            }
            return "c:/" + game + "/";
        }
    }

    /// <summary>
    /// 框架根目录
    /// </summary>
    public static string FrameworkRoot
    {
        get
        {
            return Application.dataPath + "/" + AppName;
        }
    }

    /// <summary>
    /// 本地目录(更多对应平台请自行添加)
    /// </summary>
    public static string LocalFilePath
    {
        get
        {
#if UNITY_ANDROID
		return "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		return Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            return "file://" + Application.dataPath + "/StreamingAssets/";
#else
        return string.Empty;
#endif
        }
    }

    /// <summary>
    /// 场景根物体
    /// </summary>
    public static GameObject RootObject
    {
        get
        {
            return GameObject.Find(RootObjectName);
        }
    }

    /// <summary>
    /// 切换场景时不进行销毁的根物体
    /// </summary>
    public static GameObject DDOLRootObject
    {
        get
        {
            return GameObject.Find(DDOLRootObjectName);
        }
    }

    /// <summary>
    /// 当前场景的 root
    /// </summary>
    public static IContextRoot Root
    {
        get
        {
            return GameObject.Find(RootObjectName).GetComponent<IContextRoot>();
        }
    }

    /// <summary>
    /// 切换场景时不进行销毁的 root
    /// </summary>
    public static IContextRoot DDOLRoot
    {
        get
        {
            return GameObject.Find(DDOLRootObjectName).GetComponent<IContextRoot>();
        }
    }
}
