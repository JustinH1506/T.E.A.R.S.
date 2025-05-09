using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}

	
	public override void EnterState()
	{
		ctx.Anim.CrossFade(PlayerAnimationFactory.DodgeAnim, 0.01f);
		ctx.HandleDodge();
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
		
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return;
		}

		ctx.IsDodging = false;
		
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
	public override void InitializeSubStates(){}
}
