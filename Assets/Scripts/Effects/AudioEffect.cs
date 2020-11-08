using Runtime.Audio;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Runtime.Effects {
    [CreateAssetMenu(fileName = "Audio_New", menuName = "Effects/Audio")]
    public class AudioEffect : Effect {
        [SerializeField, Expandable]
        AudioClip[] clips = default;

        [SerializeField, Expandable]
        AudioMixerGroup mixer = default;

        [SerializeField, Range(0, 1)]
        float spatialBlend = 0;
        [SerializeField, Range(0, 5)]
        float mininumPitch = 1;
        [SerializeField, Range(0, 5)]
        float maxmimumPitch = 1;
        [SerializeField, Range(0, 5)]
        float mininumVolume = 1;
        [SerializeField, Range(0, 5)]
        float maxmimumVolume = 1;
        [SerializeField]
        bool loop = false;
        [SerializeField, Range(0, 10)]
        float timeOffset = 0;
        [SerializeField, Range(0, 10)]
        float playDuration = 0;

        [Header("BGM")]
        [SerializeField]
        bool isBackgroundMusic = false;
        [SerializeField, Range(0, 10)]
        float crossFadeDuration = 0;

        public override void Invoke(GameObject context) {
            if (!AudioManager.instance) {
                return;
            }
            var audio = new AudioInfo {
                position = context.transform.position,
                clip = clips.RandomElement(),
                mixer = mixer,
                spatialBlend = spatialBlend,
                pitch = Random.Range(mininumPitch, maxmimumPitch),
                volume = Random.Range(mininumVolume, maxmimumVolume),
                loop = loop,
                timeOffset = timeOffset,
                playDuration = playDuration,
            };
            if (isBackgroundMusic) {
                AudioManager.instance.PlayMusic(audio, crossFadeDuration);
            } else {
                AudioManager.instance.PlaySFX(audio);
            }
        }
    }
}
