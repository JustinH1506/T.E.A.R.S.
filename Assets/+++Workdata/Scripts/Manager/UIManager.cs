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

	private void Start()
	{
		mixer.SetFloat(master, Mathf.Log10(masterSlider.value) * 20);
		mixer.SetFloat(music, Mathf.Log10(musicSlider.value) * 20);
		mixer.SetFloat(sfx, Mathf.Log10(sfxSlider.value) * 20);
	}

	private void Update()
	{
		if (GameManager.Instance.gameStates == GameManager.GameStates.MainMenu)
			return;

		if (Keyboard.current.escapeKey.wasPressedThisFrame && pauseScreen.alpha < 1)
		{
			OpenMenu(pauseScreen, CursorLockMode.None, 0f);
			AudioManager.Instance.ChangeAllSfxSources(true);
		}
		else if(Keyboard.current.escapeKey.wasPressedThisFrame && pauseScreen.alpha >= 1)
		{
			CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
			AudioManager.Instance.ChangeAllSfxSources(false);
		}

		if (Keyboard.current.tabKey.wasPressedThisFrame && journalScreen.alpha < 1 && itemScreen.alpha < 1)
		{
			OpenMenu(journalScreen, CursorLockMode.None, 0f);
		}
		else if (Keyboard.current.tabKey.wasPressedThisFrame)
		{
			CloseMenu(journalScreen, CursorLockMode.Locked, 1f);
			CloseMenu(itemScreen, CursorLockMode.Locked, 1f);
		}
	}

	public void StartNewGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, false, GameManager.GameStates.InGame));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
		DataPersistenceManager.Instance.NewGame();
		AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameMusic);
		AudioManager.Instance.mainMenuListener.enabled = false;
	}

	public void LoadGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, true, GameManager.GameStates.InGame));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
	}

	public void ReloadGame()
	{
		CloseMenu(gameOverScreen, CursorLockMode.Locked, 1f);
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, (int)SceneLoader.Instance.sceneStates, 1)); 
	}

	public void BackToMainMenu()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Manager;
		StartCoroutine(SceneLoader.Instance.UnloadScene(SceneLoader.Instance.currentScene, (int)SceneLoader.Instance.sceneStates, 1));
		CloseMenu(pauseScreen, CursorLockMode.None, 1f);
		CloseMenu(inGameUi, CursorLockMode.None, 1f);
		CloseMenu(demoEndScreen, CursorLockMode.None, 1f);
		OpenMenu(mainMenuScreen, CursorLockMode.None, 1f);
		AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
		AudioManager.Instance.mainMenuListener.enabled = true;
	}

	public void OpenOptionsMenu(bool getsOpened)
	{
		if (getsOpened)
		{
			OpenMenu(optionsScreen, CursorLockMode.None, 0f);
		}
		else
		{
			CloseMenu(optionsScreen, CursorLockMode.None, 0f);
		}
	}

	public void Resume()
	{
		CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
		AudioManager.Instance.ChangeAllSfxSources(false);
	}

	public void OpenMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale)
	{
		canvasGroup.ShowCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			player.GetComponent<PlayerStateMachine>().DisablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

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

	public IEnumerator StartText(string currentText)
	{
		OpenMenu(infoTextUi, CursorLockMode.Locked, 1f);
		infoText.text = currentText;
		
		yield return new WaitForSeconds(2f);
		
		CloseMenu(infoTextUi, CursorLockMode.Locked, 1f);
	}

	public void ChangeJournal(Journals journals)
	{
		JournalText.text = journals.journalText;
	}

	public void SaveGame()
	{
		DataPersistenceManager.Instance.SaveGame();
	}
	
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
	
	public void Quit()
	{
		Application.Quit();
	}
	
	#endregion
}