using SimpleContainer.Container;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantDefine
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
    /// 项目名称
    /// </summary>
    public const string ProjectName = "SimpleContainer";

    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = ProjectName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
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
    /// 项目根目录
    /// </summary>
    public static string ProjectRoot
    {
        get
        {
            return Application.dataPath + "/" + ProjectName;
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
    /// 当前场景的 root Component
    /// </summary>
    public static IContextRoot RootComponent
    {
        get
        {
            return GameObject.Find(RootObjectName).GetComponent<IContextRoot>();
        }
    }

    /// <summary>
    /// 切换场景时不进行销毁的 root Component
    /// </summary>
    public static IContextRoot DDOLRootComponent
    {
        get
        {
            return GameObject.Find(DDOLRootObjectName).GetComponent<IContextRoot>();
        }
    }

    public static string DDOLRootName = "DDOLRoot";

    public static GameObject DDOLRoot
    {
        get
        {
            if (DDOLRootObj == null)
            {
                DDOLRootObj = GameObject.Find(DDOLRootName);

                if (DDOLRootObj == null)
                {
                    DDOLRootObj = new GameObject(DDOLRootName);
                }
            }
            return DDOLRootObj;
        }
        private set { }
    }
    private static GameObject DDOLRootObj = null;
}
