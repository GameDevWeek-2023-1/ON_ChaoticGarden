using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    public enum Sound
    {
        ClickMenuPoint,
        EnemyDamage,
        EnemyShoot,
        FootstepsPlayer,
        GrassGrow,
        GrassPlant,
        MushroomDie,
        MushroomGrow,
        MushroomPlant,
        MushroomShot,
        PlayerDamage,
        PlayerDied,
        PlayerWin,
        SamenbagCollect,
        StepThroughGras,
        TimeSkip,
    }

    public static SoundEffectsController Instance { get; private set; }

    public event EventHandler OnSoundVolumeChanged;

    [SerializeField] private AudioSource _audioSourceOnShoot;

    private Dictionary<Sound, AudioClip> _soundEffectDictionary;
    private float _volume = 1f;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _soundEffectDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {
            _soundEffectDictionary.Add(sound, Resources.Load<AudioClip>(sound.ToString()));
        }
    }
    public void PlayOnShoot(Sound sound)
    {
        _audioSourceOnShoot.PlayOneShot(_soundEffectDictionary[sound], _volume);
    }
    public void AddVolume()
    {
        _volume += .1f;
        ClampVolume();
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public void ReduceVolume()
    {
        _volume -= .1f;
        ClampVolume();
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    private void ClampVolume()
    {
        _volume = Mathf.Clamp01(_volume);
        _audioSourceOnShoot.volume = _volume;
    }
    public float GetVolume() => _volume;
}
