using UnityEngine;
using UnityEngine.Audio;
using System;
using BaseUtilities;

public class AudioManager : SingletonBase<AudioManager>
{
    [SerializeField] AudioMixer audioMixer;

    public Sound[] sounds;

    public const string volumeKEY = "volume";
    public const string musicKEY = "musicVolume";
    public const string sfxKEY = "sfxVolume";

    void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = sound.mixerGroup;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        LoadVolume();
    }
    public void Play(string name)
    {
        FindSong(name).source.Play();
    }
    public void PlayOnce(string name)
    {
        if (!FindSong(name).source.isPlaying)
        {
            Play(name);
        }
    }
    public void Stop(string name)
    {
        FindSong(name).source.Stop();
    }
    Sound FindSong(string _name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {_name} not found!");
            return null;
        }
        return s;
    }
    public void StopAllSFX()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.mixerGroup.ToString() == "SFX")
            {
                Stop(sound.name);
            }
        }
    }
    void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat(volumeKEY, 1f);
        audioMixer.SetFloat(volumeKEY, Mathf.Log10(volume) * 20f);
        float music = PlayerPrefs.GetFloat(musicKEY, 1f);
        audioMixer.SetFloat(musicKEY, music);
        float sfx = PlayerPrefs.GetFloat(sfxKEY, 1f);
        audioMixer.SetFloat(sfxKEY, sfx);
    }
    public float GetMusicVolume()
    {
        audioMixer.GetFloat(musicKEY, out float value);
        return value;
    }
    public float GetSFXVolume()
    {
        audioMixer.GetFloat(sfxKEY, out float value);
        return value;
    }
    void ToggleMute(bool _mute, string _key)
    {
        if (_mute)
        {
            audioMixer.SetFloat(_key, -80f);
            PlayerPrefs.SetFloat(_key, -80f);
        }
        else
        {
            audioMixer.SetFloat(_key, 1f);
            PlayerPrefs.SetFloat(_key, 1f);
        }
        _mute = !_mute;
    }
    public void ToggleMusic(bool _mute)
    {
        ToggleMute(_mute, musicKEY);
    }
    public void ToggleSFX(bool _mute)
    {
        ToggleMute(_mute, sfxKEY);
    }
}
