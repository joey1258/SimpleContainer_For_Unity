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
        binder
            .Bind<object>()
            .To(1)
            .Bind<object>()
            .To(2)
            .As(2)
            .Bind<int>()
            .To(1)
            .Bind<int>()
            .To(2)
            .Bind<float>()
            .To(1f)
            .As(1)
            .Bind<float>()
            .To(2f);
        //Act
        //binder.UnbindNullIdBindingByType<object>();
        binder.UnbindNullIdBindingByType<int>();
        //binder.UnbindNullIdBindingByType<float>();

        //print(binder.GetAllBindings().Count);
        //print(binder.GetBindingsByType<object>().Count);
        print(binder.GetBindingsByType<int>().Count);
        //print(binder.GetBindingsByType<float>().Count);
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

public class someClass : IInjectionFactory
{
    public int id;
    public object Create(InjectionContext context) { return this; }
}

public class someClass_b : someClass { }

public class someClass_c : someClass { }




