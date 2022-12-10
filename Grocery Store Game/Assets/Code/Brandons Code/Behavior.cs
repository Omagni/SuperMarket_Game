using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{

    //The character will activate in the case of a boolean switch
    public bool onSwitch = false;

    void Update()
    {

    }

    public void changeBool(GameObject x)
    {
        onSwitch = true;

        if (onSwitch)
        {
            print("Gamer mode activated");
        }
    }




}
