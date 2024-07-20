using Photon.Pun;
using UnityEngine;

public class BaseSectionAction : MonoBehaviourPunCallbacks
{
    public Section Section;
    public bool IsCompleted;

    public virtual void Execute()
    {
        IsCompleted = true;
    }
}
