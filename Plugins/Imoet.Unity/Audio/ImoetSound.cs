using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity.Audio
{
    [SerializeField]
    public sealed class ImoetSound : UnitySingleton<ImoetSound>{
        [Header("Music Audio Source")]
        [SerializeField]
        private AudioSource m_musicAudio;

        [Header("SFX Audio Source")]
        [SerializeField]
        private AudioSource m_sfxAudio;

        [Header("Global Audio Source")]
        [SerializeField]
        private List<AudioSource> m_audios;

        public static AudioSource musicAudioSource { get { return current.m_musicAudio; } }
        public static AudioSource sfxAudioSource { get { return current.m_sfxAudio; } }
        public static List<AudioSource> audioSources { get { return current.m_audios; } }

        private void Awake() {
            m_audios = new List<AudioSource>(GetComponents<AudioSource>());
        }

        public static AudioSource Play(string audioName) {
            foreach (var audio in current.m_audios)
            {
                if (!audio.isPlaying) {
                    Play(audioName,audio);
                    return audio;
                }
            }
            var newAudio = _getNewAudio();
            Play(audioName, newAudio);
            return newAudio;
        }
        public static void Play(string audioName, AudioSource audioSource) {
            audioSource.clip = _clipFromQuery(audioName);
            audioSource.Play();
        }
        public static AudioSource PlayOneShot(string audioName) {
            foreach (var audio in current.m_audios) {
                if (!audio.isPlaying) {
                    PlayOneShot(audioName, audio);
                    return audio;
                } 
            }
            var newAudio = _getNewAudio();
            PlayOneShot(audioName, newAudio);
            return newAudio;
        }
        public static void PlayOneShot(string audioName, AudioSource audioSource) {
            audioSource.PlayOneShot(_clipFromQuery(audioName));
        }

        public static AudioSource PlaySFX(string audioName) {
            var clip = ImoetSoundDatabase.sfxPack.GetClip(audioName);
            if (clip != null && current.m_sfxAudio) {
                current.m_sfxAudio.PlayOneShot(clip);
                return current.m_sfxAudio;
            }
            current.m_sfxAudio = PlayOneShot("sfx/" + audioName);
            return current.m_sfxAudio;
        }

        public static void PlaySFX(string audioName, AudioSource audioSource) {
            PlayOneShot(audioName, audioSource);
        }

        public static AudioSource PlayMusic(string audioName) {
            var clip = ImoetSoundDatabase.musicPack.GetClip(audioName);
            if (clip != null && current.m_musicAudio) {
                current.m_musicAudio.clip = clip;
                current.m_musicAudio.Play();
                return current.m_musicAudio;
            }
            current.m_musicAudio = Play("music/" + audioName);
            return current.m_musicAudio;
        }

        public static void PlayMusic(string audioName, AudioSource audioSource) {
            Play(audioName, audioSource);
        }

        //Private Variable
        private static string[] _validateQuery(string query) {
            var res = new string[2];
            var splittedQuery = query.Split('/');
            var len = splittedQuery.Length;
            if (len > 1) {
                res[0] = splittedQuery[0];
                res[1] = query.Replace(res[0], "").Remove(0, 1);
            }
            else {
                res[0] = query;
            }
            return res;
        }

        private static AudioClip _clipFromQuery(string query) {
            var queryData = _validateQuery(query);
            if (!string.IsNullOrEmpty(queryData[1]))
            {
                var pack = ImoetSoundDatabase.GetPack(queryData[0]);
                if (pack != null)
                    return pack.GetClip(queryData[1]);
            }
            else {
                foreach (var pack in ImoetSoundDatabase.packs) {
                    var clip = pack.GetClip(queryData[0]);
                    if (clip != null)
                        return clip;
                }
            }
            return null;
        }

        private static AudioSource _getNewAudio() {
            var source = current.gameObject.AddComponent<AudioSource>();
            current.m_audios.Add(source);
            return source;
        }
    }
}
