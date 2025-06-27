
public abstract class PlayerBaseState
{
    #region Variables
    
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    
    #endregion
    
    #region My Methods
    protected PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubStates();

    /// <summary>
    /// Exits the state, enters a new state and set the new state. 
    /// </summary>
    /// <param name="newState"></param>
    protected void SwitchStates(PlayerBaseState newState)
    {
        ExitState();
        
        newState.EnterState();
        
        ctx.CurrentState = newState;
    }

    public abstract void ChangeAttackAnimation();
    
    protected void SetSuperState(){}
    protected void SetSubState(){}
    
    #endregion
}