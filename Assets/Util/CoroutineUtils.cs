using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _Instance = null;

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = GameObject.Find("MonoObject");
                if (go == null)
                {
                    go = new GameObject("MonoObject");
                    DontDestroyOnLoad(go);
                }
                _Instance = go.AddComponent<T>();

            }
            return _Instance;
        }
    }

    /// <summary>
    /// 程序退出事件
    /// </summary>
    private void OnApplicationQuit()
    {
        _Instance = null;
    }
}

public class CoroutineUtils : MonoSingleton<CoroutineUtils> { }
