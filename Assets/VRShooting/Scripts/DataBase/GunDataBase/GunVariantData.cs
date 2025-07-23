using System;
using UnityEngine;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.DataBase.GunDataBase
{
    /// <summary>
    /// 銃のオブジェクトとGunVariantを関連付けるクラス
    /// </summary>
    [Serializable]
    public class GunVariantData
    {
        /// <summary>
        /// 銃の種類
        /// </summary>
        public GunVariant VariantKey => _variantKey;

        /// <summary>
        /// 銃のオブジェクト
        /// </summary>
        public IGunObject GunObject => _gunObject;

        [SerializeField] private GunVariant _variantKey;
        [SerializeField] private GunBehaviour _gunObject;
    }
}