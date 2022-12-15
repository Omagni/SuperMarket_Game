using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer rendy;
    [SerializeField] private GameObject highlight;

    public void Init(bool isOffset)
    {
        rendy.color = isOffset ? offsetColor : baseColor;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);   
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

}
