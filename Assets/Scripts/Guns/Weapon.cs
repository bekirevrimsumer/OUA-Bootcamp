using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public event Action<int, int> OnWeaponAmmoChange;

	[SerializeField] private Camera cam;
    [SerializeField] private int damage = 20;
    [SerializeField] private float fireRate = 0.3f;

	[Header("VFX")]
	[SerializeField] private GameObject AKHitEffect;

	[Header("Ammo")]
	[SerializeField] private int magAmmo = 30;
	[SerializeField] private int totalAmmo = 150;

	private int currentAmmo;

    private float currentFireTime;

	private void Start()
	{
		currentAmmo = magAmmo;
	}

	private void Update()
	{
		if(Input.GetButton("Fire1") && currentFireTime <= 0f)
		{
			if (currentAmmo <= 0)
				return;

			currentFireTime = 1 / fireRate;

			GunFire();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reload();
		}

		currentFireTime -= Time.deltaTime;
	}

	void GunFire()
	{
		currentAmmo--;

		OnWeaponAmmoChange?.Invoke(currentAmmo, totalAmmo);

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
		{
			PhotonNetwork.Instantiate(AKHitEffect.name, hit.point, Quaternion.identity);

			if (hit.transform.GetComponent<PlayerHealth>())
			{
				hit.transform.GetComponent<PhotonView>().RPC("DecreaseHealth", RpcTarget.All, damage);
			}
		}
	}

	private void Reload()
	{
		if (totalAmmo < 0 || currentAmmo >= magAmmo)
			return;

		int neededAmmo = totalAmmo >= magAmmo ? magAmmo - currentAmmo : totalAmmo;
		totalAmmo -= neededAmmo;
		currentAmmo += neededAmmo;

		OnWeaponAmmoChange?.Invoke(currentAmmo, totalAmmo);
	}

}
