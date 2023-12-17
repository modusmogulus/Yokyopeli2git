using UnityEngine;
using System;
using System.Collections.Generic;


public enum AudioChan //Chan = short for channel lol (well i guess you could call me audio chan too lmao)
{
    DEFAULT,
    SFX,
    PLAYERSOUND,
    MUSIC,
    DEBUG,

}

/*You wont believe how much i tried to fight with unity to
show dictionaries in the inspector... Only to find out it's not
even possible!?! System.Serializable wont help there lol, that's
why I'm using classes instead

t. aino
*/

[System.Serializable]
public class SpAudioClass
{
    public AudioChan AudioChan;
    public string audioName;
    public float minVolume = 1f;
    public float maxVolume = 1f;
    public AudioClip[] audioClips;
    public float minPitch = 1f;
    public float maxPitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("AudioManager");
                    _instance = singleton.AddComponent<AudioManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    [SerializeField]
    private SpAudioClass[] audioClasses;

    private Dictionary<AudioChan, List<AudioSource>> audioSourcesDictionary;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
            InitializeAudioSources();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        audioSourcesDictionary = new Dictionary<AudioChan, List<AudioSource>>();

        foreach (AudioChan AudioChan in Enum.GetValues(typeof(AudioChan)))
        {
            int maxSimultaneousSounds = GetMaxSimultaneousSounds(AudioChan);
            List<AudioSource> audioSources = new List<AudioSource>();

            for (int i = 0; i < maxSimultaneousSounds; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSources.Add(audioSource);

                // Check if it's the MUSIC channel and set the loop to true
                if (AudioChan == AudioChan.MUSIC)
                {
                    audioSource.loop = true;
                }
            }

            audioSourcesDictionary.Add(AudioChan, audioSources);
        }
    }

    private int GetMaxSimultaneousSounds(AudioChan AudioChan)
    { //Sleepy blåhaj weighted blanket coding time :3
        switch (AudioChan)
        {
            case AudioChan.DEFAULT:
                return 4;
            case AudioChan.SFX:
                return 2;
            case AudioChan.PLAYERSOUND:
            case AudioChan.MUSIC:
            case AudioChan.DEBUG:
                return 1;
            default:
                return 1;
        }
    }

    //Find audio class by name
    /*I'm still flabbergasted that Unity cant show dictionaries in the inspector?!?
    I would use them if Unity could TwT
    but now I have to iterate through EVERY audioclass!!!
    */
    private SpAudioClass FindAudioClass(string audioName)
    {
        foreach (SpAudioClass AudioChan in audioClasses)
        {
            if (AudioChan.audioName == audioName)
            {
                return AudioChan;
            }
        }

        Debug.LogWarning("Audio class with name " + audioName + " not found.");
        return null;
    }

    // Play audio by name :3
    public void PlayAudio(string audioName)
    {
        SpAudioClass AudioChan = FindAudioClass(audioName);

        if (AudioChan != null)
        {
            List<AudioSource> audioSources = audioSourcesDictionary[AudioChan.AudioChan];
            AudioSource availableSource = null;

            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    availableSource = audioSource;
                    break;
                }
            }

            if (availableSource == null)
            {
                // If voices are maxed out, use the last one
                availableSource = audioSources[audioSources.Count - 1];
            }

            //------------------ Hilavitkutin  features --------------
            float randomVolume = UnityEngine.Random.Range(AudioChan.minVolume, AudioChan.maxVolume);
            availableSource.volume = randomVolume;

            // Random pitch
            float randomPitch = UnityEngine.Random.Range(AudioChan.minPitch, AudioChan.maxPitch);
            availableSource.pitch = randomPitch;

            if (AudioChan.audioClips.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, AudioChan.audioClips.Length);
                availableSource.clip = AudioChan.audioClips[randomIndex];
                availableSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioClassista " + audioName + " puuttuu audioclipit??");
            }
        }
    }
}