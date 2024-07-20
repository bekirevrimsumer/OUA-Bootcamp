using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;
using System;
using System.Collections;

public class RoomManager : MultiplayerGenericSingleton<RoomManager>
{
	[SerializeField] private GameObject player;
	[Space]
	[SerializeField] private TMP_Dropdown maxPlayer;
	[Space]
	[SerializeField] private TMP_InputField roomNameText;
	public string roomNameToJoin = "test";

    public override void Awake()
    {
		base.Awake();
        PhotonNetwork.AutomaticallySyncScene = true; 
    }

	public void JoinRoomButtonPressed()
	{
		Debug.Log("Connecting");

		PhotonNetwork.JoinOrCreateRoom(roomNameText.text, new RoomOptions { MaxPlayers = maxPlayer.value + 1 }, null);
	}

	public void OnRoomItemButtonPressed(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		Debug.Log("Connected in a room");

		StartCoroutine(StartGameCoroutine());
	}

	private IEnumerator StartGameCoroutine()
	{
		LoadGameScene();

		yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Game");
		
		SpawnPlayer();
	}

	public void SpawnPlayer()
	{
		Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
		GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
		MultiplayerEvent.Trigger(MultiplayerEventType.JoinGame, _player);
	}

	private void LoadGameScene()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("Game");
		}
		
	}
}
