using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	#region Variables
	
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	#endregion
	
	#region My Methods
	
	/// <summary>
	/// Starts the idle animation. 
	/// </summary>
	public override void EnterState()
	{
		ctx.Anim.CrossFade(PlayerAnimationFactory.IdleAnim, 0.1f);
	}

	/// <summary>
	/// Calls the Handle Rotation method. 
	/// </summary>
	public override void UpdateState()
	{
		ctx.HandleRotation(ctx.HandleCameraRelative(), ctx.RotationSpeed);
		CheckSwitchStates();
	}
	
	/// <summary>
	/// Calls teh GetCurrentStamina method. 
	/// </summary>
	public override void FixedUpdateState()
	{
		ctx.GetCurrentStamina();
	}
	
	public override void ExitState(){}

	/// <summary>
	/// Checks if any state should be active.
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
		}
		else if (ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Run());
		}
		else if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
		}
	}
	
	public override void ChangeAttackAnimation()
	{
		
	}
	
	public override void InitializeSubStates(){}
	
	#endregion
}