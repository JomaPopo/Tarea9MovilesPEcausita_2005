using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sfxSounds;
    public Sound[] musicTracks;

    public void PlayMusic(string trackName)
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            if (musicTracks[i].name == trackName)
            {
                musicSource.clip = musicTracks[i].clip;
                musicSource.Play();
                return;
            }
        }
        Debug.LogWarning("Track not found: " + trackName);
    }

    public void PlaySFX(string soundName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == soundName)
            {
                sfxSource.PlayOneShot(sfxSounds[i].clip);
                return;
            }
        }
        Debug.LogWarning("SFX not found: " + soundName);
    }
}