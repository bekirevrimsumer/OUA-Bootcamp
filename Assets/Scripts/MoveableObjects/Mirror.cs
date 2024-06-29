using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Mirror : MonoBehaviour
{
	[PunRPC]
	private void CarryMirrorRPC(int parentId)
	{
		transform.parent = PhotonView.Find(parentId).gameObject.FindInChildren("InteractObjectTransform").transform;

		Vector3 targetPosition = new Vector3(0.1f, -0.05f, 0.03f);
		Vector3 targetRotation = new Vector3(-90, 0, 90);

		transform.DOLocalMove(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
		transform.DOLocalRotate(targetRotation, 0.5f).SetEase(Ease.InOutQuad);
	}

	[PunRPC]
	private void DropMirrorRPC()
	{
		transform.parent = null;
	}
}
