using UnityEngine;

public class EnemyStateFactory
{
   private EnemyStateMachine context;

   public EnemyStateFactory(EnemyStateMachine currentContext)
   {
      context = currentContext;
   }

   public EnemyBaseState Patrol()
   {
      return new EnemyPatrolState(context, this);
   }
   
   public EnemyBaseState Follow()
   {
      return new EnemyFollowState(context, this);
   }
   
   public EnemyBaseState Attack()
   {
      return new EnemyAttackState(context, this);
   }
   
   public EnemyBaseState Stun()
   {
      return new EnemyStunState(context, this);
   }
   
   public EnemyBaseState Death()
   {
      return new EnemyDeathState(context, this);
   }
}
