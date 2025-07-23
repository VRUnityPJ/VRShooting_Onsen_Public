using System;
using UnityEngine;
using UnityEngine.Pool;
using  VRShooting.Scripts.Enemy.Interfaces;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// 弾のスクリプトが継承するクラス
    /// </summary>
    public abstract class BulletBehaviour : MonoBehaviour, IBullet
    {
        #region 変数

        /// <summary>
        /// 移動方向
        /// </summary>
        protected Vector3 moveDirection;

        /// <summary>
        /// 現在の弾の状態
        /// </summary>
        protected BulletState bulletState;

        /// <summary>
        /// 弾のデータ
        /// </summary>
        protected IBulletData bulletData;

        /// <summary>
        /// 弾の生存時間(LifeTime)をカウントする
        /// </summary>
        protected float timeCount;

        /// <summary>
        /// 自身が格納されているオブジェクトプール
        /// </summary>
        protected ObjectPool<IBullet> objectPool;

        /// <summary>
        /// 弾のRigidBody
        /// </summary>
        protected Rigidbody rigidbody;
        
        /// <summary>
        /// 初期処理が行われているか
        /// </summary>
        protected bool isInitialized = false;
        
        /// <summary>
        /// オブジェクトプールからリリースされているか
        /// </summary>
        protected bool isReleased = false;

        #endregion

        #region 抽象メソッド
        
        protected abstract void OnTriggerEnter(Collider other);

        protected abstract void OnCollisionEnter(Collision other);
        
        /// <summary>
        /// 移動する処理
        /// FixedUpdateメソッドにより呼び出される
        /// </summary>
        protected abstract void MoveOnFixedUpdate();
        
        #endregion
        
        #region 仮想メソッド

        /// <summary>
        /// 変数の初期化を行う
        /// </summary>
        protected virtual void Initialize()
        {
            // 変数の初期化
            TryGetComponent(out bulletData);
            TryGetComponent(out rigidbody);
            timeCount = 0f;
            
            isInitialized = true;
        }

        protected virtual void Update()
        {
            // 生存時間を確認する
            CheckLifeTime();
        }

        protected virtual void FixedUpdate()
        {
            // 移動
            MoveOnFixedUpdate();
        }

        protected virtual void CheckLifeTime()
        {
            // Active化されてからの経過時間をカウント
            timeCount += Time.deltaTime;
            
            // 経過時間が生存時間を超えていたらリリースする
            if (!IsWithinLifeTime())
                ReleaseSelf();
        }

        /// <summary>
        /// 敵と衝突していたら、しかるべき処理をしてオブジェクトプールからリリースする。
        /// </summary>
        protected virtual void ReleaseWhenHitEnemy(Collider other)
        {
            // IDamageableインタフェースの取得を試みる。できなかったら処理を中断する
            if (!other.gameObject.TryGetComponent(out IDamageable damageable)) return;
            
            // 敵にダメージを与える
            damageable.TakeDamage(bulletData.attackPower);
            // オブジェクトプールからリリースする
            ReleaseSelf();
        }
        
        
        /// <summary>
        /// オブジェクトプールからアクティブ化される度に呼び出される。
        /// 独自のカウンタなどのリセットをここで行わないと、２回目以降の呼び出しで動作がおかしくなる可能性がある。
        /// </summary>
        /// <param name="muzzleTransform"></param>
        /// <param name="bulletPool"></param>
        public virtual void Spawn(Transform muzzleTransform, ObjectPool<IBullet> bulletPool)
        {
            if(!isInitialized)
                Initialize();
            // 生存時間のカウントをリセットする
            timeCount = 0f;
            // リリースされたかどうかを表すフラグをリセット
            isReleased = false;
            // オブジェクトをアクティブ化
            gameObject.SetActive(true);
            // スポーン位置に移動
            transform.SetPositionAndRotation(muzzleTransform.position, muzzleTransform.rotation);
            // 移動方向を設定（基本的にforwardかな？）
            moveDirection = muzzleTransform.forward;
            // ObjectPoolを参照
            objectPool = bulletPool;
            rigidbody.velocity = Vector3.zero;
        }

        #endregion

        #region publicメソッド

        public IBullet InstantiateBullet()
        {
            var bullet = Instantiate(gameObject).GetComponent<IBullet>();
            return bullet;
        }

        // 自身のオブジェクトを破壊する
        public void DestroyBulletObject()
        {
            Destroy(gameObject);
        }

        #endregion

        #region protectedメソッド

        /// <summary>
        /// 自身をオブジェクトプールからリリースする。
        /// </summary>
        protected void ReleaseSelf()
        {
            // 既にリリースされていれば処理を中断
            if (isReleased) return;
            
            // リリース処理
            isReleased = true;
            objectPool.Release(this);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 生存時間内であるかどうか
        /// </summary>
        /// <returns></returns>
        protected bool IsWithinLifeTime()
        {
            var lifeTime = bulletData.lifeTime;
            return timeCount <= lifeTime;
        }

        #endregion
    }
}