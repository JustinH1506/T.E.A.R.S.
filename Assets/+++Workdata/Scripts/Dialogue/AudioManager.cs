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

    [Header("Dialogue Variables")]
    public AudioSource dialogueSource;
    [SerializeField] private DialogueData[] ringSubtitleData;
    [SerializeField] private TextMeshProUGUI dialogueTextComponent;
    [Space]
    
    [Header("Music Clips")]
    public AudioSource musicSource;
    public AudioClip mainMenuMusic;
    public AudioClip inBattleMusic;
    public AudioClip inGameMusic;
    public AudioClip gameOverMusic;
    
    [Header("Sound Effects Clips")]
    public AudioSource soundSource;
    public AudioClip playerStepSounds;
    public AudioClip enemyStepSounds;
    public AudioClip swordAttackSound;
    public AudioClip swordHitSound;
    public AudioClip enemyHitSound;
    
    private void Awake()
    {
        Instance = this;
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
        
        for (int i = 0; i < ringSubtitleData.Length; i++)
        {
            dialogueTextComponent.text = ringSubtitleData[i].line;
            dialogueSource.clip = ringSubtitleData[i].audioClip;
            dialogueSource.Play();
            
            yield return new WaitForSeconds(ringSubtitleData[i].audioClip.length);
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

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        
        musicSource.clip = musicClip;
        
        musicSource.Play();
    }

    public void PlaySound(AudioClip soundClip,AudioSource source , bool useRandomPitch)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }

        if (useRandomPitch)
        {
            source.pitch = Random.Range(0.5f, 1.6f);
        }
        
        source.clip = soundClip;
        
        source.Play();
    }

    public void ChangeAllSfxSources(bool isPlaying)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        
        
        if (isPlaying)
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                allAudioSources[i].Pause();
            }
        }
        else
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                allAudioSources[i].UnPause();
            }
        }
    }
}