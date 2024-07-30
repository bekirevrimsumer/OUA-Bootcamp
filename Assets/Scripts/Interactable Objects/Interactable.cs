using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public bool IsInteracting;
    [HideInInspector]
    public PlayerController CurrentPlayer;

    public virtual void Interact()
    {
    }
}
