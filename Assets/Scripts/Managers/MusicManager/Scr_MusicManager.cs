using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MusicManager : PersistentSingleton<Scr_MusicManager>
{
    public enum SoundType { SOUND, MUSIC, MENU_SOUND, TEST_SOUND}

    [System.Serializable]
    public struct SoundData
    {
        public SoundType soundType;
        public List<AudioClip> clips;
    }

    [System.Serializable]
    public struct VolumeSetting
    {
        public SoundType soundType;
        public float volume;
        public bool isSingle;
    }

    [SerializeField]
    VolumeSettings soundSettings;

    Dictionary<SoundType, AudioSource> audioSourceDictionary = new Dictionary<SoundType, AudioSource>();

    Dictionary<SoundType, VolumeSetting> volumeDictionary;

    public void PlayRandom(SoundData soundData)
    {
        float volume = 0;
        if (volumeDictionary.ContainsKey(soundData.soundType))
            volume = volumeDictionary[soundData.soundType].volume;
        AudioClip clip = null;
        if(soundData.clips.Count > 0)
        {
            int random = Random.Range(0, soundData.clips.Count);
            clip = soundData.clips[random];
        }
        AudioSource source = audioSourceDictionary[soundData.soundType];
        if (volumeDictionary[soundData.soundType].isSingle)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip, volume);
        }
    }

    public void PlaySound(SoundData soundData, int soundOrder)
    {
        float volume = 0;
        if (volumeDictionary.ContainsKey(soundData.soundType))
            volume = volumeDictionary[soundData.soundType].volume;


        AudioClip clip = null;
        if (soundData.clips.Count > 0)
        {
            clip = soundData.clips[soundOrder];
        }
        AudioSource source = audioSourceDictionary[soundData.soundType];
        if (volumeDictionary[soundData.soundType].isSingle)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip, volume);
        }
    }

    public  override void Awake ()
	{
        base.Awake();
        Debug.Log("awakening sound system");
        volumeDictionary = new Dictionary<SoundType, VolumeSetting>();
        for (int i = 0; i < soundSettings.volumeSettings.Count; i++)
        {
            volumeDictionary.Add(soundSettings.volumeSettings[i].soundType, soundSettings.volumeSettings[i]);
        }
        for (int i = 0; i < soundSettings.volumeSettings.Count; i++) {
            audioSourceDictionary.Add(soundSettings.volumeSettings[i].soundType, CreateAudioSource(soundSettings.volumeSettings[i].soundType.ToString(), soundSettings.volumeSettings[i].isSingle));
        }
	}

    public  void         StopSound   (SoundType soundType)
    {
        if (audioSourceDictionary.ContainsKey(soundType))
            audioSourceDictionary[soundType].Stop(); ;
    }

    public void PauseSound(SoundType soundType)
    {
        if (audioSourceDictionary.ContainsKey(soundType))
            audioSourceDictionary[soundType].Pause(); ;
    }
    

    private AudioSource  CreateAudioSource     (string name, bool isLoop)
    {
        GameObject temporaryAudioHost         = new GameObject(name);
        AudioSource audioSource               = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;  
		audioSource.playOnAwake               = false;
        audioSource.loop                      = isLoop;
        audioSource.spatialBlend              = 0.0f;
        temporaryAudioHost.transform.SetParent(this.transform);
        return audioSource;
    }

}

