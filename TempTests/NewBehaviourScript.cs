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
        binder.MultipleBind(
            new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
            new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                .To(new List<object>() { typeof(someClass_b), new int[] { 111, 222 }, new someClass() })
                .As(new List<object>() { null, 1, 2 })
                .Bind<someClass>().ToSelf()
                .MultipleBind(
            new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
            new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                .Bind<someClass>().To(new someClass()).As(3)
                .MultipleBind(
            new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
            new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                .To(new List<object>() { typeof(someClass_b), new int[] { 444, 555, 666 }, new someClass() })
                .As(new List<object>() { null, 4, 5 })
                .Bind<someClass>().ToSelf();
        //Act
        int num = binder.GetBinding<someClass>(1).valueArray.Length +
            binder.GetBinding<someClass>(4).valueArray.Length;

        print(num);
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




