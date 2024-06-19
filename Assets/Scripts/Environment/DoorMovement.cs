using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DoorMovement : MonoBehaviour
{
	[Header("Move Logic")]
	[SerializeField] private Transform[] walkPoints;
	[SerializeField] private float moveSpeed = 4f;
	private int positionIndex = 0;
	//private int positionIndexSetter = -1;
	private bool isKeyPressed = false;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//Bir sonraki yürüme noktasina varip varmadigini kontrol ediyor
		if (isKeyPressed)
		{
			positionIndex = 1;
		}
		else
		{
			positionIndex = 0;
		}

		MoveToNextPoint();
	}

	//Bir sonraki yürüme noktasina ilerleme
	private void MoveToNextPoint()
	{
		if (Vector3.Distance(transform.position, walkPoints[positionIndex].position) <= 0.2f)
			return;

		Vector3 dir = walkPoints[positionIndex].position - transform.position;
		rb.MovePosition(transform.position + dir.normalized * moveSpeed * Time.fixedDeltaTime);
	}

	public void SetKeyPressed(bool value)
	{
		isKeyPressed = value;
	}
}
