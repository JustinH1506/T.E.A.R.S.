
public class PlayerStunState : PlayerBaseState
{
	public PlayerStunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	/// <summary>
	/// Plays the Hit Animation. 
	/// </summary>
	public override void EnterState()
	{
		ctx.Anim.Play(PlayerAnimationFactory.HitAnim);
	}

	/// <summary>
	/// Calls the CheckSwitchSate method. 
	/// </summary>
	public override void UpdateState()
	{
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	/// <summary>
	/// Checks if any state should be active.
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
		else if(!ctx.IsAttacking)
		{
			SwitchStates(factory.Idle());
		}
	}
	
	public override void ChangeAttackAnimation()
	{
		
	}
	
	public override void InitializeSubStates(){}
}
