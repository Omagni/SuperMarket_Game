using UnityEngine;

public class Shelf : MonoBehaviour
{
    // number of items the crate can hold
    [SerializeField] public int stock = 0;
    [SerializeField] private Sprite image_one;
    [SerializeField] private Sprite image_two;
    [SerializeField] private Sprite image_three;
    [SerializeField] private Sprite image_four;

    SpriteRenderer SR;

    private void Start()
    {
        // Get the SpriteRenderer component on the GameObject this script is attached to
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (stock == 0) // 0
            SR.sprite = image_one;
        if (stock > 0 && stock <= 10) // 1-10
            SR.sprite = image_two;
        if (stock > 10 && stock <= 15) // 11 - 15
            SR.sprite = image_three;
        if (stock > 15) // 16 and above
            SR.sprite = image_four;
    }
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
