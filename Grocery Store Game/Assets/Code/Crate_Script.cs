using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Script : MonoBehaviour
{
    // number of items the crate can hold
    [SerializeField] public int stock = 20;

    public void RemoveStock(int amount)
    {
        stock -= amount;
        print("Item removed from stock");
    }

}
