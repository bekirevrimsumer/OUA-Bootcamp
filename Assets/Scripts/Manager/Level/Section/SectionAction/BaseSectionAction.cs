using Photon.Pun;
using UnityEngine;

public class BaseSectionAction : MonoBehaviourPunCallbacks
{
    public bool IsCompleted;

    public virtual void Execute()
    {
        IsCompleted = true;
    }
}
