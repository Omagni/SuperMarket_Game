using UnityEngine;

public class Pallet : MonoBehaviour
{
    // number of items the crate can hold
    [SerializeField] public int stock = 20;

    public void AddStock(int amount)
    {
        stock += amount;
        print("Item added to stock");
    }

    public void RemoveStock(int amount)
    {
        stock -= amount;
        print("Item removed from stock");
    }

}
