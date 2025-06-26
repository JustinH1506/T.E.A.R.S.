using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	/// <summary>
	/// Enters the state and Plays aniamtion. 
	/// </summary>
	public override void EnterState()
	{
		if (ctx.targetLock.isTargeting)
		{
			ctx.Anim.Play(PlayerAnimationFactory.LockedDodgeMovement);
		}
		else
		{
			ctx.Anim.Play(PlayerAnimationFactory.DodgeAnim);
		}
		ctx.HandleDodge();
	}

	/// <summary>
	/// Calls CheckSwitchState.
	/// </summary>
	public override void UpdateState()
	{
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}

	/// <summary>
	/// Sets the is dodgin to false.
	/// </summary>
	public override void ExitState()
	{
		ctx.IsDodging = false;
		ctx.dodgeCounter = 0;
	}

	/// <summary>
	/// Checks if any state should be active. 
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return;
		}
		
		if (ctx.IsMoving && ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
		else if (ctx.IsMoving && !ctx.IsSprinting)
		{
			SwitchStates(factory.Walk());
		}
		else
		{
			SwitchStates(factory.Idle());
		}
	}

	public override void ChangeAttackAnimation()
	{
		
	}
	public override void InitializeSubStates(){}
}