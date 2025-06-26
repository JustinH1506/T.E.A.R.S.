using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ElenoirRing : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;

	[SerializeField] private GameObject inactiveEnemy;
	[SerializeField] private GameObject activeEnemy;
	
	[SerializeField] private PlayableDirector ringCutscene;
	
	[SerializeField] private CinemachineVirtualCamera cam;
    
    [SerializeField] private GameObject itemBlinkLight;
    
    [SerializeField] private ControlPanel controlPanel;
    
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
			sphereCollider.enabled = false;
			itemBlinkLight.SetActive(false);
			playerInReach = false;
			controlPanel.enabled = true;
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.ringSubtitleData, cam));
			GameManager.Instance.ActivateItem(1);
			doorAnim.Play("CloseDoors");
			inactiveEnemy.SetActive(false);
			activeEnemy.SetActive(true);
		}
	}
}
