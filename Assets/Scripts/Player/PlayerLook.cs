using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.05f;

    float xRotation;
    float yRotation;

	private void Start()
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	private void Update()
	{
        MyInput();

        //cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //cam.transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        //orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        orientation.Rotate(Vector3.up * yRotation);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void MyInput()
	{
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation = mouseX * sensX * multiplier * Time.deltaTime;
        xRotation -= mouseY * sensY * multiplier * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
