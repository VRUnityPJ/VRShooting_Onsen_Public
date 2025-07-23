using System;
using UnityEngine;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 新しく銃のデータを作るときに参考にするスクリプト。
    /// 作業としては
    /// １．銃ごとにキーとなるEnum型を作る
    /// ２．作ったEnum型をキーとしてGenericDataを定義する
    /// ３．IGunDataインターフェースを実装する。
    ///     ここは、Intから特定のEnum型への変更とコメントした部分を
    ///     作ったEnum型でキャストするように変更するぐらいで十分
    /// この３つを行うことで新しい銃のデータを作ることができるはず。
    /// </summary>
    public class GunBaseData : MonoBehaviour, IGunData
    {
        public IBullet bulletObj => _bulletBaseObjectObj;
        /// <summary>
        /// 弾のオブジェクト
        /// </summary>
        [SerializeField] private BulletBehaviour _bulletBaseObjectObj;
        /// <summary>
        /// GunBaseDataTypeをキーとするデータ
        /// </summary>
        [SerializeField] private GenericWeaponData<GunBaseDataType> _weaponData;
        
        // Intのデータを取得して返す
        public int GetIntData<TKey>(TKey key)
            where TKey : Enum
        {
            // EnumからIntへ型変更
            var value = Convert.ToInt32(key);
            // Intから特定のEnum型へ変更
            var convertedKey = (GunBaseDataType)value;
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
            var convertedKey = (GunBaseDataType)value;
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
            var convertedKey = (GunBaseDataType)value;
            // 値を返す
            return _weaponData.GetBoolValue(convertedKey);
        }
    }
}

