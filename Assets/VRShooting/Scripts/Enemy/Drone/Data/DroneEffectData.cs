using System;
using UnityEngine;
using UnityEngine.VFX;
using VRShooting.Scripts.Enemy.Drone.DataType;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Enemy.Drone.Data
{
    /// <summary>
    /// Droneのエフェクトデータを取り出す
    /// </summary>
    public class DroneEffectData : MonoBehaviour, IEffectData
    {
        [SerializeField]
        private GenericEffectData<DroneEnemyEffectDataType> _effectData;
        
        public VisualEffect GetVisualEffect<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (DroneEnemyEffectDataType)value;
            // 値を返す
            return _effectData.GetVisualEffect(convertedKey);
        }

        public ParticleSystem GetParticleSystem<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (DroneEnemyEffectDataType)value;
            // 値を返す
            return _effectData.GetParticleEffect(convertedKey);
        }

        public AudioClip GetAudioClip<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (DroneEnemyEffectDataType)value;
            // 値を返す
            return _effectData.GetAudioClip(convertedKey);
        }
    }
}