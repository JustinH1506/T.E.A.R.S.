using UnityEngine;


public class PlayerInputManager : MonoBehaviour
{
	#region Variables

	private PlayerInput playerInput;
	private PlayerStateMachine playerStateMachine;
	private TargetLock targetLock;
	
	#endregion

	#region Unity Methods
	
	/// <summary>
	/// Creates PlayerInput.
	/// Gets PlayerStateMachine. 
	/// </summary>
	private void Awake()
	{
		playerInput = new PlayerInput();
		playerStateMachine = GetComponent<PlayerStateMachine>();
		targetLock = GetComponent<TargetLock>();
	}

	/// <summary>
	/// Enables all Inputs for the Player.
	/// </summary>
	private void OnEnable()
	{
		playerInput.Enable();

		playerInput.Player.Movement.performed += playerStateMachine.Move;
		playerInput.Player.Movement.canceled += playerStateMachine.Move;

		playerInput.Player.Sprint.performed += playerStateMachine.Sprint;
		playerInput.Player.Sprint.canceled += playerStateMachine.Sprint;

		playerInput.Player.Attack.performed += playerStateMachine.Attack;
		
		playerInput.Player.Dodge.performed += playerStateMachine.Dodge;

		playerInput.TargetLock.Lock.performed += targetLock.AssignTarget;
	}

	/// <summary>
	/// Disables all Inputs for the Player.
	/// </summary>
	private void OnDisable()
	{
		playerInput.Disable();
		
		playerInput.Player.Movement.performed -= playerStateMachine.Move;
		playerInput.Player.Movement.canceled -= playerStateMachine.Move;
		
		playerInput.Player.Sprint.performed -= playerStateMachine.Sprint;
		playerInput.Player.Sprint.canceled -= playerStateMachine.Sprint;
		
		playerInput.Player.Attack.performed -= playerStateMachine.Attack;
		
		playerInput.Player.Dodge.performed -= playerStateMachine.Dodge;
		
		playerInput.TargetLock.Lock.performed -= targetLock.AssignTarget;

	}
	
	#endregion
}
