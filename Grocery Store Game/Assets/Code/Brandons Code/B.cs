using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{

    A a;


    // Start is called before the first frame update
    void Start()
    {
        print("In the B");
        a = GameObject.FindGameObjectWithTag("TagA").GetComponent<A>();
        a.MethodA(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
