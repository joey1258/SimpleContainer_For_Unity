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
        IBinding binding = binder.BindMultiton<int>().To(new object[] { 1, 2, 3, 4, 5, 6 });
        //Act
        binding.RemoveValue(new object[] { 1, 2 });

        print(binding.valueList.Count);
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




