using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum BookColor
{
    Black, 
    Yellow,
    Green,
    Blue,
    Purple
}

public class Book : MonoBehaviour
{
    public BookColor bookColor;
    public ShelfInteractable ShelfInteractable;
    [HideInInspector]
    public bool isPickedUp = false;
    [HideInInspector]
    public Transform FirstTransform;
    [HideInInspector]
    public BookSlot? BookSlot;

    public QuickOutline outline;


    private void Start()
    {
        outline = GetComponent<QuickOutline>();
        outline.enabled = false;
        FirstTransform = transform;
    }

    
}
