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
    [Range(0f, 1f)] [SerializeField] private float spatialBlend = 1f;
    [SerializeField] private bool looping = false;
    
    public AudioClip Clip => clip;   
    public float Volume => volume;
    public AudioType AudioType => audioType;
    public float SpatialBlend => spatialBlend;
    public bool Looping => looping;

    public AudioSettings(AudioClip clip, float volume = 1f, AudioType audioType = AudioType.OTHER, float spatialBlend = 1, bool looping = false)
    {
        this.clip = clip;
        this.volume = volume;
        this.audioType = audioType;
        this.spatialBlend = spatialBlend;
        this.looping = looping;
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
    [SerializeField] private GlobalvfxSO globalfxSO;
    [Range(0, 100)] [SerializeField] private int masterVolume = 100;
    [Range(0, 100)] [SerializeField] private int towerVolume = 100;
    [Range(0, 100)] [SerializeField] private int unitVolume = 100;
    [Range(0, 100)] [SerializeField] private int otherVolume = 100;

    public GlobalvfxSO GlobalEffects => globalfxSO;
    public int MasterVolume { get => masterVolume; set { masterVolume = value; } }
    public int TowerVolume { get => towerVolume; set { towerVolume = value; } }
    public int UnitVolume { get => unitVolume; set { unitVolume = value; } }
    public int OtherVolume { get => otherVolume; set { otherVolume = value; } }

    [Header("Components")]
    [SerializeField] private GameObject audioPrefab;

    public static FXManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public GameObject PlaySound(AudioSettings settings, Vector3 position)
    {
        if (settings.Clip == null) return null;

        GameObject audioObject = Instantiate(audioPrefab, position, Quaternion.identity);
        AudioSource source = audioObject.GetComponent<AudioSource>();

        source.clip = settings.Clip;
        source.volume = CalculateVolume(settings.AudioType, settings.Volume);
        source.loop = settings.Looping;
        source.spatialBlend = settings.SpatialBlend;

        source.Play();

        if (!settings.Looping)
            Destroy(audioObject, settings.Clip.length);
        
        return audioObject;
    }

    public GameObject PlayEffect(EffectSettings settings, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (settings.EffectPrefab == null) return null;

        GameObject particle = Instantiate(settings.EffectPrefab);
        particle.transform.position = position;
        particle.transform.rotation = rotation;
        particle.transform.localScale = scale;

        if (settings.Lifetime > 0)
            Destroy(particle, settings.Lifetime);
        
        return particle;
    }

    public (GameObject, GameObject) PlayFXPair(FXPair fXPair)
    {
        return (PlaySound(fXPair.AudioSettings, Vector3.zero), PlayEffect(fXPair.EffectSettings, Vector3.zero, Quaternion.identity, Vector3.one));
    }

    public (GameObject, GameObject) PlayFXPair(FXPair fXPair, Vector3 position)
    {
        return (PlaySound(fXPair.AudioSettings, position), PlayEffect(fXPair.EffectSettings, position, Quaternion.identity, Vector3.one));
    }

    public (GameObject, GameObject) PlayFXPair(FXPair fXPair, Vector3 position, Quaternion rotation)
    {
        return (PlaySound(fXPair.AudioSettings, position), PlayEffect(fXPair.EffectSettings, position, rotation, Vector3.one));
    }

    public (GameObject, GameObject) PlayFXPair(FXPair fXPair, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        return (PlaySound(fXPair.AudioSettings, position), PlayEffect(fXPair.EffectSettings, position, rotation, scale));
    }

    public void PlayThemeStartScreenSound()
    {
        PlayFXPair(globalfxSO.RuinsTheme.mainscreenTheme, Vector3.zero, Quaternion.identity);
    }

    public void PlayThemeAtmosphereSound()
    {
        PlayFXPair(globalfxSO.RuinsTheme.gameAtmosphere, Vector3.zero, Quaternion.identity);
    }

    public void PlayThemeProceedSound()
    {
        PlayFXPair(globalfxSO.RuinsTheme.proceedClick, Vector3.zero, Quaternion.identity);
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
