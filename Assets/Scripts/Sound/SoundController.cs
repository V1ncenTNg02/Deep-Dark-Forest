using ORZ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip sound;
        public float volume;
    }

    public static SoundController Instance { get; private set; }

    public AudioSource MainSound;
    public AudioSource UISound;
    public AudioSource VolumeTest;
    public AudioSource BGM;

    public AudioSource[] AllSounds { get; set;}
    public GameObject[] Enemies { get; set; }

    public List<Sound> Sounds = new();

    public float Volume = 1.0f;

    public float minDistance = 10.0f;

    private bool enemyRoaring = false;

    private Coroutine enemySound = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AllSounds = FindObjectsOfType<AudioSource>();
    }

    public void RandomPlayEnemySound()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemySound = StartCoroutine(PlayEnemySound());
        Debug.Log(Enemies.Length);
    }

    public void StopEnemySound()
    {
        if (enemySound != null)
        {
            StopCoroutine(enemySound);
        }
        
    }

    IEnumerator PlayEnemySound()
    {
        while (true)
        {
            Vector3 playerPos = ObjectGetter.player.transform.position;
            float closestDis = float.MaxValue;
            foreach (var e in Enemies)
            {
                float distance = Vector3.Distance(playerPos, e.transform.position);
                if (distance < closestDis)
                {
                    closestDis = distance;
                }
            }
            if (Random.value < 0.1f + minDistance / closestDis && !enemyRoaring)
            {
                PlaySpecSound("GhostRoar");
                enemyRoaring = true;
                Invoke(nameof(EndRoaring), 6.0f);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    void EndRoaring()
    {
        enemyRoaring = false;
    }

    public void PlaySound(AudioClip audio)
    {
        MainSound.PlayOneShot(audio);
    }

    public void PlaySpecSound(string name)
    {
        foreach (var sound in Sounds)
        {
            if (sound.name == name)
            {
                MainSound.PlayOneShot(sound.sound, sound.volume);
                return;
            }
        }
    }

    public void PlayUISound(AudioClip audio)
    {
        UISound.PlayOneShot(audio);
    }

    public void PlaySpecUISound(string name)
    {
        foreach (var sound in Sounds)
        {
            if (sound.name == name)
            {
                UISound.PlayOneShot(sound.sound, sound.volume);
                return;
            }
        }
    }

    public void PlayVolumeTest(Slider slider)
    {
        VolumeTest.volume = slider.value;
        if (!VolumeTest.isPlaying)
        {
            VolumeTest.Play();
        }
        Volume = slider.value;
    }

    public void PlaySpecBGM(string name)
    {
        StartCoroutine(ChangeBGM(name));
    }

    IEnumerator ChangeBGM(string name)
    {
        while (BGM.volume > 0.0f)
        {
            BGM.volume -= Time.deltaTime;
            yield return null;
        }

        float volume = 0;
        foreach (var sound in Sounds)
        {
            if (sound.name == name)
            {
                BGM.clip = sound.sound;
                volume = sound.volume;
                BGM.Play();
            }
        }

        while (BGM.volume < volume)
        {
            BGM.volume += Time.deltaTime;
            yield return null;
        }

        BGM.volume = volume;
    }

    public void PlayBGM()
    {
        BGM.Play();
    }

    public void PauseBGM()
    {
        BGM.Pause();
    }

    public void UnPauseBGM()
    {
        BGM.UnPause();
    }

    public void PauseAllSounds()
    {
        foreach (var sound in AllSounds)
        {
            sound.Pause();
        }
    }

    public void UnPauseAllSounds()
    {
        foreach (var sound in AllSounds)
        {
            sound.UnPause();
        }
    }

    public void ChangeVolumeOfAllSounds(float rate)
    {
        foreach (var sound in AllSounds)
        {
            if(sound.gameObject.CompareTag("UISound") || sound.gameObject.CompareTag("TestSound")) continue;
            sound.volume = Volume * rate;
        }
    }

    public AudioSource[] GetAllSounds()
    {
        return AllSounds;
    }
}
