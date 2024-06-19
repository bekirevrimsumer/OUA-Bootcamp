using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
	[SerializeField] private TextMeshProUGUI healthText;

	private void Start()
	{
		health.OnHealthDecrease += PlayerHealth_OnHealthDecrease;
	}

	private void PlayerHealth_OnHealthDecrease(int healthAmount)
	{
		healthText.text = healthAmount.ToString();
	}
}
