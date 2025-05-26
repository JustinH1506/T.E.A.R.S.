using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
	public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
    
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = true;
		ctx.Anim.Play(EnemyAnimationFactory.Death);
	}

	public override void UpdateState()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
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
}
