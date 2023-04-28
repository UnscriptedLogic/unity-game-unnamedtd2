using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    TOWER,
    UNITS,
    OTHER
}

public struct AudioSettings
{
    private AudioClip clip;
    private float volume;
    private AudioType audioType;
    
    public AudioClip Clip => clip;   
    public float Volume => volume;
    public AudioType AudioType => audioType;

    public AudioSettings(AudioClip clip, float volume = 1f, AudioType audioType = AudioType.OTHER)
    {
        this.clip = clip;
        this.volume = volume;
        this.audioType = audioType;
    }
}

public class SoundManager : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] private int masterVolume;
    [Range(0, 100)] [SerializeField] private int towerVolume;
    [Range(0, 100)] [SerializeField] private int unitVolume;

    public int MasterVolume { get => masterVolume; set { masterVolume = value; } }
    public int TowerVolume { get => towerVolume; set { towerVolume = value; } }
    public int UnitVolume { get => unitVolume; set { unitVolume = value; } }

    [Header("Components")]
    [SerializeField] private GameObject audioPrefab;

    public static SoundManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioSettings settings, Vector3 position)
    {
        GameObject audioObject = Instantiate(audioPrefab, position, Quaternion.identity);
        AudioSource source = audioObject.GetComponent<AudioSource>();

        source.clip = settings.Clip;
        source.volume = CalculateVolume(settings.AudioType, settings.Volume);
        source.loop = false;

        source.Play();

        Destroy(audioObject, settings.Clip.length);
    }

    public float CalculateVolume(AudioType audioType, float requestedVolume)
    {
        float volume = 0f;

        switch (audioType)
        {
            case AudioType.TOWER:
                volume = requestedVolume / 100 * towerVolume;
                break;
            case AudioType.UNITS:
                volume = requestedVolume / 100 * unitVolume;
                break;
            case AudioType.OTHER:
                break;
            default:
                break;
        }

        volume = volume / 100 * masterVolume;
        return volume;
    }
}
