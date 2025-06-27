using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public enum GameStates
    {
        MainMenu,
        InGame,
        InBattle
    }
    
    [Header("Game Variables")]
    public static GameManager Instance;

    public int killedEnemies = 0;
    
    public GameStates gameStates;

    public bool[] journalStates;
    public bool[] itemStates;

    public bool hasControlRoomKey = false;
    public bool defeated2ndWave = false;
    public bool hasControlPanelKey = false;
    public bool inactiveCharactersActive = false;
    public bool isClosed = false;
    
    /// <summary>
    /// This is for editor only and makes building impossible without the preprocessor.
    /// </summary>
#if  UNITY_EDITOR
    [FormerlySerializedAs("_debug")]
    [Space]
    [Header("Debug variables.")]
    [SerializeField] public DebugAsset debugAsset;
#endif
    
    /// <summary>
    /// Makes this script to an instance.
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
    }

    /// <summary>
    /// Either opens the main menu or the InGame Menu depending on which debug settings are active. 
    /// </summary>
    private void Start()
    {
#if UNITY_EDITOR

        if (debugAsset.useEditorCode)
        {
            string activeSceneName = debugAsset.startScene.name;
            
            if (activeSceneName != String.Empty)
            {
                if (debugAsset.loadGame)
                {
                    DataPersistenceManager.Instance.LoadGame();
                }
                else
                {
                    DataPersistenceManager.Instance.NewGame();
                }
                
                SceneManager.LoadScene(activeSceneName, LoadSceneMode.Additive);
                
                UIManager.Instance.OpenMenu(UIManager.Instance.inGameUi, CursorLockMode.Locked, 1f, false);
            }
        }
        else
        {
            UIManager.Instance.OpenMenu(UIManager.Instance.mainMenuScreen, CursorLockMode.None, 1f, true);
        }
#endif
        
        UIManager.Instance.OpenMenu(UIManager.Instance.mainMenuScreen, CursorLockMode.None, 1f, true);
    }

    /// <summary>
    /// Checks how many enemies where defeated and makes either the key true or starts the dialogue. 
    /// </summary>
    public void CheckKey()
    {
        if (killedEnemies == 2)
        {
            StartCoroutine(UIManager.Instance.StartText("You got an office key!"));
            ActivateItem(0);
            hasControlRoomKey = true;
        }
        else if (killedEnemies == 4)
        {
            defeated2ndWave = true;
            ActivateItem(2);
            StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.afterDefeatingSecondWave));
        }
    }

    /// <summary>
    /// Activates the journal ui depending on which journal was found. 
    /// </summary>
    /// <param name="journalIndex"></param>
    public void ActivateJournal(int journalIndex)
    {
        journalStates[journalIndex] = true;
        
        UIManager.Instance.journalButtons[journalIndex].interactable = true;
        UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f, true);
        UIManager.Instance.journalButtons[journalIndex].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        UIManager.Instance.journalButtons[journalIndex].onClick.Invoke();
    }
    
    public void ActivateItem(int itemIndex)
    {
        itemStates[itemIndex] = true;
        
        UIManager.Instance.itemButtons[itemIndex].interactable = true;
        UIManager.Instance.itemButtons[itemIndex].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        UIManager.Instance.itemButtons[itemIndex].onClick.Invoke();
    }

    /// <summary>
    /// Loads data needed for this script. 
    /// </summary>
    /// <param name="gameData"></param>
    public void LoadData(GameData gameData)
    {
        journalStates = gameData.activeJournals;

        for (int i = 0; i < journalStates.Length; i++)
        {
            if (journalStates[i])
            {
                ActivateJournal(i);
            }
        }
        
        itemStates = gameData.activeItems;

        for (int i = 0; i < itemStates.Length; i++)
        {
            if (itemStates[i])
            {
                ActivateItem(i);
            }
        }
        
        UIManager.Instance.CloseMenu(UIManager.Instance.journalScreen, CursorLockMode.Locked, 1f);
        
        hasControlRoomKey = gameData.hasControlRoomKey;
        defeated2ndWave = gameData.defeated2ndWave;
        hasControlPanelKey = gameData.hasControlPanelKey;
        killedEnemies = gameData.enemiesDefeated;
        inactiveCharactersActive = gameData.inactiveCharactersActive;
        isClosed = gameData.openedEntranceDoor;
    }

    /// <summary>
    /// Saves data from this script. 
    /// </summary>
    /// <param name="gameData"></param>
    public void SaveData(GameData gameData)
    {
        gameData.activeJournals = journalStates;
        gameData.activeItems = itemStates;
        gameData.hasControlPanelKey = hasControlPanelKey;
        gameData.defeated2ndWave = defeated2ndWave;
        gameData.hasControlRoomKey = hasControlRoomKey;
        gameData.enemiesDefeated = killedEnemies; 
        gameData.inactiveCharactersActive = inactiveCharactersActive;
        gameData.openedEntranceDoor = isClosed;
    }
}
