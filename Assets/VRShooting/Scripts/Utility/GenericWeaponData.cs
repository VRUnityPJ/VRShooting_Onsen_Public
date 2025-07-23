using System;
using UnityEngine;

namespace VRShooting.Scripts.Utility
{
    /// <summary>
    /// 任意のキーと値を登録でき、取り出せるクラス
    /// </summary>
    /// <typeparam name="TKey">キーにしたい任意のEnum型</typeparam>
    [Serializable]
    public class GenericWeaponData<TKey>
        where TKey : Enum
    {
        /// <summary>
        /// Intのデータ
        /// </summary>
        [SerializeField] private GenericData<TKey, int> _intData;

        /// <summary>
        /// Floatのデータ
        /// </summary>
        [SerializeField] private GenericData<TKey, float> _floatData;

        /// <summary>
        /// Boolのデータ
        /// </summary>
        [SerializeField] private GenericData<TKey, bool> _boolData;

        /// <summary>
        /// IntParametersに登録されている値を取得する
        /// </summary>
        /// <param name="key">値に関連付けられているキー</param>
        public int GetIntValue(TKey key)
        {
            return _intData.GetValue(key);
        }

        /// <summary>
        /// FloatParametersに登録されている値を取得する
        /// </summary>
        /// <param name="key">値に関連付けられているキー</param>
        public float GetFloatValue(TKey key)
        {
            return _floatData.GetValue(key);
        }

        /// <summary>
        /// BoolParametersに登録されている値を取得する
        /// </summary>
        /// <param name="key">値に関連付けられているキー</param>
        public bool GetBoolValue(TKey key)
        {
            return _boolData.GetValue(key);
        }
    }
}