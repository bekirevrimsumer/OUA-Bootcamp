using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveObjWithRay : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private Transform moveableObjPosition;
	[SerializeField] private LayerMask moveableObjLayer;

	private Transform moveableObj = null;
	private bool hasObj = false;
	private int parentId;

	RaycastHit hit;

	private void Update()
	{
		/*if (!GetComponent<PhotonView>().IsMine)
			return;*/

		if(Input.GetKeyDown(KeyCode.E))
		{
			if (!hasObj && MoveableObjDetect())
			{
				TakeMoveableObj();
			}
			else if(hasObj)
			{
				ReleaseMoveableObj();
			}
		}
	}

	private bool MoveableObjDetect()
	{
		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 10f, moveableObjLayer))
		{
			moveableObj = hit.transform;
			return true;
		}

		return false;
	}

	private void TakeMoveableObj()
	{
		hasObj = true;
		parentId = GetComponent<PhotonView>().ViewID;
		moveableObj.GetComponent<PhotonView>().RPC("SetMoveableObj", RpcTarget.All, parentId);
	}

	private void ReleaseMoveableObj()
	{
		hasObj = false;
		moveableObj.GetComponent<PhotonView>().RPC("ReleaseMoveableObj", RpcTarget.All);
	}
}
