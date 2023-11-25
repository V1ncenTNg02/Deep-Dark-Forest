using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Serializable]
    public struct Audio
    {
        public String name;
        public AudioClip audioClip;
        [Range(0f, 1f)] public float volume;
        [Range(0f, 2f)] public float speed;
    }

    public List<Audio> audios;
    private AudioSource audioSource;

    private string playingAudio;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(string name)
    {
        foreach (Audio audio in audios)
        {
            if (audio.name == name && audio.name != playingAudio)
            {
                playingAudio = audio.name;
                audioSource.clip = audio.audioClip;
                audioSource.volume = audio.volume;
                audioSource.pitch = audio.speed;
                audioSource.Play();
                break;
            }
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
        playingAudio = null;
    }
}
