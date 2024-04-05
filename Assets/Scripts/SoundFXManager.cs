using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXobject;

    public float maxDistance = 50f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXobject, spawnTransform.position, Quaternion.identity);

        //assign the audio clip
        audioSource.clip = audioClip;

        // Calculate the distance between the player and the sound source
        //float distanceToPlayer = Vector3.Distance(spawnTransform.position, GameObject.FindWithTag("Player").transform.position);

        // Calculate the volume based on the distance
        //float adjustedVolume = Mathf.Clamp01(1f - (distanceToPlayer / maxDistance)) * volume;


        //assign volume
        //audioSource.volume = adjustedVolume;

        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get the lenght of sound FX clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClips.Length);

        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXobject, spawnTransform.position, Quaternion.identity);

        //assign the audio clip
        audioSource.clip = audioClips[rand];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get the lenght of sound FX clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

    }
}
