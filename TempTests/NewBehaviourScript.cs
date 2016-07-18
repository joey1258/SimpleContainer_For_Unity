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
        binder.Bind<someClass>()
            .ToSelf()
            .Bind<int>()
            .To(1)
            .BindSingleton<someClass>()
            .To(new someClass() { id = 10086 })
            .As("A")
            .BindFactory<someClass_b>()
            .To(new someClass() { id = 110 })
            .As("b");

        IList<IBinding> bindings = binder.GetAllBindings();
        print(bindings.Count);
        foreach(IBinding binding in bindings)
        {
            print(binding.value);
        }
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




