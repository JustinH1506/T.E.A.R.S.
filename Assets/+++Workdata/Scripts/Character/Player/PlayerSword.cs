using System;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
	[SerializeField] private PlayerStateMachine _playerActions;

	[SerializeField] private AudioSource soundSource;
	
	/// <summary>
	/// If the Enemy gets hit we call the TakeDamage from it. 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && !other.GetComponent<CharacterBase>().isDead)
		{
			other.GetComponent<CharacterBase>().TakeDamage(_playerActions.baseAttack);
			AudioManager.Instance.PlaySound(AudioManager.Instance.playerHitSound, soundSource);
			other.GetComponent<EnemyStateMachine>().GotHit = true;
		}
	}
}
