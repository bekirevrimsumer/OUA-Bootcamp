using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Mirror : MonoBehaviourPun, IPunObservable
{
	public bool IsCarry = false;
	private Quaternion _networkRotation;

	[PunRPC]
	private void CarryMirrorRPC(int parentId)
	{
		IsCarry = true;
		transform.parent = PhotonView.Find(parentId).gameObject.FindInChildren("InteractObjectTransform").transform;

		Vector3 targetPosition = new Vector3(0.1f, -0.05f, 0.03f);
		Vector3 targetRotation = new Vector3(-90, 0, 90);

		transform.DOLocalMove(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
		transform.DOLocalRotate(targetRotation, 0.5f).SetEase(Ease.InOutQuad);
	}

	[PunRPC]
	private void DropMirrorRPC()
	{
		IsCarry = false;
		transform.parent = null;
	}

	[PunRPC]
    private void RotateMirrorRPC(Vector3 axis, float angle)
    {
        transform.Rotate(axis, angle, Space.Self);
    }

    public void RotateMirror(Vector3 axis, float angle)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RotateMirrorRPC", RpcTarget.All, axis, angle);
        }
    }

    void Update()
    {
        if (!photonView.IsMine && transform.parent == null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _networkRotation, Time.deltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            _networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
