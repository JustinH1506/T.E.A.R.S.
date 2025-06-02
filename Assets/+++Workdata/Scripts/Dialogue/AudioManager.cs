using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [System.Serializable]
    public class DialogueData
    {
        [TextArea(3, 10)]
        public string line;
        public AudioClip audioClip;
    }

    public AudioSource audio;
    [SerializeField] private DialogueData[] dialogueData;

    [SerializeField] private TextMeshProUGUI dialogueTextComponent;
    public bool dialogueWasActive;
    
    [SerializeField] private float textSpeed;

    public bool isPlaying;

    private int dialogueDataIndex;

    private void Awake()
    {
        Instance = this;
        
        audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Sets dialogue values.
    /// </summary>
    void Start()
    {
        dialogueTextComponent.text = string.Empty;
        //speakerTextComponent.text = string.Empty;
        dialogueWasActive = true;
    }
    
    /// <summary>
    /// Starts the dialogue and sets needed values.
    /// </summary>
    public IEnumerator StartDialogue()
    {
        dialogueDataIndex = 0;
        isPlaying = true;
        UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.None, 1f);
        
        for (int i = 0; i < dialogueData.Length; i++)
        {
            dialogueTextComponent.text = dialogueData[i].line;
            audio.clip = dialogueData[i].audioClip;
            audio.Play();
            
            yield return new WaitForSeconds(dialogueData[i].audioClip.length);
        }
        
        AfterText();
    }
    
    
    /// <summary>
    /// Depending if isPreTutorial is active either opens the level selector menu or sets the player action and in game ui active.
    /// </summary>
    private void AfterText()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().EnablePlayerActions();
        isPlaying = false;
        UIManager.Instance.CloseMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f);
    }
}
