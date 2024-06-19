using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;

	[SerializeField] private GameObject player;
	[Space]
	[SerializeField] private Transform[] spawnPoints;
	[Space]
	[SerializeField] private GameObject roomCam;

	public string roomNameToJoin = "test";

	private void Awake()
	{
		Instance = this;
	}

	public void JoinRoomButtonPressed()
	{
		Debug.Log("Connecting");

		PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();

		Debug.Log("Connected in a room");

		roomCam.SetActive(false);

		SpawnPlayer();
	}

	public void SpawnPlayer()
	{
		Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

		GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
		_player.GetComponent<PlayerSetup>().IsLocalPlayer();
	}
}
