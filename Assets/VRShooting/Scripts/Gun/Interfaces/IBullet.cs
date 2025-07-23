using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// Bulletオブジェクトのインターフェース
    /// </summary>
    public interface IBullet
    {
        /// <summary>
        /// 自身のインスタンスを生成する。オブジェクトプールのCreateイベントで呼び出すのが主な使い方。
        /// </summary>
        public IBullet InstantiateBullet();
        /// <summary>
        /// オブジェクトプールから有効化されたときに呼び出す
        /// </summary>
        public void Spawn(Transform muzzleTransform, ObjectPool<IBullet> bulletPool);
        /// <summary>
        /// 自身のオブジェクトを破壊する
        /// </summary>
        public void DestroyBulletObject();
    }
}
