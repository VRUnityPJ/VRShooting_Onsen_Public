using System;
using UnityEngine;

namespace Onogawa.Scripts.Audio
{
    public interface IAudioData<TKey> where TKey : Enum
    {
        public AudioClip GetAudioClip(TKey key);
        public TKey[] GetAllKeys(TKey key = default);
    }
}