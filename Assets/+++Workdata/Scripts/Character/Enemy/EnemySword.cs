using System;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
	[SerializeField] private EnemyStateMachine _enemyActions;

	[SerializeField] private AudioSource soundSource;
	
	/// <summary>
	/// If the Player gets hit we call the TakeDamage from it. 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !other.GetComponent<PlayerStateMachine>().IsDodging)
		{
			AudioManager.Instance.PlaySound(AudioManager.Instance.enemyHitSound, soundSource);
			other.GetComponent<CharacterBase>().TakeDamage(_enemyActions.baseAttack); 
		}
	}
}
