using System;
using UnityEngine;
using VRShooting.Scripts.Gun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 反射弾のデータが保存されており、
    /// </summary>
    public class ExplosionBulletData : MonoBehaviour, IBulletData
    {
        public float attackPower => _data.GetFloatValue(ExplosionBulletDataType.Power);
        public float maxSpeed => _data.GetFloatValue(ExplosionBulletDataType.MaxSpeed);
        public float lifeTime => _data.GetFloatValue(ExplosionBulletDataType.LifeTime);
        
        /// <summary>
        /// 反射銃のデータ
        /// </summary>
        [SerializeField] private GenericWeaponData<ExplosionBulletDataType> _data;

        public int GetIntData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ExplosionBulletDataType)value;
            // 値を返す
            return _data.GetIntValue(convertedKey);
        }

        public float GetFloatData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ExplosionBulletDataType)value;
            // 値を返す
            return _data.GetFloatValue(convertedKey);
        }

        public bool GetBoolData<TKey>(TKey key) where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ExplosionBulletDataType)value;
            // 値を返す
            return _data.GetBoolValue(convertedKey);
        }
    }
}