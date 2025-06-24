using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
	
	#region Methods
	
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = false;
		ctx.Anim.CrossFade(EnemyAnimationFactory.Run, 0.1f);
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
		
		// if (ctx.DistanceBetweenPlayer() <= ctx.AttackDistance)
		// {
		// 	ctx.transform.RotateAround(ctx.PlayerTransform.position, Vector3.up, 10f * Time.deltaTime);
		// }
	}
	
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
