using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
	
	#region Variables
	public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
	
    #endregion
	
    #region Methods
	public override void EnterState()
	{
		ctx.NavMeshAgent.isStopped = true;
		ctx.Anim.Play(EnemyAnimationFactory.Death);
	}

	public override void UpdateState()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
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
