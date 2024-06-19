using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    //Buna baska bir yol uygula

    [SerializeField] private RBMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private MoveObjWithRay moveObjWithRay;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject climbCheck;

    public bool IsLocal { get; private set; } = false;

	public void IsLocalPlayer()
	{
        IsLocal = true;
        playerMovement.enabled = true;
        moveObjWithRay.enabled = true;
        playerLook.enabled = true;
        cam.SetActive(true);
        groundCheck.SetActive(true);
        climbCheck.SetActive(true);
	}
}
