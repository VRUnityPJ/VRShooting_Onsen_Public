using UnityEngine;

namespace VRShooting.Scripts.Gun
{
    public interface IGunObject
    {
        /// <summary>
        /// 弾を発射する
        /// </summary>
        public void Shoot();

        /// <summary>
        /// トリガーボタンが離されたときの処理
        /// </summary>
        public void OnReleaseTrigger();
        
        /// <summary>
        /// 武器を装備する
        /// </summary>
        /// <param name="handTransform"></param>
        public void Equipped(Transform handTransform);
        
        /// <summary>
        /// 銃のGameObjectを生成する。
        /// UnityのInstantiateを使用する
        /// </summary>
        /// <returns></returns>
        public IGunObject InstantiateGunObject();
        
        /// <summary>
        /// 武器の装備を解除する
        /// </summary>
        public void RemoveEquipment();
    }
}
