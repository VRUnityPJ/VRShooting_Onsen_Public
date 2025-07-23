using System;
using UnityEngine;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 弾のデータを保持するクラス。
    /// 新たにBulletを作り、データが必要になった場合に
    /// 行う作業としては、GunBaseData.csのドキュメントコメントを参照すること。
    /// </summary>
    public class BulletBaseData : MonoBehaviour, IBulletData
    {
        // BulletBaseDataTypeにPowerを定義していないので仮でSpeedにしておく。
        public float attackPower => _weaponData.GetFloatValue(BulletBaseDataType.Speed);
        public float maxSpeed => _weaponData.GetFloatValue(BulletBaseDataType.Speed);
        public float lifeTime => _weaponData.GetFloatValue(BulletBaseDataType.LifeTime);
        
        /// <summary>
        /// BulletBaseDataTypeをキーにしたデータ
        /// </summary>
        [SerializeField] private GenericWeaponData<BulletBaseDataType> _weaponData;
        
        // Intのデータを取得して返す
        public int GetIntData<TKey>(TKey key)
            where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (BulletBaseDataType)value;
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
            var convertedKey = (BulletBaseDataType)value;
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
            var convertedKey = (BulletBaseDataType)value;
            // 値を返す
            return _weaponData.GetBoolValue(convertedKey);
        }
    }
}