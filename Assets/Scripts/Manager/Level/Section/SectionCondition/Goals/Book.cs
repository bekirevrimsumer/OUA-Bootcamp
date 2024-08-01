using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public enum BookColor
{
    Black, 
    Yellow,
    Green,
    Blue,
    Purple
}

public class Book : MonoBehaviourPunCallbacks
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

    [PunRPC]
    private void PlaceBook(float x, float y, float z)
    {
            DOTween.To(() => FirstTransform.position, x => FirstTransform.position = x, new Vector3(x, y, z), 0.2f);
            transform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
    }
    
}
