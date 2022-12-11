using UnityEngine;

public class Stocker_Script : MonoBehaviour
{

    // stockers attributes
    [SerializeField] public float speed = 1.0f;
    int stockerCarryAmount = 5; // How many items the stocker can carry/store/grab at once
    public int itemsCarrying = 0;  // The amount of items stocker is currently holding

    // Used for a target. Can either be used to locate a pallet or a shelf.
    // Probably need an array of all shelfs/pallets later down the line.
    public GameObject target;
    public GameObject shelf; // Used for remembering which shelf to return to.

    // distance from target
    float distanceFromTarget;

    // stock count references
    int palletStock = 0;
    int shelfStock = 0;


    private void Start()
    {
        // Target shelf
        TargetShelf();
    }

    void Update()
    {

        // WalkToTarget will only work in update function.
        WalkToTarget();

        // INSIDE UPDATE: *

        // Check Shelf stock
        // If stock is < MAX
        // initiate stock process


        // POSSIBLY OUTSIDE UPDATE: *
        // STOCK PROCESS

        // target pallet

        // walk to pallet

        // grab stock from pallet

        // Target shelf

        // Walk to shelf

        // fill stock

        // return and repeat.


    }

    //Function to fill stock? - BROKEN ATM
    private void StockingProcess()
    {

        // Only when close enough to shelf
        if (distanceFromTarget <= 0.5f)
        {
            // If stock on shelf is less than max:
            if (CheckShelfStock() < 20)
            {

                // If stocker does not have any items on hand:
                if (itemsCarrying == 0)
                {
                    // Target Pallet
                    TargetPallet();
                    // walk toward the target (pallet)
                    WalkToTarget();

                    // if he is close enough to the pallet
                    if (distanceFromTarget <= 0.5f)
                    {
                        // get current pallets stock count
                        CheckPalletStock();

                        // if available stock, grab
                        if (CheckPalletStock() >= stockerCarryAmount)
                            GrabStock(stockerCarryAmount);
                    }

                    // Target Shelf
                    TargetShelf();
                    // Walk to target
                    WalkToTarget();
                    // Add stock to shelf
                    AddStock(stockerCarryAmount);

                    Debug.Log(string.Format("The stock is {0}", palletStock));
                }


            }


        }
    }


    // Target Shelf
    private void TargetShelf()
    {
        target = GameObject.FindGameObjectWithTag("Shelf");
    }

    // Target Pallet
    private void TargetPallet() 
    {
        target = GameObject.FindGameObjectWithTag("Pallet");
    }

    // Check Shelf stock count
    private int CheckShelfStock()
    {
        return target.GetComponent<Shelf>().stock;
    }

    // Check Pallet stock count
    private int CheckPalletStock()
    {
        return target.GetComponent<Pallet>().stock;
    }

    // NEED TO CHECK IF I AM CLOSE ENOUGH TO SHELVES OR PALLETS TO RUN THE FUNCTIONS. RUN A CHECK DISTANCE IN EACH FUNCTION.

    // Remove stock from Pallets
    public void GrabStock(int amount) 
    {
        // increment stockers items on hand
        itemsCarrying += amount;
        // decrement local stock check variable
        palletStock -= amount; 
        // call the removestock function on Pallet script to decrement its stock value
        target.GetComponent<Pallet>().RemoveStock(amount); 
    }

    // Adds stock to Shelves
    public void AddStock(int amount)
    {
        // increment stockers items on hand
        itemsCarrying -= amount;
        // decrement local stock check variable
        shelfStock += amount;
        // call the removestock function on Pallet script to decrement its stock value
        target.GetComponent<Shelf>().AddStock(amount);
    }


    // Simply walks to target. Pretty much always active. Will probably cause conflicts.
    private void WalkToTarget()
    {

        // Calculate distance from target
        distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

        // return (do not run / stop running) if on target
        if (distanceFromTarget <= 0.5f)
            return;

        //walk to target
        Vector3 direction = target.transform.position - transform.position;

        // normalize the direction to keep walk speed consistent
        direction.Normalize();

        // move the GameObject in the direction of the target at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);

    }

}
