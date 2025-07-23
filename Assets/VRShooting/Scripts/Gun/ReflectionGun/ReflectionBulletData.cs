using System;
using UnityEngine;
using VRShooting.Scripts.Gun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 反射弾のデータが保存されており、
    /// </summary>
    public class ReflectionBulletData : MonoBehaviour, IBulletData
    {
        public float attackPower => _data.GetFloatValue(ReflectionBulletDataType.Power);
        public float maxSpeed => _data.GetFloatValue(ReflectionBulletDataType.MaxSpeed);
        public float lifeTime => _data.GetFloatValue(ReflectionBulletDataType.LifeTime);
        
        /// <summary>
        /// 反射銃のデータ
        /// </summary>
        [SerializeField] private GenericWeaponData<ReflectionBulletDataType> _data;

        public int GetIntData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletDataType)value;
            // 値を返す
            return _data.GetIntValue(convertedKey);
        }

        public float GetFloatData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletDataType)value;
            // 値を返す
            return _data.GetFloatValue(convertedKey);
        }

        public bool GetBoolData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionBulletDataType)value;
            // 値を返す
            return _data.GetBoolValue(convertedKey);
        }
    }
}