using System.Collections;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
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
    public DialogueData[] mainCharacterStartDialogue;
    public DialogueData[] ringSubtitleData;
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
    public IEnumerator StartDialogue(DialogueData[] dialogueData, CinemachineVirtualCamera vcam)
    {
        UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f, true);
        
        for (int i = 0; i < dialogueData.Length; i++)
        {
            dialogueTextComponent.text = dialogueData[i].line;
            dialogueSource.clip = dialogueData[i].audioClip;
            dialogueSource.Play();

            if (vcam != null)
            {
                vcam.Priority = 11;
            }
            
            yield return new WaitForSeconds(dialogueData[i].audioClip.length);
        }
        
        if (vcam != null)
        {
            vcam.Priority = 9;
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