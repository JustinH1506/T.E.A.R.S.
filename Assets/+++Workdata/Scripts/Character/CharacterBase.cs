using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	public event System.Action<int> OnHealthChanged;

	[Header("Base Variables")]
	[SerializeField] protected int baseMaxHealth;
	[SerializeField] protected int baseCurrentHealth;
	[SerializeField] protected int baseDefense;
	public int baseAttack;
	[Space]
	
	public bool isDead = false;
	
	public bool IsDead { get => isDead;
		set => isDead = value;
	}

	public int CurrentHealth
	{
		get => baseCurrentHealth;
	
		set => SetCurrentHealth(value);
	}
	
	/// <summary>
	/// Gets called when base current health gets changed. Changes health and looks if they die. 
	/// </summary>
	/// <param name="newHealth"></param>
	private void SetCurrentHealth(int newHealth)
	{
		if (newHealth > baseMaxHealth)
			newHealth = baseMaxHealth;
	
		baseCurrentHealth = newHealth;
	
		if (OnHealthChanged != null)
		{
			OnHealthChanged(baseCurrentHealth);
		}
		
		if (baseCurrentHealth <= 0)
		{
			if (gameObject.CompareTag("Player"))
			{
				IsDead = true;
				UIManager.Instance.OpenMenu(UIManager.Instance.gameOverScreen, CursorLockMode.None, 0f, true);
				baseCurrentHealth = 1;
			}
			else
			{
				baseCurrentHealth = 1;
				IsDead = true;
				GetComponent<CapsuleCollider>().enabled = false;
				GameManager.Instance.killedEnemies++;
				GameManager.Instance.CheckKey();
			}
		}
	}
	
	/// <summary>
	/// calls SetCurrentHealth to initiate health. 
	/// </summary>
	protected virtual void Awake()
	{
		SetCurrentHealth(baseMaxHealth);
	}

	/// <summary>
	/// Reduces health by the damage amount. 
	/// </summary>
	/// <param name="damage"></param>
	public void TakeDamage(int damage)
	{
		CurrentHealth -= (damage - baseDefense);
	}
	
	/// <summary>
	/// Subscribes callback to onhealthchanged. 
	/// </summary>
	/// <param name="callback"></param>
	/// <param name="getInstantCallback"></param>
	public void OnRegisterCurrentHealth(System.Action<int> callback, bool getInstantCallback = false)
	{
		OnHealthChanged += callback;
		if (getInstantCallback)
			callback(baseCurrentHealth);
	}
	
}
