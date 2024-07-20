using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MirrorItem : Interactable
{
    [HideInInspector]
    public Mirror Mirror;

    private void Start()
    {
        Mirror = transform.GetComponent<Mirror>();
    }

    public override void Interact()
    {
        if (!IsInteracting && Input.GetKeyDown(KeyCode.F))
        {
            if(Mirror.IsInteractable == false) return;
            
            CurrentPlayer.Animator.SetBool("IsCarry", true);
            var _parentId = CurrentPlayer.transform.GetComponent<PhotonView>().ViewID;

            CurrentPlayer.Rb.useGravity = false;
            CurrentPlayer.Rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

		    transform.GetComponent<PhotonView>().RPC("CarryMirrorRPC", RpcTarget.All, _parentId);

            InteractEvent.Trigger(InteractEventType.Interact, "CarryMirrorWindow", false, true, false);
            IsInteracting = true;
        }
        else if (IsInteracting && Input.GetKeyDown(KeyCode.F))
        {
            CurrentPlayer.Animator.SetBool("IsCarry", false);

            CurrentPlayer.Rb.useGravity = true;
            CurrentPlayer.Rb.constraints = RigidbodyConstraints.FreezeRotation;

            transform.GetComponent<PhotonView>().RPC("DropMirrorRPC", RpcTarget.All);

            InteractEvent.Trigger(InteractEventType.InteractEnd, "InteractMirrorWindow", false, true, false);
            IsInteracting = false;
        }

        if (!IsInteracting && Input.GetKey(KeyCode.Q))
        {
            transform.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, -45 * Time.deltaTime);
        }

        if (!IsInteracting && Input.GetKey(KeyCode.E))
        {
            transform.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, 45 * Time.deltaTime);
        }

        if (!IsInteracting && Input.GetKey(KeyCode.X))
        {
            transform.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, -45 * Time.deltaTime);
        }

        if (!IsInteracting && Input.GetKey(KeyCode.C))
        {
            transform.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, 45 * Time.deltaTime);
        }
    }
}
