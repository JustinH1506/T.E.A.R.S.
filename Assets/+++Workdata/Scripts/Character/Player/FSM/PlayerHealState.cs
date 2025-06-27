using UnityEngine;

public class PlayerHealState : PlayerBaseState
{
	public PlayerHealState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}

	
	public override void EnterState()
	{
		
	}

	/// <summary>
	/// Checks to switch the state.
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

	public override void CheckSwitchStates()
	{
		
	}
	
	public override void ChangeAttackAnimation()
	{
		
	}
	
	public override void InitializeSubStates(){}
}
