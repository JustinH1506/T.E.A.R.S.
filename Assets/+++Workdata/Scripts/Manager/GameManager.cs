using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,
        InGame,
        InBattle
    }
    
    [Header("Game Variables")]
    public static GameManager Instance;

    public int currentAttackingEnemies;
    
    public GameStates gameStates;

    public bool[] journalStates;
    
    [FormerlySerializedAs("_debug")]
    [Space]
    
    [Header("Debug variables.")]
    [SerializeField] public DebugAsset debugAsset;
    
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
                
                UIManager.Instance.OpenMenu(UIManager.Instance.inGameUi, CursorLockMode.Locked, 1f);
            }
        }
        else
        {
            UIManager.Instance.OpenMenu(UIManager.Instance.mainMenuScreen, CursorLockMode.None, 1f);
        }
#endif
    }

    public void AddEnemies()
    {
        currentAttackingEnemies++;
        
        if (currentAttackingEnemies == 0)
        {
            gameStates = GameStates.InGame;
        }
    }

    public void RemoveEnemies()
    {
        currentAttackingEnemies--;

        if (currentAttackingEnemies >= 1)
        {
            gameStates = GameStates.InBattle;
        }
    }

    public void ActivateJournal(int journalIndex)
    {
        journalStates[journalIndex] = true;
        
        UIManager.Instance.journalButtons[journalIndex].interactable = true;
        UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f);
        UIManager.Instance.journalButtons[journalIndex].onClick.Invoke();
    }
}
