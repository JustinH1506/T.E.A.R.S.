using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	/// <summary>
	/// Starts animation depending on if we are locked or not. 
	/// </summary>
	public override void EnterState( )
	{
		if (ctx.targetLock.isTargeting)
		{
			ctx.Anim.CrossFade(PlayerAnimationFactory.LockedRunMovement, 0.1f);
		}
		else
		{
			ctx.Anim.CrossFade(PlayerAnimationFactory.RunAnim, 0.1f);
		}
	}

	public override void UpdateState()
	{
		
	}
	
	/// <summary>
	/// Looks for the movement, uses stamina when in this state, and checks to switch states. 
	/// </summary>
	public override void FixedUpdateState()
	{
		ctx.HandleMovement();
		ctx.Stamina -= Time.deltaTime * ctx.RunCost;
		CheckSwitchStates();
	}
	
	/// <summary>
	/// Exits the state. 
	/// </summary>
	public override void ExitState()
	{
		
	}

	/// <summary>
	/// Checks if any state should be active. 
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
			ctx.Rb.linearVelocity = Vector3.zero;
		}
		else if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
			
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
		}
		else if (ctx.Stamina <= 0 && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
		}
		else if (ctx.Stamina <= 0 && !ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
	}
	
	/// <summary>
	/// Changes attack animation.
	/// </summary>
	public override void ChangeAttackAnimation()
	{
		
	}
	
	/// <summary>
	/// Setted substates. 
	/// </summary>
	public override void InitializeSubStates(){}
}