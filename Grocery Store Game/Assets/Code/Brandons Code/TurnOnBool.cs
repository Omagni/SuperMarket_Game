using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnBool : MonoBehaviour
{
    Behavior behavior;

    private void Start()
    {
        behavior = GameObject.FindGameObjectWithTag("DumbAss").GetComponent<Behavior>();
        //behavior.changeBool(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            behavior.changeBool(gameObject);
        }
    }


}
