using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
	
	#region Variables
	public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
	
    #endregion
	
    #region Methods
    /// <summary>
    /// Enters the death State. 
    /// </summary>
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = true;
		AudioManager.Instance.PlaySound(AudioManager.Instance.enemyFallSound, ctx.soundSource);
		ctx.Anim.CrossFade(EnemyAnimationFactory.Death, 0.01f);
	}

    /// <summary>
    /// disables the ctx script and animator after animation played. 
    /// </summary>
	public override void UpdateState()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			foreach (Rigidbody rb in ctx.rbs)
			{
				rb.isKinematic = false;
			}
			
			ctx.NavMeshAgent.enabled = false; 
			ctx.Anim.enabled = false;
			ctx.enabled = false;
		}
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		
	}
	
	#endregion
}