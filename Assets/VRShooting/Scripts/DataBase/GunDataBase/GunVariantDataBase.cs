using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.DataBase.GunDataBase
{
    [CreateAssetMenu(menuName = "Gun/DataBase")]
    public class GunVariantDataBase : ScriptableObject
    {
        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] private List<GunVariantData> _gunVariantDataBase;

        // ERROR Message
        private const string ERROR_MESSAGE_NOT_CONTAINS_KEY = "その種類の銃はデータベースに登録されていません。 Variant : ";

        /// <summary>
        /// キーに関連付けられている銃をデータベースから探して取得する。
        /// </summary>
        /// <param name="variantKey">キーとなる値</param>
        /// <returns>銃のオブジェクトを扱うためのインターフェース</returns>
        /// <exception cref="InvalidEnumArgumentException">キーに対応するデータが存在しないときにスローされる</exception>
        public IGunObject GetGunObject(GunVariant variantKey)
        {
            // キーに関連付けられているデータを探して返す。
            foreach (var data in _gunVariantDataBase)
            {
                if (data.VariantKey == variantKey)
                    return data.GunObject;
            }

            // ここに到達した時点で該当するデータがないことを意味するので例外をスローする
            throw new InvalidEnumArgumentException(ERROR_MESSAGE_NOT_CONTAINS_KEY + variantKey);
        }
    }
}