using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    
    public EnemyPatrolState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

    /// <summary>
    /// Enters the state and changes states to walk again in case its necessary. 
    /// </summary>
    public override void EnterState()
    {
        ctx.NavMeshAgent.isStopped = false;
        
        ctx.NavMeshAgent.updatePosition = false;

        ctx.Anim.Play(EnemyAnimationFactory.Walk);

        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
        
        ctx.StartCoroutine(ctx.DetectPlayer());
    }

    /// <summary>
    /// Calls UpdateState. 
    /// </summary>
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    
    /// <summary>
    /// Calls NextPoint if distance to the current point is smaller than 0.5f.
    /// </summary>
    public override void FixedUpdateState()
    {
        if (ctx.NavMeshAgent.remainingDistance <= 0.5f)
        {
            NextPoint();
        }
    }

    public override void ExitState()
    {
        
    }

    /// <summary>
    /// Checks if any state should be active. 
    /// </summary>
    public override void CheckSwitchStates()
    {
        if (ctx.DistanceBetweenPlayer() < ctx.FollowDistance && ctx.hasTarget)
        {
            SwitchStates(factory.Follow());
        }
        else if(ctx.GotHit)
        {
            SwitchStates(factory.Stun());
        }
    }

    /// <summary>
    /// Sets teh new point the object should walk to. 
    /// </summary>
    private void NextPoint()
    {
        if (ctx.CheckPoints.Length == 0)
            return;
        
        ctx.CurrentPoint = (ctx.CurrentPoint + 1) % ctx.CheckPoints.Length;
        
        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
    }

  
}

