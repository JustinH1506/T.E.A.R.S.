using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("audio")] public AudioSource source;
    [SerializeField] private DialogueData[] dialogueData;
    [SerializeField] private TextMeshProUGUI dialogueTextComponent;

    private void Awake()
    {
        Instance = this;
        
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Sets dialogue values.
    /// </summary>
    void Start()
    {
        dialogueTextComponent.text = string.Empty;
    }
    
    /// <summary>
    /// Starts the dialogue and sets needed values.
    /// </summary>
    public IEnumerator StartDialogue()
    {
        UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.None, 1f);
        
        for (int i = 0; i < dialogueData.Length; i++)
        {
            dialogueTextComponent.text = dialogueData[i].line;
            source.clip = dialogueData[i].audioClip;
            source.Play();
            
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
        UIManager.Instance.CloseMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f);
    }
}