using UnityEngine;
using UnityEngine.InputSystem;

public class ElenorRing : MonoBehaviour, IDataPersistence
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;

	[SerializeField] private GameObject inactiveEnemy;
	[SerializeField] private GameObject activeEnemy;
    
    [SerializeField] private GameObject itemBlinkLight;
    
    [SerializeField] private ControlPanel controlPanel;
    
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private EntranceDoor entranceDoor;
    
    private SphereCollider sphereCollider;

    /// <summary>
    /// Gets the collider.
    /// </summary>
    private void Awake()
    {
	    sphereCollider = GetComponent<SphereCollider>();
    }

    /// <summary>
	/// Activates indicator and makes the Player seem in reach. 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.OpenMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f, false);
			playerInReach = true;
		}
	}

	/// <summary>
	///  Deactivates indicator and makes the Player seem in reach. 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			playerInReach = false;
		}
	}
	
	/// <summary>
	///  When pressing e start a text and activating the next parts to work. 
	/// </summary>
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			playerInReach = false;
			sphereCollider.enabled = false;
			
			activeEnemy.SetActive(true);
			inactiveEnemy.SetActive(false);

			AudioManager.Instance.makeControlPanelActive = true;
			
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			
			doorAnim.Play("CloseDoors");

			GameManager.Instance.isClosed = true;
			GameManager.Instance.inactiveCharactersActive = true;
			
			GameManager.Instance.ActivateItem(1);
			
			audioSource.Play();
			
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.ringSubtitleData));
		}

		if (AudioManager.Instance.controlPanelActive)
		{
			enabled = false;
			
			controlPanel.enabled = true;
			controlPanel.isActive = true;
			
			itemBlinkLight.SetActive(false);
			controlPanel.itemBlinkLight.SetActive(true);
			
			AudioManager.Instance.controlPanelActive = false;
		}
	}

	public void SaveData(GameData gameData)
	{
		
	}
	
	public void LoadData(GameData gameData)
	{
		if (gameData.activeControlPanel)
		{
			enabled = false;
			
			controlPanel.enabled = true;
			controlPanel.isActive = true;
			
			itemBlinkLight.SetActive(false);
			controlPanel.itemBlinkLight.SetActive(true);
		}
	}
}