using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace VRShooting.Scripts.Gun
{
    public abstract class GunBehaviour : MonoBehaviour, IGunObject
    {
        #region 変数

        /// <summary>
        /// 弾の発射位置
        /// </summary>
        [Header("Gun Setting")]
        [SerializeField, Required]
        protected Transform _muzzle;
        /// <summary>
        /// 標準の弾のプールサイズ
        /// </summary>
        [Header("Bullet Object Pool Setting")]
        [SerializeField]
        protected int _defaultPoolSize = 10;
        /// <summary>
        /// 弾のプールサイズの上限。これを超えるオブジェクトが生成された場合、削除される。
        /// </summary>
        [SerializeField]
        protected int _maxPoolSize = 100;
        /// <summary>
        /// 弾のオブジェクトプール
        /// </summary>
        protected ObjectPool<IBullet> bulletPool;
        /// <summary>
        /// 銃のパラメータデータ
        /// </summary>
        protected IGunData gunData;

        #endregion
        
        #region 抽象メソッド
        
        // 弾を発射する処理を記述する
        public abstract void Shoot();
        
        #endregion

        #region 仮想メソッド

        protected virtual void Start()
        {
            TryGetComponent(out gunData);
            CreateBulletObjectPool();
        }

        // 銃のインスタンスをGameObjectとして生成する。
        public virtual IGunObject InstantiateGunObject()
        {
            var gunObject = Instantiate(this.gameObject).GetComponent<IGunObject>();
            return gunObject;
        }
        
        //武器を装備するための処理を記述する
        public virtual void Equipped(Transform handTransform)
        {
            // 手の子オブジェクトにする
            transform.parent = handTransform;
            // ローカルの座標と回転をリセットする。
            transform.SetLocalPositionAndRotation(
                Vector3.zero,
                Quaternion.Euler(Vector3.zero)
            );
        }

        // 装備を解除するための処理
        public virtual void RemoveEquipment()
        {
            // オブジェクトプールの中の弾は残ってしまうので、確実に削除するために実行する。
            // ただし、現在Activeなオブジェクトは削除されない
            bulletPool.Dispose();
            Destroy(this.gameObject);
        }

        /// <summary>
        /// トリガーを離したときに呼び出される処理
        /// </summary>
        public virtual void OnReleaseTrigger()
        {
            
        }
        
        /// <summary>
        /// 弾のオブジェクトプールを生成する。
        /// </summary>
        protected virtual void CreateBulletObjectPool()
        {
            // ObjectPoolの初期設定
            bulletPool = new ObjectPool<IBullet>(
                // オブジェクトが生成されるときの処理
                createFunc: () => gunData.bulletObj.InstantiateBullet(),
                // プールからオブジェクトが取り出されたときの処理
                actionOnGet: target => target.Spawn(_muzzle, bulletPool),
                // プールから破棄されるときに呼び出される
                actionOnDestroy: target => target.DestroyBulletObject(),
                // エディタでのみ実行される設定
                collectionCheck: true,
                // 初期のプールの容量
                defaultCapacity: _defaultPoolSize,
                // プールの最大サイズ
                maxSize: _maxPoolSize
            );
        }
        
        #endregion
    }
}