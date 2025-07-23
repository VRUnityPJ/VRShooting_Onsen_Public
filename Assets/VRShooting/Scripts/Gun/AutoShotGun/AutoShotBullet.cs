using UnityEngine;
using UnityEngine.Pool;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun.AutoShotGun.DataType;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    /// <summary>
    /// AutoShotBulletは通常弾なのでここはほぼコピペ
    /// </summary>
    public class AutoShotBullet : BulletBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        private IEffectData _effectData;

        protected override void Initialize()
        {
            base.Initialize();
            TryGetComponent(out _effectData);
        }

        public override void Spawn(Transform muzzleTransform, ObjectPool<IBullet> bulletPool)
        {
            base.Spawn(muzzleTransform, bulletPool);
            // Trailエフェクトの消去
            _trailRenderer.Clear();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // 敵と衝突したときにオブジェクトプールからリリースする。
            ReleaseWhenHitEnemy(other);
        }

        protected override void ReleaseWhenHitEnemy(Collider other)
        {
            // IDamageableインタフェースの取得を試みる。できなかったら処理を中断する
            if (!other.gameObject.TryGetComponent(out IDamageable damageable)) return;
            
            // 敵にダメージを与える
            damageable.TakeDamage(bulletData.attackPower);
            
            // Play Hit VFX
            var hitVFX = _effectData.GetParticleSystem(AutoShotBulletEffectDataType.HitVFX);
            Instantiate(hitVFX.gameObject, transform.position, Quaternion.identity);
            
            // Play Hit Sound
            var hitSFX = _effectData.GetAudioClip(AutoShotBulletEffectDataType.HitSound);
            AudioPlayer.PlayOneShotAudioAtPoint(hitSFX, transform.position);
            
            // オブジェクトプールからリリースする
            ReleaseSelf();
        }

        protected override void OnCollisionEnter(Collision other)
        {
            ReleaseSelf();
        }

        protected override void MoveOnFixedUpdate()
        {
            var maxSpeed = bulletData.maxSpeed;
            
            // 最大速度に達していたら処理を中断
            if (rigidbody.velocity.magnitude >= maxSpeed) return;
            
            // 動かしたい方向へと移動させる
            var force = moveDirection * maxSpeed;
            rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
