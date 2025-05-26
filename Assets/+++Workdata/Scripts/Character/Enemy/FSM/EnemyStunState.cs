using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
	public EnemyStunState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
    
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = true;
		ctx.Anim.Play(EnemyAnimationFactory.Hit);
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ctx.GotHit = false;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.GotHit)
		{
			ctx.Anim.Play(EnemyAnimationFactory.Hit);
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
}
