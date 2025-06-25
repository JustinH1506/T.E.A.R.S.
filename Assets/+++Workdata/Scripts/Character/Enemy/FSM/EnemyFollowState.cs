using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
	
	#region Methods
	
	/// <summary>
	/// ENters state starts animation and starts navMeshAgent.
	/// </summary>
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = false;
		ctx.Anim.CrossFade(EnemyAnimationFactory.Run, 0.1f);
	}

	/// <summary>
	/// Calls CheckSwitchState.
	/// </summary>
	public override void UpdateState()
	{
		CheckSwitchStates();
		
		// if (ctx.DistanceBetweenPlayer() <= ctx.AttackDistance)
		// {
		// 	ctx.transform.RotateAround(ctx.PlayerTransform.position, Vector3.up, 10f * Time.deltaTime);
		// }
	}
	
	/// <summary>
	/// Looks at the Distance between Player and object and changes destination if is ture.
	/// </summary>
	public override void FixedUpdateState()
	{
		if (ctx.DistanceBetweenPlayer() >= ctx.AttackDistance)
		{
			ctx.NavMeshAgent.SetDestination(ctx.PlayerTransform.position);
		}
		// else
		// {
		// 	ctx.transform.RotateAround(ctx.PlayerTransform.position, Vector3.up, 1);
		// }
	}

	public override void ExitState()
	{
		
	}

	/// <summary>
	/// Checks if any state should be active. 
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.DistanceBetweenPlayer() < ctx.AttackDistance && ctx.CanAttack && ctx.hasTarget)
		{
			ctx.NavMeshAgent.isStopped = true;
			SwitchStates(factory.Attack());
		}
		else if(!ctx.hasTarget)
		{
			SwitchStates(factory.Patrol());
		}
		else if (ctx.GotHit)
		{
			SwitchStates(factory.Stun());
		}
	}
	
	#endregion
}
