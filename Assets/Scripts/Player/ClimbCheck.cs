using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{
	public event Action<bool> OnClimbChange;
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Climb"))
		{
			OnClimbChange?.Invoke(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Climb"))
		{
			OnClimbChange?.Invoke(false);
		}
	}
}
