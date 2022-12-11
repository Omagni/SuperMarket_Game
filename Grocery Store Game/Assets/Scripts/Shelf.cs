using UnityEngine;

public class Shelf : MonoBehaviour
{
    // number of items the crate can hold
    [SerializeField] public int stock = 0;

    public void AddStock(int amount)
    {
        stock += amount;
        print("Stock added to Shelf");
    }

    public void RemoveStock(int amount)
    {
        stock -= amount;
        print("Stock removed from Shelf");
    }
}
