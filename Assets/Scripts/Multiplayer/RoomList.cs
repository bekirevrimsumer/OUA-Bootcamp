using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RoomList : MultiplayerGenericSingleton<RoomList>
{
	[SerializeField] private RoomManager roomManager;
	[SerializeField] private GameObject roomManagerGO;

	public event Action<List<RoomInfo>> OnRoomUpdate;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

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
    foreach (var room in roomList)
    {
        int index = cachedRoomList.FindIndex(r => r.Name == room.Name);

        if (index != -1)
        {
            if (room.RemovedFromList)
            {
                cachedRoomList.RemoveAt(index);
            }
            else
            {
                cachedRoomList[index] = room;
            }
        }
        else
        {
            if (!room.RemovedFromList)
            {
                cachedRoomList.Add(room);
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
