using _Framework.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    SFX_PlayerDie = 0,
    SFX_ThrowWeapon = 1,
    SFX_SizeUp = 2,
    SFX_WeaponHit = 3,
    SFX_ButtonClick = 4,
    SFX_Count = 5,
    SFX_EndWin = 6,
    SFX_EndLose = 7,
}
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] List<AudioClip> clips = new List<AudioClip>();

    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        for (int i = 0; i < clips.Count; i++)
        {
            GameObject audioObject = new GameObject("AudioSource_" + i);

            AudioSource audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.clip = clips[i];

            audioSources.Add(audioSource);

            audioObject.transform.SetParent(transform);
        }
    }
    public void Play(AudioType type)
    {
        audioSources[(int)type]?.Play();
    }
}
