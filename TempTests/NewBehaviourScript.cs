using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //Arrange 
        IBinder binder = new Binder();
        //Act
        IBinding binding1 = binder.Bind<object>().ToSelf();
        IBinding binding2 = binder.Bind<object>().To<object>();

        print(binding1.bindingType);
        print(binder.GetAllBindings().Count);
    }

    // Update is called once per frame
    void Update () {
	
	}


}

public class ren { public int age; public string name; }





