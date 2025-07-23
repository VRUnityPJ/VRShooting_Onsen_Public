using System;
using UnityEngine;
using UnityEngine.VFX;
using VRShooting.Scripts.Gun.AutoShotGun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    public class AutoShotGunEffectData : MonoBehaviour, IEffectData
    {
        [SerializeField] private GenericEffectData<AutoShotGunEffectDataType> _effectData;
        public VisualEffect GetVisualEffect<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (AutoShotGunEffectDataType)value;
            // 値を返す
            return _effectData.GetVisualEffect(convertedKey);
        }

        public ParticleSystem GetParticleSystem<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (AutoShotGunEffectDataType)value;
            // 値を返す
            return _effectData.GetParticleEffect(convertedKey);
        }

        public AudioClip GetAudioClip<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (AutoShotGunEffectDataType)value;
            // 値を返す
            return _effectData.GetAudioClip(convertedKey);
        }
    }
}