using System;
using UnityEngine;
using VRShooting.Scripts.Gun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    public class ReflectionGunData : MonoBehaviour, IGunData
    {
        public IBullet bulletObj => _bulletBaseObjectObj;
        /// <summary>
        /// 弾のオブジェクト
        /// </summary>
        [SerializeField] private BulletBehaviour _bulletBaseObjectObj;
        /// <summary>
        /// GunBaseDataTypeをキーとするデータ
        /// </summary>
        [SerializeField] private GenericWeaponData<ReflectionGunDataType> _weaponData;
        
        // Intのデータを取得して返す
        public int GetIntData<TKey>(TKey key)
            where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionGunDataType)value;
            // 値を返す
            return _weaponData.GetIntValue(convertedKey);
        }

        // Floatのデータを取得して返す
        public float GetFloatData<TKey>(TKey key)
            where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionGunDataType)value;
            // 値を返す
            return _weaponData.GetFloatValue(convertedKey);
        }

        // Boolのデータを取得して返す
        public bool GetBoolData<TKey>(TKey key)
            where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (ReflectionGunDataType)value;
            // 値を返す
            return _weaponData.GetBoolValue(convertedKey);
        }
    }
}