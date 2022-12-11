using UnityEngine;

public class Stocker_Script : MonoBehaviour
{
    // stockers attributes
    [SerializeField] public float speed = 1.0f;
    int stockerCarryAmount = 5; // How many items the stocker can carry/store/grab at once
    public int itemsCarrying = 0;  // The amount of items stocker is currently holding
    public bool arrivedAtDestination = false;
    public bool nearShelf = false;
    public bool nearPallet = false;
    public bool stocking = false;

    // Used for a target. Can either be used to locate a pallet or a shelf.
    // Probably need an array of all shelfs/pallets later down the line.
    public GameObject target;
    public Shelf shelf;
    public Pallet pallet;

    // distance from target
    float distanceFromTarget;
    float distanceFromShelf;
    float distanceFromPallet;

    // stock count references
    int palletStock = 0;
    int shelfStock = 0;
    int MAX = 20;


    private void Start()
    {
        shelf = GameObject.FindGameObjectWithTag("Shelf").GetComponent<Shelf>();
        pallet = GameObject.FindGameObjectWithTag("Pallet").GetComponent<Pallet>();
    }

    void Update()
    {
        //ALWAYS RUNNING FUNCTIONS:

        // WalkToTarget will only work in update function.
        if (target != null) {
            WalkToTarget();
        }

        //Always check if near shelf
        NearShelf();

        //Always check if near pallet
        NearPallet();

        // CHECKS:
        if (shelf.stock < 20 & stocking == false) {
            stocking = true;
        }
        else if (shelf.stock == 20) { stocking = false; }

        if (stocking == true & itemsCarrying == 0)
        {
            TargetPallet();
            //Walks to pallet
        }

        if (nearPallet == true & stocking == true & itemsCarrying == 0)
        {
            GrabStock(stockerCarryAmount);
        }

        if (stocking == true & itemsCarrying == stockerCarryAmount)
        {
            TargetShelf();
            // he walks to shelf
        }

        if (nearShelf == true & itemsCarrying == stockerCarryAmount & stocking == true)
        {
            AddStock(stockerCarryAmount);
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
        // stock walking to target when destination reached.
        if (arrivedAtDestination == true)
            return;

        //walk to target
        Vector3 direction = target.transform.position - transform.position;

        // normalize the direction to keep walk speed consistent
        direction.Normalize();

        // move the GameObject in the direction of the target at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Function to determine if stock has arrived at destination
    public void ArrivedAtDestination()
    {
        // Calculate distance from target
        distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

        // if he is 0.5 units away from target, stocker has arrived to destination
        if (distanceFromTarget <= 0.5f)
            arrivedAtDestination = true;
        else
            arrivedAtDestination = false;
    }

    // Can interact with shelf if near
    public void NearShelf()
    {
        distanceFromShelf = Vector3.Distance(transform.position, shelf.transform.position);

        if (distanceFromShelf <= 0.5f)
            nearShelf = true;
        else
            nearShelf = false;


    }

    // Can interact with pallet if near
    public void NearPallet()
    {
        distanceFromPallet = Vector3.Distance(transform.position, pallet.transform.position);

        if (distanceFromPallet <= 0.5f)
            nearPallet = true;
        else
            nearPallet = false;
    }

}
