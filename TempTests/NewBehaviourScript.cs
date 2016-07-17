using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        object o1 = 1;
        object o2 = 1;
        print(o1.Equals(o2));
        object o3 = new ren();
        object o4 = new ren();
        object o5 = o3;
        object o6 = o3;
        print(o3.Equals(o4));
        print(o5.Equals(o6));
    }

    // Update is called once per frame
    void Update () {
	
	}


}

public class ren { public int age; public string name; }





