using System.Collections;
using UnityEngine;


/*                      MUST SEE NOTES:
 * Anything you use a IEnumerator function, always use StartCoroutine
 * 
 */


public class Stocker_Script : MonoBehaviour
{
                // Stockers attributes
    [SerializeField] public float speed = 1.0f;
    // How many items the stocker can carry/store/grab at once
    int stockerCarryAmount = 5;
    // The amount of items stocker is currently holding
    public int itemsOnHand = 0;
    public int ActionWaitTime = 2;

    // Animations
    public Animator animator;
    private string currentState;
    const string IDLE = "Stocker_Idle";
    const string WALK = "Stocker_Walk";


                // Checks
    // Used for WalkToTarget (function)
    public bool arrivedAtDestination = false;
    // Used to check if Stocker near either Shelf or Pallet (functions)
    public bool nearShelf = false;
    public bool nearPallet = false;
    // Check if Stocker is currently active (working)
    public bool active = false; 

    // Stockers current target
    public GameObject target;
    public Shelf shelf;
    public Pallet pallet;
    // Probably need an array of all shelfs/pallets later down the line.

    // To calculate if Stocker is near Shelf/Pallet
    float distanceFromTarget = 0.0f;
    float distanceFromShelf = 0.0f;
    float distanceFromPallet = 0.0f;

    // Stock count references
    int palletStock = 0;
    int shelfStock = 0;
    int stockMax = 20;

    // If Stocker is running a Wait/StartCoroutine - Grabbing/Loading Stock.
    bool isWaiting = false;


    private void Start()
    {
        shelf = GameObject.FindGameObjectWithTag("Shelf").GetComponent<Shelf>();
        pallet = GameObject.FindGameObjectWithTag("Pallet").GetComponent<Pallet>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
                //ALWAYS RUNNING FUNCTIONS:

        // If Stocker has a target, walk to it. (Will always walk to target)
        if (target != null)
            WalkToTarget();

        //Always check if near shelf
        NearShelf();
        //Always check if near pallet
        NearPallet();


                // CHECKS:

        // if stocker is waiting (working/loading/unloading) do not try to do anything else.
        if (isWaiting == false)
        { 

        // If Shelf is not at stockMax (20), and is not currently stocking: Start stocking
        if (shelf.stock < stockMax & active == false)
            active = true;
        // If stock is full: do not start stocking
        else if (shelf.stock == stockMax) 
            active = false;

        // If Stocker is stocking, and he has no items: Go to Pallet
        if (active == true & itemsOnHand == 0)
            TargetPallet();

        // If Stocker is stocking and is near a Pallet and has no items on hand: get stock from Pallet
        if (active == true & nearPallet == true & itemsOnHand == 0)
            StartCoroutine( GrabStock(stockerCarryAmount) );

        // If Stocker is stocking, has all he can carry: Return to Shelf
        if (active == true & itemsOnHand == stockerCarryAmount)
            TargetShelf();

        // If stocking is stocking and is near Shelf and has items on hand: add items to the Shelf
        if (active == true & nearShelf == true & itemsOnHand == stockerCarryAmount)
            StartCoroutine( AddStockToShelf(stockerCarryAmount) );

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




    // Remove stock from Pallets (includes wait time)
    public IEnumerator GrabStock(int amount) 
    {
        // if stocker is not waiting: start wait
        if (isWaiting == false)
            isWaiting = true;

        print("Wait started.");

        yield return new WaitForSeconds(2);

        print("Wait ended");

        // increment stockers items on hand
        itemsOnHand += amount;
        // decrement local stock check variable
        palletStock -= amount;
        // call the removestock function on Pallet script to decrement its stock value
        target.GetComponent<Pallet>().RemoveStock(amount);
        isWaiting = false;
    }

    // Adds Stock to Shelf (includes wait time)
    public IEnumerator AddStockToShelf(int amount)
    {
        // if stocker is not waiting: start wait
        if (isWaiting == false)
            isWaiting = true;

        print("Wait started");

        yield return new WaitForSeconds(2);

        print("Wait started");

        // Decrement stockers items on hand
        itemsOnHand -= amount;
        // decrement local stock check variable
        shelfStock += amount;
        // call the removestock function on Pallet script to decrement its stock value
        target.GetComponent<Shelf>().AddStock(amount);
        isWaiting = false;
    }



    // Walk to target. Always active. Should probably disable thing if Stocker is in some idle mode.
    private void WalkToTarget()
    {
        // Return if target met.
        if (ArrivedAtDestination() == true)
        {
            ChangeAnimationState(IDLE);
            return;
        }
        else
        {
            ChangeAnimationState(WALK);
        }

        //walk to target
        Vector3 direction = target.transform.position - transform.position;
        // Keep speed consistent
        direction.Normalize();
        // Move the GameObject in the direction of the target at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Function to determine if stocker has arrived to his destination. Helps control WalkToTarget
    public bool ArrivedAtDestination()
    {
        // Calculate distance from target
        distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

        // if he is 0.5 units away from target, stocker has arrived to destination
        if (distanceFromTarget <= 0.5f)
            arrivedAtDestination = true;
        else
            arrivedAtDestination = false;

        return arrivedAtDestination;
    }

    // Detects if Stocker is near a Shelf
    public void NearShelf()
    {
        distanceFromShelf = Vector3.Distance(transform.position, shelf.transform.position);

        if (distanceFromShelf <= 0.5f)
            nearShelf = true;
        else
            nearShelf = false;
    }

    // Detects if Stocker is near a Pallet
    public void NearPallet()
    {
        distanceFromPallet = Vector3.Distance(transform.position, pallet.transform.position);

        if (distanceFromPallet <= 0.5f)
            nearPallet = true;
        else
            nearPallet = false;
    }

    void ChangeAnimationState(string newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState) 
            return;

        // play the animation
        animator.Play(newState);

        // reassign the current state
        currentState = newState;
    }

}
