using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
	[SerializeField] private Transform[] doors;
	private void OnTriggerEnter(Collider other)
	{
		foreach (var door in doors)
		{
			door.GetComponent<DoorMovement>().SetKeyPressed(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		foreach (var door in doors)
		{
			door.GetComponent<DoorMovement>().SetKeyPressed(false);
		}
	}
}
