using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RoomList : MonoBehaviourPunCallbacks
{
	public static RoomList Instance;

	[SerializeField] private RoomManager roomManager;
	[SerializeField] private GameObject roomManagerGO;

	public event Action<List<RoomInfo>> OnRoomUpdate;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

	private void Awake()
	{
		Instance = this;
	}

	IEnumerator Start()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.Disconnect();
		}

		yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();

		PhotonNetwork.JoinLobby();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		if(cachedRoomList.Count <= 0)
		{
			cachedRoomList = roomList;
		}
		else
		{
			foreach (var room in roomList)
			{
				for (int i = 0; i < cachedRoomList.Count; i++)
				{
					if(cachedRoomList[i].Name == room.Name)
					{
						List<RoomInfo> newRoomList = cachedRoomList;

						if (room.RemovedFromList)
						{
							newRoomList.Remove(newRoomList[i]);
						}
						else
						{
							newRoomList[i] = room;
						}

						cachedRoomList = newRoomList;
					}
				}
			}
		}

		OnRoomUpdate?.Invoke(cachedRoomList);
	}

	public void JoinRoomByName(string roomName)
	{
		roomManager.roomNameToJoin = roomName;
		roomManagerGO.SetActive(true);
		gameObject.SetActive(false);
	}

	public void ChangeRoomToCreateRoom(string _roomName)
	{
		roomManager.roomNameToJoin = _roomName;
	}
}
