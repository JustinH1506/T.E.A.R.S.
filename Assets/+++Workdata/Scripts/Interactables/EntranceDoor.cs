using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntranceDoor : MonoBehaviour, IDataPersistence
{
    public bool playerInReach = false;
 	
    [SerializeField] private GameObject itemBlinkLight;
 	
 	private Animator anim;
    
    private SphereCollider sphereCollider;
    
    private AudioSource audioSource;

    [SerializeField] private GameObject inactiveGameobjects;
    
 	/// <summary>
 	/// Get the animator and sphere collider. 
 	/// </summary>
 	private void Awake()
 	{
 		anim = GetComponent<Animator>();
	    sphereCollider = GetComponent<SphereCollider>();
	    audioSource = GetComponent<AudioSource>();
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
 	/// Deactivates indicator and makes the Player seem in reach. 
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
 	/// When pressing e start a text and activating the next parts to work. 
 	/// </summary>
 	private void Update()
 	{
 		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
	    {
		    playerInReach = false;
		    sphereCollider.enabled = false;
		    sphereCollider.enabled = false;
		    
		    itemBlinkLight.SetActive(false);
 			anim.Play("DoorOpens");
		    
		    AudioManager.Instance.PlaySound(AudioManager.Instance.entranceDoor, audioSource);
		    
		    StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.mainCharacterStartDialogue));
		    
		    UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
 		}
 	}

    public void SaveData(GameData gameData)
    {
	    
    }

    /// <summary>
    /// Loads the data to make the entrance door like it should be after having taken elenors ring. 
    /// </summary>
    /// <param name="gameData"></param>
    public void LoadData(GameData gameData)
    {
	    if (gameData.openedEntranceDoor)
	    {
		    playerInReach = false;
		    sphereCollider.enabled = false;
		    sphereCollider.enabled = false;
		    itemBlinkLight.SetActive(false);
		    inactiveGameobjects.SetActive(true);
	    }
    }
}
