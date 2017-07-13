using UnityEngine;
using System.Collections;

namespace Utils
{
    public static class UnityUtils
    {
        /// <summary>
        /// 获取组件，如果没有该组件就添加后再返回
        /// </summary>
        public static T GetComponent<T>(this GameObject go) where T : Component
        {
            T ret = go.GetComponent<T>();
            if (ret == null)
                ret = go.AddComponent<T>();
            return ret;
        }
    }
}
