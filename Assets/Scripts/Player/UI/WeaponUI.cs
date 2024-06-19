using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;

	private void Start()
	{
		weapon.OnWeaponAmmoChange += Weapon_OnWeaponAmmoChange;
	}

	private void Weapon_OnWeaponAmmoChange(int currentAmmo, int totalAmmo)
	{
		currentAmmoText.text = currentAmmo.ToString();
		totalAmmoText.text = totalAmmo.ToString();
	}
}
