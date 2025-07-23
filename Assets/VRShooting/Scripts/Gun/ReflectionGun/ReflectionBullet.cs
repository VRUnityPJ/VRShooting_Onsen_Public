using Onogawa.Scripts.Point;
using UnityEngine;
using UnityEngine.Pool;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun.DataType;

namespace VRShooting.Scripts.Gun
{
    public class ReflectionBullet : BulletBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        // [Inject] private PointHolder _pointHolder;
        private IEffectData _effectData;
        private int _reflectCount = 0;

        protected override void Initialize()
        {
            base.Initialize();
            TryGetComponent(out _effectData);
            _reflectCount = 0;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // 敵と衝突したときにオブジェクトプールからリリースする。
            ReleaseWhenHitEnemy(other);
        }

        /// <summary>
        /// 敵と衝突していたら、しかるべき処理をしてオブジェクトプールからリリースする。
        /// </summary>
        protected override void ReleaseWhenHitEnemy(Collider other)
        {
            // IDamageableインタフェースの取得を試みる。できなかったら処理を中断する
            if (!other.gameObject.TryGetComponent(out IDamageable damageable)) return;

            // 敵にダメージを与える
            damageable.TakeDamage(bulletData.attackPower);

            // 反射した後の場合、特別な処理を行う
            if (_reflectCount > 0)
            {
                // ポイントを加算する処理
                var addPoint = bulletData.GetIntData(ReflectionBulletDataType.ReflectPoint);
                //温泉では反射しないのでここコメントアウトします
                // _pointHolder.AddPoint(addPoint);

                // エフェクトの再生
                var vfx = _effectData.GetParticleSystem(ReflectionBulletEffectDataType.ReflectionHit);
                Instantiate(vfx, transform.position, Quaternion.identity);
            }

            // オブジェクトプールからリリースする
            ReleaseSelf();
        }

        protected override void OnCollisionEnter(Collision other)
        {
            // var isCanReflection = _reflectCount <= bulletData.GetIntData(ReflectionBulletDataType.MaxReflect);
            // 反射不可に変更
            var isCanReflection = false;

            // 最大反射回数を超えて衝突した場合は、反射できないので自身をリリースする。
            if (!isCanReflection)
            {
                ReleaseSelf();
                return;
            }

            foreach (var contact in other.contacts)
            {
                // 衝突地点の法線ベクトルを取得
                var normal = contact.normal;

                // エフェクトの再生
                var vfx = _effectData.GetVisualEffect(ReflectionBulletEffectDataType.Reflect);
                Instantiate(vfx, transform.position, Quaternion.identity);

                // 反射させる処理
                Reflection(normal);

                return;
            }
        }

        private void Reflection(Vector3 normal)
        {
            // 移動方向を計算
            moveDirection = Vector3.Reflect(moveDirection, normal).normalized;

            // 反射した方向に弾を移動させる
            var currentSpeed = rigidbody.velocity.magnitude;
            rigidbody.velocity = moveDirection * currentSpeed;

            // 反射回数をカウント
            _reflectCount++;
        }

        protected override void MoveOnFixedUpdate()
        {
            var maxSpeed = bulletData.GetFloatData(ReflectionBulletDataType.MaxSpeed);

            // 最大速度に達していたら処理を中断
            if (rigidbody.velocity.magnitude >= maxSpeed) return;

            // 動かしたい方向へと移動させる
            var force = moveDirection * maxSpeed;
            rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public override void Spawn(Transform muzzleTransform, ObjectPool<IBullet> bulletPool)
        {
            base.Spawn(muzzleTransform, bulletPool);

            // 速度をリセットする
            rigidbody.velocity = Vector3.zero;
            // トレイルエフェクトをクリアする
            _trailRenderer?.Clear();
            // スポーンされる度に反射回数をリセットする。
            _reflectCount = 0;
        }
    }
}