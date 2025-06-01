using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
	public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	private float waitCounter;
	private float maxWaitCounter = 2f;
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = true;
		FaceTarget();
		ctx.Anim.Play(EnemyAnimationFactory.Attack);
		waitCounter = maxWaitCounter;
		ctx.AttackCooldown = ctx.MaxAttackCooldown;
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
		waitCounter = maxWaitCounter;
		ctx.NavMeshAgent.isStopped = false;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.GotHit)
		{
			SwitchStates(factory.Stun());
		}
		
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			return;
		}
		
		if (ctx.DistanceBetweenPlayer() < ctx.FollowDistance)
		{
			SwitchStates(factory.Follow());
		}
		else if(ctx.DistanceBetweenPlayer() > ctx.PatrolDistance)
		{
			SwitchStates(factory.Patrol());
		}
	}
	
	void FaceTarget()
	{
		var turnTowardNavSteeringTarget = ctx.PlayerTransform.position;
      
		Vector3 direction = (turnTowardNavSteeringTarget - ctx.transform.position);
		Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
		
		ctx.transform.forward = Vector3.Slerp(ctx.transform.forward, lookDirection, 500 * Time.deltaTime);
	}
}
