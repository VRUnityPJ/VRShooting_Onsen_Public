using System;
using UnityEngine;
using UnityEngine.VFX;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Audio
{
    /// <summary>
    /// AudioDataを保存し、取り出せるクラス
    /// </summary>
    [Serializable]
    public class GenericAudioData<TKey>
        where TKey : Enum
    {
        // AudioClip のデータ
        [SerializeField] private GenericData<TKey, AudioClip> _audioData;

        /// <summary>
        /// AudioDataに登録されている値を取得する
        /// </summary>
        /// <param name="key">値に関連付けられているキー</param>
        public AudioClip GetAudioClip(TKey key)
        {
            return _audioData.GetValue(key);
        }

        public TKey[] GetAllKeys()
        {
            return _audioData.GetAllKeys();
        }
    }
}