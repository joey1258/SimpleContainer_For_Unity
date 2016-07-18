using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        /*//Arrange 
        IBinder binder = new Binder();
        //Act
        IBinding binding1 = binder.Bind<object>().ToSelf();
        IBinding binding2 = binder.Bind<object>().To<object>();

        print(binding1.bindingType);

        print(TypeUtils.IsAssignable(typeof(p), typeof(ren2)));*/

        ren1 ren = new ren1() { age = 18 };
        ren.printAge();
    }

    // Update is called once per frame
    void Update () {
	
	}


}

public interface p {  }
public class ren : p { public int age; public string name; }
public class ren1 : ren {  }
public class ren2 : ren {  }

public static class renKZ
{
    public static void printAge(this ren r) { UnityEngine.Debug.Log(r.age); }
}




