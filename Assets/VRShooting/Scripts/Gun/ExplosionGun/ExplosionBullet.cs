using UnityEngine;
using UnityEngine.Pool;
using VRShooting.Scripts.Gun.DataType;

namespace VRShooting.Scripts.Gun
{
    public class ExplosionBullet : BulletBehaviour
    {
        private int _reflectCount = 0;
        private float explosionRadius = 0f;

        protected override void Initialize()
        {
            base.Initialize();
            _reflectCount = 0;
            explosionRadius = bulletData.GetFloatData(ExplosionBulletDataType.ExplosionRadius);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // 敵と衝突したときにオブジェクトプールからリリースする。
            ReleaseWhenHitEnemy(other);
            //さらに他の敵も巻き込むよう爆破
            Explosion();
        }

        protected override void OnCollisionEnter(Collision other)
        {
            var isCantReflection = _reflectCount >= bulletData.GetIntData(ExplosionBulletDataType.MaxReflect); 
            
            // 最大反射回数を超えて衝突した場合は、反射できないので自身をリリースする。
            if (isCantReflection)
            {
                Explosion();//おそらく自身のリリースは銃弾自身の破壊なので、その前に爆破させる
                ReleaseSelf();
                return;
            }
            
            foreach (var contact in other.contacts)
            {
                // 衝突地点の法線ベクトルを取得
                var normal = contact.normal;
                
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

        private void Explosion()
        {
            //爆破を行う処理
            //OverlapSphereの一定範囲内のcolliderを配列に入れ、それぞれ弾に当たったことにする
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider collider in colliders)
            {
                ReleaseWhenHitEnemy(collider);
            }
        }

        protected override void MoveOnFixedUpdate()
        {
            var maxSpeed = bulletData.GetFloatData(ExplosionBulletDataType.MaxSpeed);
            
            // 最大速度に達していたら処理を中断
            if (rigidbody.velocity.magnitude >= maxSpeed) return;
            
            // 動かしたい方向へと移動させる
            var force = moveDirection * maxSpeed;
            rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public override void Spawn(Transform muzzleTransform, ObjectPool<IBullet> bulletPool)
        {
            base.Spawn(muzzleTransform, bulletPool);
            
            // スポーンされる度に反射回数をリセットする。
            _reflectCount = 0;
        }
    }
}