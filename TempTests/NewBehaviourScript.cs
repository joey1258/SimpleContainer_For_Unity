using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        object o = new ren();
        print(o as Type);
    }

    // Update is called once per frame
    void Update () {
	
	}


}

public class ren { public int age; public string name; }





