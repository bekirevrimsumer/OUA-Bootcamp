using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public event Action<int> OnHealthDecrease;

	[SerializeField] private PlayerSetup playerSetup;

    [SerializeField] private int health = 100;

	[PunRPC]
    public void DecreaseHealth(int damage)
	{
		health -= damage;
		OnHealthDecrease?.Invoke(health);

		if(health <= 0)
		{
			if(playerSetup.IsLocal)
				RoomManager.Instance.SpawnPlayer();
			/*if (GetComponent<PhotonView>().IsMine)
				RoomManager.Instance.SpawnPlayer();*/

			Destroy(gameObject);
		}
	}
}
