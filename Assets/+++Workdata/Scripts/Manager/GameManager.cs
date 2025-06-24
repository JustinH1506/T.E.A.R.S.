using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    [FormerlySerializedAs("hasKey")] public bool hasControlRoomKey = false;
    public bool defeated2ndWave = false;
    [FormerlySerializedAs("hasExplosive")] public bool hasControlPanelKey = false;
    
#if  UNITY_EDITOR
    [FormerlySerializedAs("_debug")]
    [Space]
    [Header("Debug variables.")]
    [SerializeField] public DebugAsset debugAsset;
#endif
    
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

    public void CheckKey()
    {
        if (killedEnemies == 2)
        {
            StartCoroutine(UIManager.Instance.StartText("You got a key!"));
            hasControlRoomKey = true;
        }
        else if (killedEnemies == 4)
        {
            defeated2ndWave = true;
            StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.afterDefeatingSecondWave));
        }
    }

    public void ActivateJournal(int journalIndex)
    {
        journalStates[journalIndex] = true;
        
        UIManager.Instance.journalButtons[journalIndex].interactable = true;
        UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f, true);
        UIManager.Instance.journalButtons[journalIndex].onClick.Invoke();
    }

    public void LoadData(GameData gameData)
    {
        journalStates = gameData.activeJournals;
    }

    public void SaveData(GameData gameData)
    {
        gameData.activeJournals = journalStates;
    }
}
