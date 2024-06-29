using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBox : MonoBehaviour
{
    [PunRPC]
    private void SetMoveableObj(int parentId)
	{
		transform.parent = PhotonView.Find(parentId).transform.Find("InteractObjectTransform").transform;
		transform.GetComponent<Rigidbody>().isKinematic = true;
		transform.GetComponent<Collider>().enabled = false;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	[PunRPC]
	private void ReleaseMoveableObj()
	{
		transform.parent = null;
		transform.GetComponent<Rigidbody>().isKinematic = false;
		transform.GetComponent<Collider>().enabled = true;
	}
}
