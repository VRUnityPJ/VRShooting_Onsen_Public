using System;
using UnityEngine;
using UnityEngine.VFX;
using VRShooting.Scripts.Gun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    public class ReflectionBulletEffectData : MonoBehaviour, IEffectData
    {
        [SerializeField] private GenericEffectData<ReflectionBulletEffectDataType> _effectData;
        
        public VisualEffect GetVisualEffect<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletEffectDataType)value;
            // 値を返す
            return _effectData.GetVisualEffect(convertedKey);
        }

        public ParticleSystem GetParticleSystem<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletEffectDataType)value;
            // 値を返す
            return _effectData.GetParticleEffect(convertedKey);
        }

        public AudioClip GetAudioClip<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletEffectDataType)value;
            // 値を返す
            return _effectData.GetAudioClip(convertedKey);
        }
    }
}