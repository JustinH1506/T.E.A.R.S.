using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
	#region Variables

	public EnemyStunState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	#endregion
    
	#region Methods
	
	/// <summary>
	/// ENters the state.
	/// </summary>
	public override void EnterState()
	{
		ctx.Anim.Play(EnemyAnimationFactory.Hit);
		ctx.NavMeshAgent.isStopped = true;
	}
	
	/// <summary>
	/// Calls CheckSwitchState
	/// </summary>
	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void FixedUpdateState()
	{
		
	}

	/// <summary>
	/// Exits State. 
	/// </summary>
	public override void ExitState()
	{
		ctx.GotHit = false;
	}

	/// <summary>
	/// Checks if any state should be active. 
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.GotHit)
		{
			ctx.Anim.CrossFade(EnemyAnimationFactory.Hit, 0.1f);
			ctx.GotHit = false;
		}
		
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			return;
		}

		if (ctx.IsDead)
		{
			SwitchStates(factory.Death());
		}
		else if (Vector3.Distance(ctx.transform.position, ctx.PlayerTransform.position) < ctx.FollowDistance)
		{
			SwitchStates(factory.Follow());
		}
		else if(ctx.NavMeshAgent.remainingDistance > 5)
		{
			SwitchStates(factory.Patrol());
		}
	}
	#endregion
}