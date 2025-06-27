using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	/// <summary>
	/// Enters the Attack state and plays the animation. 
	/// </summary>
	public override void EnterState()
	{
		ctx.CanTurn = false;
		ctx.Anim.Play(PlayerAnimationFactory.AttackAnim01);
		ctx.AttackMovement(12);
		AudioManager.Instance.PlaySound(AudioManager.Instance.swordAttackSounds[Random.Range(0,2)], ctx.soundSource);
	}

	/// <summary>
	/// Rotates depending on input and Calls CheckSwitchState. 
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
	/// Exits the State and resets some varaibles. 
	/// </summary>
	public override void ExitState()
	{
		ctx.CanTurn = true;
		ctx.AttackAmount = 0;
	}

	/// <summary>
	/// Checks if any state should be active. 
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
	
	/// <summary>
	/// Changes Attack animation depending on AttackAmount. 
	/// </summary>
	public override void ChangeAttackAnimation()
	{
		if (ctx.AttackAmount > 1)
		{
			SwitchStates(factory.AttackSecond());
		}
	}
	
	public override void InitializeSubStates(){}
}