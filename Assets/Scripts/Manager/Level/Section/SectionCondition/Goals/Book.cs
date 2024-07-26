using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Book : MonoBehaviour
{
    public string Symbol;
    public int Code;
    public ShelfInteractable ShelfInteractable;
    public bool isPickedUp = false;
    public Transform FirstTransform;

    public QuickOutline outline;


    private void Start()
    {
        outline = GetComponent<QuickOutline>();
        outline.enabled = false;
        FirstTransform = transform;
    }

    
}
