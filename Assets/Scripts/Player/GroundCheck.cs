using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	public event Action<bool> OnGroundedChange;
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			OnGroundedChange?.Invoke(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			OnGroundedChange?.Invoke(false);
		}
	}
}
