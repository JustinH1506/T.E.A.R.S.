using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : CharacterBase, IDataPersistence
{
	#region States
	
	private EnemyBaseState currentState;
	private EnemyStateFactory states;
	
	public bool hasTarget;
	
	public EnemyBaseState CurrentState { get => currentState;
		set => currentState = value;
	}
	
	#endregion
	
	#region Variables

	[SerializeField] private string uniqueGuid;
	
	private NavMeshAgent navMeshAgent;
	
	private Transform playerTransform;

	private Animator anim;
	
	[SerializeField] private float followDistance = 0f;
	[SerializeField] private float attackDistance = 0f;
	[SerializeField] private float patrolDistance = 0f;

	[SerializeField] private Transform[] checkPoints;
	
	[SerializeField] private float angleViewField = 0;
	
	[SerializeField] private float maxAttackCooldown = 0;
	[SerializeField] private float attackCooldown = 0;
	
	[SerializeField] private SphereCollider hitBox;
	
	[SerializeField] private LayerMask layerCovers = 0;

	public Rigidbody[] rbs;
	
	private int currentPoint = 0;
	
	private bool gotHit = false;
	
	private bool canAttack = true;

	private Data data;
	
	#endregion
	
	#region Getters and Setters
	
	public NavMeshAgent NavMeshAgent { get => navMeshAgent;
		set => navMeshAgent = value;
	}
	
	public Transform PlayerTransform { get => playerTransform;
		set => playerTransform = value;
	}
	
	public Animator Anim => anim;

	public Transform[] CheckPoints => checkPoints;

	public int CurrentPoint { get => currentPoint;
		set => currentPoint = value;
	}

	public float FollowDistance => followDistance;
	public float AttackDistance => attackDistance;
	public float PatrolDistance => patrolDistance;
	public float MaxAttackCooldown => maxAttackCooldown;
	public float AttackCooldown
	{
		get => attackCooldown; 
		set => attackCooldown = value;
	}

	public bool GotHit { get => gotHit;
		set => gotHit = value;
	}

	public bool CanAttack
	{
		get => canAttack;
		set => canAttack = value;
	}
	
	public SphereCollider HitBox => hitBox;
	
	
	
	#endregion

	#region Data Class
	
	[System.Serializable]
	public class Data
	{
		public Vector3 position;
		public int currentPatrolPoint;
		public bool isDead;
	}
	
	#endregion
	
	#region Methods
	
	/// <summary>
	/// Gets all variables from the object. 
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
		data = new Data();
		
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
		
		states = new EnemyStateFactory(this);
		currentState = states.Patrol();
		currentState.EnterState();
		
		rbs = GetComponentsInChildren<Rigidbody>();
	}

	//Calls UpdateState.
	private void Update()
	{
		//HandleAttackCooldown();
		currentState.UpdateState();
	}

	/// <summary>
	/// Calls FixedUpdateState.
	/// </summary>
	private void FixedUpdate()
	{
		currentState.FixedUpdateState();
	}
	
	/// <summary>
	/// Changes the uniqueGuid when the prefab gets set in the scene.
	/// </summary>
	private void OnValidate()
	{
		if (string.IsNullOrEmpty(gameObject.scene.name))
		{
			uniqueGuid = "";
		}
		else if (string.IsNullOrEmpty(uniqueGuid))
		{
			uniqueGuid = System.Guid.NewGuid().ToString();
		}
	}

	/// <summary>
	/// Calculates the distance between object and player object.
	/// </summary>
	/// <returns></returns>
	public float DistanceBetweenPlayer()
	{
		return Vector3.Distance(transform.position, PlayerTransform.position);
	}

	/// <summary>
	/// Saves data needed for the enemy.
	/// </summary>
	/// <param name="gameData"></param>
	public void SaveData(GameData gameData)
	{
		if (gameData.enemyPositionByGuid.ContainsKey(uniqueGuid))
		{
			gameData.enemyPositionByGuid.Remove(uniqueGuid);
		}
		
		data.position = transform.position;
		data.currentPatrolPoint = CurrentPoint;
		data.isDead = isDead;
		
		gameData.enemyPositionByGuid.Add(uniqueGuid, data);
	}

	/// <summary>
	/// Loads saved data needed for the enemy. 
	/// </summary>
	/// <param name="gameData"></param>
	public void LoadData(GameData gameData)
	{
		if (gameData.enemyPositionByGuid.TryGetValue(uniqueGuid, out data))
		{
			data = gameData.GetEnemyPosition(uniqueGuid);
			
			if (data.isDead)
			{
				currentState = states.Death();
				return;
			}
			
			transform.position = data.position;
			
			CurrentPoint = data.currentPatrolPoint;
			
			NavMeshAgent.destination = CheckPoints[CurrentPoint].position;
		}
	}
	
	/// <summary>
	/// Looks for the Player in a certain degree in front of the player.
	/// </summary>
	/// <returns></returns>
	public IEnumerator DetectPlayer()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);

			Vector3 direction = PlayerTransform.transform.position - transform.position;
			float distance = DistanceBetweenPlayer();
			float targetAngle = Vector3.Angle(transform.forward, direction);
			
			bool isNotSeen = targetAngle < angleViewField && IsCharacterCovered(direction, distance);
			
			if (isNotSeen)
			{
				hasTarget = true;
			}
			else if(distance > followDistance)
			{
				hasTarget = false;
			}
		}
	}
	
	/// <summary>
	/// Start a Raycast and if it hits we return true.
	/// </summary>
	bool IsCharacterCovered(Vector3 targetDirection, float distanceToTarget)
	{
		RaycastHit[] hits = new RaycastHit[2];

		Ray ray = new Ray(transform.position, targetDirection);

		int amountOffHits = Physics.RaycastNonAlloc(ray, hits, distanceToTarget, layerCovers);

		if (amountOffHits > 0)
		{
			return true;
		}

		return false;
	}
	
	/// <summary>
	/// Syncs the navmesh to the root motion animation. 
	/// </summary>
	private void OnAnimatorMove()
	{
		Vector3 rootPosition = Anim.rootPosition;
		rootPosition.y = NavMeshAgent.nextPosition.y;
		transform.position = rootPosition;
		NavMeshAgent.nextPosition = rootPosition;
	}
	
	#endregion
}