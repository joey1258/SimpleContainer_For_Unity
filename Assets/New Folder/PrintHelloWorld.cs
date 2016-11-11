using UnityEngine;
using SimpleContainer;

public class PrintHelloWorld : MonoBehaviour {
[Inject("B")]
public Hello hello_A;
	// Use this for initialization
	void Start () {
	hello_A.HelloWorld ();
	}
	
	// Update is called once per frame
	void Update () { }
}
