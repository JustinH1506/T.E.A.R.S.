using UnityEngine;

public class PlayerStateFactory
{
    #region Variables
    
    private PlayerStateMachine context;

    #endregion
    
    #region My Methods
    
    /// <summary>
    /// Returns every State depeneding on which is needed. 
    /// </summary>
    /// <param name="currentContext"></param>
    
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }
    
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }
    
    public PlayerBaseState Run()
    {
        return new PlayerRunState(context, this);
    }
    
    public PlayerBaseState Attack()
    {
        return new PlayerAttackState(context, this);
    }
    
    public PlayerBaseState AttackSecond()
    {
        return new PlayerAttackSecondState(context, this);
    }
    
    public PlayerBaseState AttackThird()
    {
        return new PlayerAttackThirdState(context, this);
    }

    public PlayerBaseState Dodge()
    {
        return new PlayerDodgeState(context, this);
    }

    public PlayerBaseState Stun()
    {
        return new PlayerStunState(context, this);
    }
    
    #endregion
}