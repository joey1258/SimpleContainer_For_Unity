using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantDefine
{
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
