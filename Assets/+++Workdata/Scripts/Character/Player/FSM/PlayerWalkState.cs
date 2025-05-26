using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
	#region Variables
	
	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	#endregion
	
	#region My Methods
	public override void EnterState()
	{
		if (ctx.targetLock.isTargeting)
		{
			ctx.Anim.CrossFade(PlayerAnimationFactory.LockedWalkMovement, 0.1f);
		}
		else
		{
			ctx.Anim.CrossFade(PlayerAnimationFactory.WalkAnim, 0.1f);
		}
	}

	public override void UpdateState()
	{
		
	}
	
	public override void FixedUpdateState()
	{
		ctx.GetCurrentStamina();
		ctx.HandleMovement();
		CheckSwitchStates();
	}

	public override void ExitState()
	{
		ctx.Anim.StopPlayback();
	}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
			ctx.Rb.linearVelocity = Vector3.zero;
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
		else if (ctx.IsMoving && ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
	}
	
	public override void ChangeAttackAnimation()
	{
		
	}
	
	public override void InitializeSubStates(){}
	
	#endregion
}