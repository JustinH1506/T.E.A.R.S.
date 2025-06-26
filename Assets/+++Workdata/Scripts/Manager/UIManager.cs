using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Variables

	public static UIManager Instance;
	private GameObject player;

	#endregion
	
	#region Constants
	
	public const string master = "Master";
	public const string music = "Music";
	public const string sfx = "SFX";
	
	#endregion
	
	#region CanvasGroups
	[Header("Canvas Groups")]
	public CanvasGroup loadingScreen;
	public CanvasGroup mainMenuScreen;
	public CanvasGroup gameOverScreen;
	public CanvasGroup optionsScreen;
	public CanvasGroup inGameUi;
	public CanvasGroup dialogueUi;
	public CanvasGroup infoTextUi;
	public CanvasGroup pauseScreen;
	public CanvasGroup journalScreen;
	public CanvasGroup itemScreen;
	public CanvasGroup demoEndScreen;
    #endregion
	
    #region Texts
	[Header("Texts")] 
	[SerializeField] private TextMeshProUGUI JournalText;
	[SerializeField] private TextMeshProUGUI itemText;
	[SerializeField] private TextMeshProUGUI infoText;
	#endregion

	#region Buttons

	[Header("Buttons")] 
	
	public Button[] journalButtons;
	public Button[] itemButtons;
	
	[Space]

	#endregion

	#region Images

	[Header("Images")]
	public Image loadingIcon;
	public Image playerHealthUi;
	public Image playerStaminaUi;
	public Image item;
	[Space] 
	
	#endregion
	
	#region Audio
	
	[Header("Audio")] 
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioMixer mixer;
	[SerializeField] private Slider masterSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;
	
	#endregion
	
	#region Methods
	
	/// <summary>
	/// Makes this object to an instance, Adds OnSliderChanged method to the slider for music
	/// </summary>
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
		
		masterSlider.onValueChanged.AddListener(delegate { OnSliderChanged(masterSlider, master);});
		musicSlider.onValueChanged.AddListener(delegate { OnSliderChanged(musicSlider, music);});
		sfxSlider.onValueChanged.AddListener(delegate { OnSliderChanged(sfxSlider, sfx);});
	}

	/// <summary>
	/// Sets the mixers floats to master, music and sfx. 
	/// </summary>
	private void Start()
	{
		mixer.SetFloat(master, Mathf.Log10(masterSlider.value) * 20);
		mixer.SetFloat(music, Mathf.Log10(musicSlider.value) * 20);
		mixer.SetFloat(sfx, Mathf.Log10(sfxSlider.value) * 20);
	}

	/// <summary>
	/// Activates the Pause menu when e or the Journal/item menu when tab was pressed. 
	/// </summary>
	private void Update()
	{
		if (GameManager.Instance.gameStates == GameManager.GameStates.MainMenu)
			return;

		if (Keyboard.current.escapeKey.wasPressedThisFrame && pauseScreen.alpha < 1 && journalScreen.alpha < 1)
		{
			OpenMenu(pauseScreen, CursorLockMode.None, 0f, true);
			AudioManager.Instance.ChangeAllSfxSources(true);
		}
		else if(Keyboard.current.escapeKey.wasPressedThisFrame && pauseScreen.alpha >= 1)
		{
			CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
			AudioManager.Instance.ChangeAllSfxSources(false);
		}

		if (Keyboard.current.tabKey.wasPressedThisFrame && journalScreen.alpha < 1 && itemScreen.alpha < 1 && pauseScreen.alpha < 1)
		{
			OpenMenu(journalScreen, CursorLockMode.None, 0f, true);
		}
		else if (Keyboard.current.tabKey.wasPressedThisFrame)
		{
			CloseMenu(journalScreen, CursorLockMode.Locked, 1f);
			CloseMenu(itemScreen, CursorLockMode.Locked, 1f);
		}
	}

	/// <summary>
	/// Starts a new game. 
	/// </summary>
	public void StartNewGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, false, GameManager.GameStates.InGame));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f, false);
		DataPersistenceManager.Instance.NewGame();
		AudioManager.Instance.mainMenuListener.enabled = false; 
	}

	/// <summary>
	/// Loads the game data and starts from this point onward.
	/// </summary>
	public void LoadGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, true, GameManager.GameStates.InGame));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f, false);
	}

	/// <summary>
	/// Reloads the game. 
	/// </summary>
	public void ReloadGame()
	{
		CloseMenu(gameOverScreen, CursorLockMode.Locked, 1f);
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, (int)SceneLoader.Instance.sceneStates, 1)); 
	}

	/// <summary>
	/// Gets back to the main menu screen. 
	/// </summary>
	public void BackToMainMenu()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Manager;
		StartCoroutine(SceneLoader.Instance.UnloadScene(SceneLoader.Instance.currentScene, (int)SceneLoader.Instance.sceneStates, 1));
		CloseMenu(pauseScreen, CursorLockMode.None, 1f);
		CloseMenu(inGameUi, CursorLockMode.None, 1f);
		CloseMenu(demoEndScreen, CursorLockMode.None, 1f);
		OpenMenu(mainMenuScreen, CursorLockMode.None, 1f, true);
		AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
		AudioManager.Instance.mainMenuListener.enabled = true;
	}

	/// <summary>
	/// Opens th options menu. 
	/// </summary>
	/// <param name="getsOpened"></param>
	public void OpenOptionsMenu(bool getsOpened)
	{
		if (getsOpened)
		{
			OpenMenu(optionsScreen, CursorLockMode.None, 0f, true);
		}
		else
		{
			CloseMenu(optionsScreen, CursorLockMode.None, 0f);
		}
	}

	/// <summary>
	/// Resumes the game and closes the Pause menu. 
	/// </summary>
	public void Resume()
	{
		CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
		AudioManager.Instance.ChangeAllSfxSources(false);
	}

	/// <summary>
	/// Opens a Ui screen menu depending on the given variables.
	/// </summary>
	/// <param name="canvasGroup"></param>
	/// <param name="lockMode"></param>
	/// <param name="timeScale"></param>
	/// <param name="playerDisabled"></param>
	public void OpenMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale, bool playerDisabled)
	{
		canvasGroup.ShowCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null && playerDisabled)
		{
			player.GetComponent<PlayerStateMachine>().DisablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

	/// <summary>
	/// Closes a ui screen depending on the given variables.
	/// </summary>
	/// <param name="canvasGroup"></param>
	/// <param name="lockMode"></param>
	/// <param name="timeScale"></param>
	public void CloseMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale)
	{
		canvasGroup.HideCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			player.GetComponent<PlayerStateMachine>().EnablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

	/// <summary>
	/// Starts the text with given variables. 
	/// </summary>
	/// <param name="currentText"></param>
	/// <returns></returns>
	public IEnumerator StartText(string currentText)
	{
		OpenMenu(infoTextUi, CursorLockMode.Locked, 1f, false);
		infoText.text = currentText;

		float waitTime = 2f;

		while (waitTime > 0f)
		{
			waitTime -= Time.deltaTime;
			
			yield return null;
		}
		
		CloseMenu(infoTextUi, CursorLockMode.Locked, 1f);
	}

	/// <summary>
	/// Changes the journals text. 
	/// </summary>
	/// <param name="journals"></param>
	public void ChangeJournal(Journals journals)
	{
		JournalText.text = journals.journalText;
	}

	public void ChangeItem(Items items)
	{
		item.sprite = items.itemImage;
		itemText.text = items.itemDescription;
	}

	/// <summary>
	/// Calls the save game method. 
	/// </summary>
	public void SaveGame()
	{
		DataPersistenceManager.Instance.SaveGame();
	}
	
	/// <summary>
	/// Sets the mixer value depending on the slider used.
	/// </summary>
	/// <param name="slider"></param>
	/// <param name="keyName"></param>
	private void OnSliderChanged(Slider slider, string keyName)
	{
		PlayerPrefs.SetFloat(keyName, slider.value);
		
		switch (keyName)
		{
			case master:
			case music:
			case sfx:
				mixer.SetFloat(keyName, Mathf.Log10(slider.value) * 20);
				break;
		}
	}

	/// <summary>
	/// Shows or hides the canvas group depending if it has alpha to 1 or 0.
	/// </summary>
	/// <param name="canvasGroup"></param>
	public void ChangeCanvasGroup(CanvasGroup canvasGroup)
	{
		if (canvasGroup.alpha < 1)
		{
			canvasGroup.ShowCanvasGroup();
		}
		else
		{
			canvasGroup.HideCanvasGroup();
		}
	}

	public void PlayButtonSound()
	{
		AudioManager.Instance.PlaySound(AudioManager.Instance.uiPressSound, AudioManager.Instance.soundSource);
	}
	
	/// <summary>
	/// Quits the game. 
	/// </summary>
	public void Quit()
	{
		Application.Quit();
	}
	
	#endregion
}