using System.Collections;
using UnityEngine;

/*                      MUST SEE NOTES:
 * Anything you use a IEnumerator function, always use StartCoroutine
 * 
 * Shelves: Can either have two arrays: GameObject[] and Shelf[]
 * This makes it that I can simply reference each shelf: ex. (shelves[0].stock)
 * Or I can have one array of GameObjects. Reference each by (shelfObjects[0].GetComponent<Shelf>().stock)
 * To save space and optimize, i should use the second version.
 */

public class Stocker_Script : MonoBehaviour
{
    // STOCKER ATTRIBUTES
    [SerializeField] public float walkSpeed = 1.0f;
    int carryAmount = 5; // How many items the stocker can carry/store/grab at once
    public int itemsOnHand = 0; // The amount of items stocker is currently holding
    public int actionWaitTime = 2; // how long it takes to complete an action

    // ANIMATIONS
    public Animator animator;
    private string currentState;
    const string IDLE = "Stocker_Idle";
    const string WALK = "Stocker_Walk";
    const string IS_STOCKING = "Stocker_Stocking";

    // CHECKS
    public bool nearTarget = false; // check for near target
    public bool nearShelf = false; // check for near shelf
    public bool nearPallet = false; // check for near pallet
    public bool active = false; // check if Stocker is currently active (working)
    bool isWaiting = false; // if Stocker is compelting an action

    // TARGETS
    public GameObject target; // Stockers current target
    public Pallet pallet; // ceference to current pallet

    // ARRAYS
    GameObject[] shelvez; // used for storing game objects of any type into an array

    // INDEXES
    private int shelfIndex = 0;
    private int palletIndex = 0;

    // DESTINATION CALCULATIONS
    float distanceFromTarget = 0.0f;
    float distanceFromShelf = 0.0f;
    float distanceFromPallet = 0.0f;

    // Stock count references
    const int stockMax = 20;

    private void Start()
    {
        pallet = GameObject.FindGameObjectWithTag("Pallet").GetComponent<Pallet>();
        shelvez = GameObject.FindGameObjectsWithTag("Shelf"); // Find game objects of type shelf

        animator = GetComponent<Animator>();
    }

    void Update()
    {

        //reference: 
        // shelvez[shelfIndex].GetComponent<Shelf>().(use function or variable here)

        //ALWAYS RUNNING FUNCTIONS:
        // If Stocker has a target, walk to it. (Will always walk to target)
        if (target != null)
            WalkToTarget();
        //Always check if near a shelf
        NearShelf();
        //Always check if near a pallet
        NearPallet();

        // LOGIC:
        // logic for changing shelves. Simple at the moment. If one shelf reaches max stock, go to next.
        if (shelvez[shelfIndex].GetComponent<Shelf>().stock == stockMax && shelfIndex < shelvez.Length - 1)
            NextShelf();

        // if stocker is waiting (working/loading/unloading) do not try to do anything else.
        if (isWaiting == false)
        {
            // if stocker finds a shelf that is not at full capacity, start working
            if (shelvez[shelfIndex].GetComponent<Shelf>().stock < stockMax & active == false)
                active = true;
            // if stock is full: do not start stocking
            else if (shelvez[shelfIndex].GetComponent<Shelf>().stock == stockMax)
                active = false;

            // if Stocker is stocking, and he has no items: Go to Pallet
            if (active == true & itemsOnHand == 0)
                TargetPallet();

            // if Stocker is stocking and is near a Pallet and has no items on hand: get stock from Pallet
            if (active == true & nearPallet == true & itemsOnHand == 0)
                StartCoroutine(GrabStock(carryAmount));

            // if Stocker is stocking, has all he can carry: Return to Shelf
            if (active == true & itemsOnHand == carryAmount) {
                TargetShelf();
            }

            // if stocking is stocking and is near Shelf and has items on hand: add items to the Shelf
            if (active == true & nearShelf == true & itemsOnHand == carryAmount)
                StartCoroutine(AddStockToShelf(carryAmount));
        } // end of isWaiting

    }

    // Target Shelf
    private void TargetShelf() 
    { target = shelvez[shelfIndex]; }

    // Target next shelf
    private void NextShelf()
    {
        shelfIndex += 1;
        TargetShelf();
    }

    // Target Pallet
    private void TargetPallet()
    {target = GameObject.FindGameObjectWithTag("Pallet");}

    // Remove stock from Pallets (includes wait time)
    public IEnumerator GrabStock(int amount)
    {
        // if stocker is not waiting: start wait
        if (isWaiting == false)
            isWaiting = true;

        yield return new WaitForSeconds(actionWaitTime); // action wait time
        itemsOnHand += amount; // increment stockers items on hand
        pallet.RemoveStock(amount); // decrement current Pallets stock count
        isWaiting = false;
    }

    // Adds Stock to Shelf (includes wait time)
    public IEnumerator AddStockToShelf(int amount)
    {
        // if stocker is not waiting: start wait
        if (isWaiting == false)
            isWaiting = true;

        yield return new WaitForSeconds(actionWaitTime); // action wait time
        itemsOnHand -= amount; // Decrement stockers items on hand
        shelvez[shelfIndex].GetComponent<Shelf>().AddStock(amount); // decrement current Shelfs stock count
        isWaiting = false;
    }

    // Walk to target. Always active.
    private void WalkToTarget()
    {
        // Cancel function call if Stocker is on target
        if (ArrivedAtDestination() == true)
        {
            ChangeAnimationState(IDLE);
            return;
        }
        else
            ChangeAnimationState(WALK);

        Vector3 direction = target.transform.position - transform.position; // walk to target
        direction.Normalize(); // keep speed consistent
        transform.Translate(direction * walkSpeed * Time.deltaTime); // move to target
    }

    // Function to determine if stocker has arrived to his destination. Helps control WalkToTarget
    public bool ArrivedAtDestination()
    {
        // Calculate distance from target
        distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

        // if he is 0.5 units away from target, stocker has arrived to destination
        if (distanceFromTarget <= 0.5f)
            nearTarget = true;
        else
            nearTarget = false;

        return nearTarget; // return bool check
    }

    // Detects if Stocker is near a Shelf - ALWAYS RUNNING
    public void NearShelf()
    {
        distanceFromShelf = Vector3.Distance(transform.position, shelvez[shelfIndex].GetComponent<Shelf>().transform.position);

        // if he is 0.5 units away from a Shelf, stocker has arrived to destination
        if (distanceFromShelf <= 0.5f)
            nearShelf = true;
        else
            nearShelf = false;
    }

    // Detects if Stocker is near a Pallet - ALWAYS RUNNING
    public void NearPallet()
    {
        distanceFromPallet = Vector3.Distance(transform.position, pallet.transform.position);

        // if he is 0.5 units away from a Pallet, stocker has arrived to destination
        if (distanceFromPallet <= 0.5f)
            nearPallet = true;
        else
            nearPallet = false;
    }

    // used to change current animation
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
