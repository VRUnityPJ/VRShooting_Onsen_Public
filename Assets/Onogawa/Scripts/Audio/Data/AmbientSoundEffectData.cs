using System;
using UnityEngine;
using UnityEngine.Serialization;
using VRShooting.Scripts.Enemy.Drone.DataType;

namespace Onogawa.Scripts.Audio
{
    /// <summary>
    /// 環境音データを保持するクラス
    /// </summary>
    public class AmbientSoundEffectData : MonoBehaviour, IAudioData<AmbientSoundEffectDataType>
    {
        [SerializeField]
        private GenericAudioData<AmbientSoundEffectDataType> _seData;

        public AudioClip GetAudioClip(AmbientSoundEffectDataType key)
        {
            return _seData.GetAudioClip(key);
        }

        public AmbientSoundEffectDataType[] GetAllKeys(AmbientSoundEffectDataType _ = default)
        {
            return _seData.GetAllKeys();
        }
    }
}