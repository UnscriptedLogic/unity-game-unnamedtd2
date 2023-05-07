using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    TOWER,
    UNITS,
    OTHER
}

[System.Serializable]
public class AudioSettings
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume;
    [SerializeField] private AudioType audioType;
    
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

[System.Serializable]
public class EffectSettings
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float lifetime;

    public GameObject EffectPrefab => effectPrefab;
    public float Lifetime => lifetime;

    public EffectSettings(GameObject effectPrefab, float lifetime)
    {
        this.effectPrefab = effectPrefab;
        this.lifetime = lifetime;
    }
}

public class FXManager : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] private int masterVolume = 100;
    [Range(0, 100)] [SerializeField] private int towerVolume = 100;
    [Range(0, 100)] [SerializeField] private int unitVolume = 100;
    [Range(0, 100)] [SerializeField] private int otherVolume = 100;

    public int MasterVolume { get => masterVolume; set { masterVolume = value; } }
    public int TowerVolume { get => towerVolume; set { towerVolume = value; } }
    public int UnitVolume { get => unitVolume; set { unitVolume = value; } }

    [Header("Components")]
    [SerializeField] private GameObject audioPrefab;

    public static FXManager instance { get; private set; }

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

    public void PlayEffect(EffectSettings settings, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject particle = Instantiate(settings.EffectPrefab, position, rotation);
        particle.transform.localScale = scale;
        Destroy(particle, settings.Lifetime);
    }

    /// <summary>
    /// Plays a sound and visual effect prefab from the global constant list of effects.
    /// </summary>
    public void PlayGlobalEffect()
    {

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
                volume = requestedVolume / 100 * otherVolume;
                break;
            default:
                break;
        }

        volume = volume / 100 * masterVolume;
        return volume;
    }
}