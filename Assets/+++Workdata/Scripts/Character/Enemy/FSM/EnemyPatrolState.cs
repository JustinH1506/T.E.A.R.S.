using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    
    public EnemyPatrolState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

    public override void EnterState()
    {
        ctx.NavMeshAgent.isStopped = false;
        
        ctx.NavMeshAgent.updatePosition = false;

        ctx.Anim.Play(EnemyAnimationFactory.Walk);

        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
        
        ctx.StartCoroutine(ctx.DetectPlayer());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    
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

    private void NextPoint()
    {
        if (ctx.CheckPoints.Length == 0)
            return;
        
        ctx.CurrentPoint = (ctx.CurrentPoint + 1) % ctx.CheckPoints.Length;
        
        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
    }

    private void OnAnimatorMove()
    {
        
    }
}

