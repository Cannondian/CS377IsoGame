using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SoundManager class based on https://www.youtube.com/watch?v=QL29aTa7J5Q
// Object pooling based on https://www.youtube.com/watch?v=YCHJwnmUGDk
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // singleton

    // We have a single gameObject for handling position-less one shot soundFX
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    // However, for handling soundFX that have a position, we create a pool of soundFX containers on initialization
    private List<GameObject> pooledContainers = new List<GameObject>();
    private List<AudioSource> pooledAudioSources = new List<AudioSource>();
    private int amountToPool = 100;

    // Enum of all soundFX
    public enum Sound 
    {
        Grenadier_Fire,
        Grenadier_GrenadeExplode,
        // TODO, more...
    }

    // An array of Sound and AudioClip associations that we can fill in in the editor
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable] public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    // For certain sounds that can only play after some pre-determined time
    private static Dictionary<Sound, float> soundTimerDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Instantiate our pooled soundFX containers for positional audio
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject container = new GameObject("Sound");
            AudioSource audioSource = container.AddComponent<AudioSource>();

            // Set the parent transform of the container to be this sound manager and disable it by default
            container.transform.parent = instance.gameObject.transform;
            container.SetActive(false);

            // Add the container and its corresponding audio source to their respective lists
            // Then, we can access the container and its audio source component with the same index
            pooledContainers.Add(container);
            pooledAudioSources.Add(audioSource);
        }

        soundTimerDictionary = new Dictionary<Sound, float>();
        // TODO: initialize values...
    }

    public int GetPooledContainerIndex()
    {
        // Iterate through all pooled soundFX containers...
        for (int i = 0; i < pooledContainers.Count; i++)
        {
            // If an inactive container is available, return that container
            if (!pooledContainers[i].activeInHierarchy)
            {
                return i;
            }
        }

        // We only get here if there are no available containers.
        // If we ever see this error message, we should consider increasing amountToPool
        Debug.LogError("No inactive soundFX containers available!");
        return -1;
    }

    // Play a single shot of a desired sound
    public static void PlaySound(Sound sound, float volume = 1f)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null) 
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

                // Set the parent transform to be this sound manager
                oneShotGameObject.transform.parent = instance.gameObject.transform;
            }

            oneShotAudioSource.PlayOneShot(GetAudioClip(sound), volume);
        }
    }

    // Play a single shot of a desired sound at a particular position
    public static void PlaySound(Sound sound, Vector3 position, float volume = 1f)
    {
        if (CanPlaySound(sound))
        {
            int containerIndex = instance.GetPooledContainerIndex();
            if (containerIndex >= 0)
            {
                GameObject container = instance.pooledContainers[containerIndex];
                AudioSource source = instance.pooledAudioSources[containerIndex];

                container.transform.position = position;
                source.clip = GetAudioClip(sound);

                // Optional parameter adjustments...
                source.volume = volume;
                // audioSource.maxDistance = 100f;
                // audioSource.spatialBlend = 1f;
                // audioSource.rolloffMode = AudioRolloffMode.Linear;
                // audioSource.dopplerLevel = 0f;

                container.SetActive(true);
                source.Play();

                // Disable the audio container after the clip is done playing
                instance.StartCoroutine(instance.DisableContainerAfterDelay(container, source.clip.length));
            }
        }
    }

    private IEnumerator DisableContainerAfterDelay(GameObject container, float delay)
    {
        yield return new WaitForSeconds(delay);
        container.SetActive(false);
    }

    // Returns the audioClip associated with a given sound
    private static AudioClip GetAudioClip(Sound sound)
    {
        // Iterate through all sounds, returning matching sound's audioClip
        foreach (SoundAudioClip soundAudioClip in instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        // We only get here if no matching sound was found
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private static bool CanPlaySound(Sound sound) 
    {
        switch (sound)
        {
            default:
                return true;

            // case Sound.PlaceHolder:
            //     // Some special condition...
            //     return true;

        }
    }
}