using UnityEngine;

public class Stocker_Script : MonoBehaviour
{

    // To find a crate
    public GameObject crate;
    // stock of current crate found
    int stock = 0;

    // The amount of items stocker is currently holding
    [SerializeField] int items = 0;
    // how many items he can hold at once
    int amount = 5;

    // Update is called once per frame
    void Update()
    {
        
        // If stocker does not have any items, go grab more from crate
        if (items == 0)
        {
            // Find a crate
            crate = GameObject.FindGameObjectWithTag("Crate");

            // get current crates stock count
            stock = crate.GetComponent<Crate_Script>().stock;

            // as long as stock is available, grab.
            if (stock >= amount) 
            {
                RemoveItemsFromCrate(amount);
                GrabStock(amount);
            } // end of stock if
            stock -= amount;
            Debug.Log(string.Format("The stock is {0}", stock));
        } // end of item if
        
    }

    public void GrabStock(int amount) 
    {
        items += amount;
    }

    public void RemoveItemsFromCrate(int amount) 
    {
        // Stocker can grab one item at a time. Can change amount with serializedField amount.
        crate.GetComponent<Crate_Script>().RemoveStock(amount);
    }

}
