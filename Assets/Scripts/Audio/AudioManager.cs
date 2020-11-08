using System.Collections;
using Runtime.Effects;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Audio {
    public class AudioManager : MonoBehaviour {
        public static AudioManager instance;

        [SerializeField, Expandable]
        AudioSource sourcePrefab = default;
        [SerializeField]
        AnimationCurve fadeOutCurve = default;
        [SerializeField]
        AnimationCurve fadeInCurve = default;

        AudioSource musicSource;

        [SerializeField]
        EffectEvent onStart = default;

        void OnEnable() {
            instance = this;
        }
        void Start() {
            onStart.Invoke(gameObject);
        }

        public AudioSource InstantiateAudioSource(AudioInfo audio) {
            var source = Instantiate(sourcePrefab, audio.position, Quaternion.identity, transform);
            source.loop = audio.loop;
            source.clip = audio.clip;
            source.outputAudioMixerGroup = audio.mixer;
            source.spatialBlend = audio.spatialBlend;
            source.pitch = audio.pitch;
            source.volume = audio.volume;
            source.time = audio.timeOffset;
            return source;
        }

        public void PlayMusic(AudioInfo audio, float crossFadeDuration) {
            if (musicSource) {
                audio.timeOffset = musicSource.time;
                StartCoroutine(CreateFadeRoutine(musicSource, fadeOutCurve, crossFadeDuration));
                Destroy(musicSource.gameObject, crossFadeDuration);
            }
            musicSource = InstantiateAudioSource(audio);
            musicSource.Play();
            StartCoroutine(CreateFadeRoutine(musicSource, fadeInCurve, crossFadeDuration));
        }
        IEnumerator CreateFadeRoutine(AudioSource source, AnimationCurve curve, float duration) {
            for (float timer = 0; timer < duration; timer += Time.deltaTime) {
                if (source) {
                    source.volume = curve.Evaluate(timer / duration);
                }
                yield return null;
            }
            if (source) {
                source.volume = curve.Evaluate(1);
            }
        }

        public void PlaySFX(AudioInfo audio) {
            var source = InstantiateAudioSource(audio);
            source.Play();
            if (audio.playDuration > 0) {
                StartCoroutine(CreateMuteRoutine(source, audio.playDuration));
            }
            if (!audio.loop) {
                StartCoroutine(CreateDestructionRoutine(source, source.clip.length));
            }
        }

        IEnumerator CreateMuteRoutine(AudioSource source, float duration) {
            yield return new WaitForSeconds(duration);
            source.mute = true;
        }

        IEnumerator CreateDestructionRoutine(AudioSource source, float duration) {
            yield return new WaitForSeconds(duration);
            Destroy(source.gameObject);
        }
    }
}