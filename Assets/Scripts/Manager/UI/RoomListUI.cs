using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomListUI : MonoBehaviour
{
	[SerializeField] private RoomList roomList;

    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;

	private void Start()
	{
		roomList.OnRoomUpdate += RoomList_OnRoomUpdate;
	}

	private void RoomList_OnRoomUpdate(List<Photon.Realtime.RoomInfo> _roomList)
	{
		foreach (Transform roomItem in roomListParent)
		{
			Destroy(roomItem.gameObject);
		}

		foreach (var room in _roomList)
		{
			GameObject roomItemGO = Instantiate(roomListItemPrefab, roomListParent);

			roomItemGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
			roomItemGO.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/" +  room.MaxPlayers;
		}
	}
}
