using UnityEngine;
using System;
using System.Collections.Generic;
using uMVVMCS;
using uMVVMCS.DIContainer;

public class NewBehaviourScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        List<string> nums = new List<string>();
        nums.Add(null);
        print(nums[0]);
        nums.Add("1");
        nums.Add("1");
        nums.Add("1");
        nums.Add("1");
        nums.Add("1");
        nums.Add("1");
        print(nums.Count);
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




