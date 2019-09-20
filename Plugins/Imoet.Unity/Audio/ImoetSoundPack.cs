using UnityEngine;
using System.Collections.Generic;
using System;

namespace Imoet.Unity.Audio {
    [Serializable]
    public class ImoetSoundPack
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private List<AudioClip> m_clips;

        public string name { get { return m_name; } }
        public List<AudioClip> audioClips { get { return m_clips; } }

        public AudioClip GetClip(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return null;
            foreach (var clip in m_clips)
            {
                if (clip.name == search || clip.name.Contains(search) ||
                    clip.name.ToLower() == search.ToLower() ||
                    clip.name.ToLower().Contains(search.ToLower()))
                    return clip;
            }
            return null;
        }
    }
}