namespace GameBoxSdk.Runtime.Sound
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource = null;

        #region Unity Methods

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        public void UpdateDefaultClip(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void PlayClipOneShot(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void UnPause()
        {
            audioSource.UnPause();
        }

        public void SetIsSpatial(bool isSpatial)
        {
            audioSource.spatialBlend = isSpatial ? 1 : 0;
        }

        public void SetIsLooping(bool isLooping)
        {
            audioSource.loop = isLooping;
        }

        public void UpdateVolume(float volume)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}

