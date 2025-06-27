using UnityEngine;

public class PlayerAttackThirdState : PlayerBaseState
{
	public PlayerAttackThirdState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	/// <summary>
	/// Enters the state and starts attack animation. 
	/// </summary>
	public override void EnterState()
	{
		ctx.CanTurn = false;
		ctx.Anim.Play(PlayerAnimationFactory.AttackAnim03);
		ctx.AttackMovement(12);
		AudioManager.Instance.PlaySound(AudioManager.Instance.swordAttackSounds[Random.Range(0,2)], ctx.soundSource);
	}

	/// <summary>
	/// Handles the rotation and calls CheckswitchState
	/// </summary>
	public override void UpdateState()
	{
		ctx.HandleRotation(ctx.HandleCameraRelative(), 500f);
		
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}

	/// <summary>
	/// Exits the state and resets values. 
	/// </summary>
	public override void ExitState()
	{
		ctx.CanTurn = true;
		ctx.AttackAmount = 0;
	}

	/// <summary>
	/// Switches to new state when conditions are met.
	/// </summary>
	public override void CheckSwitchStates()
	{
		if (ctx.IsDodging && ctx.isCancelingDodge)
		{
			SwitchStates(factory.Dodge());
			ctx.IsAttacking = false;
			ctx.isCancelingDodge = false;
		}
		
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
		{
			return;
		}
		
		ctx.IsAttacking = false;

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
