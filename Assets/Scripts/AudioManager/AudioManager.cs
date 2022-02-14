using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup masterMixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			if(s.source.outputAudioMixerGroup == null)
				s.source.outputAudioMixerGroup = masterMixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void AdjustMasterVolume(float newVolumeValue)
	{
		masterMixerGroup.audioMixer.SetFloat("masterVolume", Mathf.Lerp(-80, 0, newVolumeValue));
	}

	public void AdjustMusicVolume(float newVolumeValue)
	{
		masterMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Lerp(-80, 0, newVolumeValue));
	}

	public void AdjustSFXVolume(float newVolumeValue)
	{
		masterMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Lerp(-80, 0, newVolumeValue));
	}
}
