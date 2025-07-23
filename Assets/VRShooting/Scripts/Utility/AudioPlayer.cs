using UnityEngine;

namespace VRShooting.Scripts.Utility
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioSource PlayOneShotAudioAtPoint(
            AudioClip clip, Vector3 position, float volume = 1f ,float spatialBlend = 0.5f)
        {
            var audioObj = new GameObject("Dead Audio");
            audioObj.transform.position = position;
            var audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = spatialBlend;
            audioSource.Play();
            Destroy(audioObj, clip.length);
            return audioSource;
        }

        public static AudioSource PlayLoopAudioAtPoint(
            AudioClip clip, Vector3 position, float volume = 1f ,float spatialBlend = 0.5f)
        {
            var audioObj = new GameObject("Loop Audio");
            audioObj.transform.position = position;
            var audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = spatialBlend;
            audioSource.loop = true;
            audioSource.Play();
            return audioSource;
        }
    }
}