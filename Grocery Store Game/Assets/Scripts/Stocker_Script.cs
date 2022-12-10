using UnityEngine;

public class Stocker_Script : MonoBehaviour
{

    // stockers attributes
    public float speed = 1.0f;
    // distance from target
    float distance;

    // Used for a target. Can either be used to locate a pallet or a shelf.
    public GameObject target;
    // have to make stocker remember the shelf that he has to return to after
    // he grabs stock from the pallet (Probably need to run a function when he targets pallet)

    // reference to pallets current stock count
    int stock = 0;

    // how many items he can hold at once
    int amount = 5;

    // The amount of items stocker is currently holding
    public int items = 0;
    

    void Update()
    {
        
        // If stocker does not have any items, go grab more from crate
        if (items == 0)
        {
            // locate a pallet
            target = GameObject.FindGameObjectWithTag("Pallet");

            // walk toward the target
            WalkToTarget();

            // get current pallets stock count
            stock = target.GetComponent<Pallet>().stock;

            // if he is close enough to the pallet
            if (distance <= 0.5f)
            {
                // as long as stock is available, grab.
                if (stock >= amount)
                    GrabStock(amount);
            }

            Debug.Log(string.Format("The stock is {0}", stock));
        } // end of item if
        
    }


    public void GrabStock(int amount) 
    {
        items += amount;
        stock -= amount; // decrement local stock check variable
        target.GetComponent<Pallet>().RemoveStock(amount);
    }


    // stocker walks toward target.
    private void WalkToTarget()
    {
        // Calculate the distance to the target
        distance = Vector3.Distance(transform.position, target.transform.position);

        //walk to target
        Vector3 direction = target.transform.position - transform.position;

        // normalize the direction to keep walk speed consistent
        direction.Normalize();

        // move the GameObject in the direction of the target at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);

        if (distance <= 0.5f)
            return;
    }

}
