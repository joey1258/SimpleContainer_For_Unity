using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        someClass sc1 = new someClass() { id = 1 };
        someClass sc2 = new someClass() { id = 2 };
        someClass sc3 = new someClass() { id = 3 };
        someClass sc4 = new someClass() { id = 4 };
        someClass sc5 = new someClass() { id = 5 };
        someClass[] scs = new someClass[] { sc1, sc2, sc3, sc4, sc5 };

        Array.Sort(scs);
        foreach(someClass sc in scs) { print(sc.id); }
        print(scs[1].id);

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

public class someClass : IInjectionFactory, IComparable<someClass>
{
    public int id;
    public object Create(InjectionContext context) { return this; }

    #region IComparable implementation 

    public int CompareTo(someClass other)
    {
        if (other == null) { return 1; }
        else { return -id.CompareTo(other.id); }
    }

    #endregion
}

public class someClass_b : someClass { }

public class someClass_c : someClass { }




