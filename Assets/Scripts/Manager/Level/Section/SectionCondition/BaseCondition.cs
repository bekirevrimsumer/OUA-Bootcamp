using Photon.Pun;
using UnityEngine;

public class BaseCondition : MonoBehaviourPunCallbacks
{
    public virtual bool IsCompleted() { return false; }
}
