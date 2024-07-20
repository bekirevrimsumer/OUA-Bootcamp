using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;

    public void OnRoomItemButtonPressed()
	{
		RoomManager.Instance.OnRoomItemButtonPressed(roomNameText.text);
	}
}
