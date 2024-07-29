using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public bool IsInteracting;
    [HideInInspector]
    public PlayerController CurrentPlayer;

    public virtual void Interact()
    {
    }
}
