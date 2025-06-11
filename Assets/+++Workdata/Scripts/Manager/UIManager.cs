using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	private GameObject player;
	
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
	public CanvasGroup pauseScreen;
	public CanvasGroup journalScreen;
	public CanvasGroup demoEndScreen;
    #endregion
	
    #region Texts
	[Header("Texts")] 
	[SerializeField] private TextMeshProUGUI JournalText;
	#endregion

	#region Buttons

	[Header("Buttons")] 
	
	public Button[] journalButtons;
	
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
		if (Input.GetKeyDown(KeyCode.Escape) && pauseScreen.alpha < 1)
		{
			OpenMenu(pauseScreen, CursorLockMode.None, 0f);
			AudioManager.Instance.ChangeAllSources(true);
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && pauseScreen.alpha >= 1)
		{
			CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
			AudioManager.Instance.ChangeAllSources(false);
		}

		if (Input.GetKeyDown(KeyCode.Tab) && journalScreen.alpha < 1)
		{
			OpenMenu(journalScreen, CursorLockMode.None, 0f);
		}
		else if (Input.GetKeyDown(KeyCode.Tab) && journalScreen.alpha >= 1)
		{
			CloseMenu(journalScreen, CursorLockMode.Locked, 1f);
		}
	}

	public void StartNewGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, false));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
		DataPersistenceManager.Instance.NewGame();
	}

	public void LoadGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, true));
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
		AudioManager.Instance.dialogueSource.UnPause();
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

	public void ChangeJournal(Journals journals)
	{
		JournalText.text = journals.journalText;
	}

	public void SaveGame()
	{
		DataPersistenceManager.Instance.SaveGame();
	}

	public void Quit()
	{
		Application.Quit();
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
	
	#endregion
}