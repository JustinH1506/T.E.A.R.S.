using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

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

    public Coroutine dialogueCoroutine;

    [Header("Dialogue Variables")]
    public AudioSource dialogueSource;
    public DialogueData[] mainCharacterStartDialogue;
    public DialogueData[] lockedLockerDialogue;
    public DialogueData[] lockedDoor;
    public DialogueData[] ringSubtitleData;
    public DialogueData[] controlPanelWithoutKey;
    public DialogueData[] afterDefeatingSecondWave;
    public DialogueData[] activatingControlPanel;
    
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
    public AudioClip[] playerStepSounds;
    public AudioClip[] enemyStepSounds;
    public AudioClip[] swordAttackSounds;
    public AudioClip enemyHitSound;
    public AudioClip playerHitSound;
    
    [Header("Audio Mixer Groups")]
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundMixerGroup;

    [Header("Audio Listener")] 
    public AudioListener mainMenuListener;

    [Header("Ui Sounds")] 
    public AudioClip uiPressSound;

    [Header("Specific Variables")] 
    public bool makeControlPanelActive;
    public bool controlPanelActive;
    
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
        
        PlayMusic(mainMenuMusic);
    }
    
    /// <summary>
    /// Starts the dialogue and sets needed values.
    /// </summary>
    public IEnumerator StartDialogue(DialogueData[] dialogueData)
    {
        UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f, false);
        
        for (int i = 0; i < dialogueData.Length; i++)
        {
            dialogueTextComponent.text = dialogueData[i].line;
            dialogueSource.clip = dialogueData[i].audioClip;
            dialogueSource.Play();
            
            yield return new WaitForSeconds(dialogueData[i].audioClip.length);
        }

        if (makeControlPanelActive)
        {
            controlPanelActive = true;
        }
        
        AfterText();
    }
    
    /// <summary>
    /// Depending if isPreTutorial is active either opens the level selector menu or sets the player action and in game ui active.
    /// </summary>
    private void AfterText()
    {
        UIManager.Instance.CloseMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f);
    }

    
    /// <summary>
    /// Stops the dialogue menu and Voice lines. 
    /// </summary>
    public void StopDialogue()
    {
        StopAllCoroutines();
        soundSource.Stop();
        UIManager.Instance.CloseMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f);
    }

    /// <summary>
    /// Plays the given audio clip.
    /// </summary>
    /// <param name="musicClip"></param>
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        
        musicSource.clip = musicClip;
        
        musicSource.Play();
    }

    /// <summary>
    /// Plays the given audio clip in given audio source.
    /// </summary>
    /// <param name="soundClip"></param>
    /// <param name="source"></param>
    public void PlaySound(AudioClip soundClip,AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
        
        source.clip = soundClip;
        
        source.Play();
    }

    /// <summary>
    /// Changes all Audio Sources that are SFX.
    /// </summary>
    /// <param name="isPlaying"></param>
    public void ChangeAllSfxSources(bool isPlaying)
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        
        if (isPlaying)
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].outputAudioMixerGroup == soundMixerGroup)
                {
                    allAudioSources[i].Pause();
                }
            }
        }
        else
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].outputAudioMixerGroup == soundMixerGroup)
                {
                    allAudioSources[i].UnPause();
                }
            }
        }
    }
    
    /// <summary>
    /// Changes all Audio Sources that are Music.
    /// </summary>
    /// <param name="isPlaying"></param>
    public void ChangeAllMusicSources(bool isPlaying)
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        
        if (isPlaying)
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].outputAudioMixerGroup == musicMixerGroup)
                {
                    allAudioSources[i].Pause();
                }
            }
        }
        else
        {
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                if (allAudioSources[i].outputAudioMixerGroup == musicMixerGroup)
                {
                    allAudioSources[i].UnPause();
                }
            }
        }
    }
    
    /// <summary>
    /// Changes all Audio Sources.
    /// </summary>
    /// <param name="isPlaying"></param>
    public void ChangeAllSources(bool isPlaying)
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        
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