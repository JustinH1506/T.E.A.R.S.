using System;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
	[SerializeField] private PlayerStateMachine _playerActions;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && !other.GetComponent<CharacterBase>().isDead)
		{
			other.GetComponent<CharacterBase>().TakeDamage(_playerActions.baseAttack);
			other.GetComponent<EnemyStateMachine>().GotHit = true;
		}
	}
}
