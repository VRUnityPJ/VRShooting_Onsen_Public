using UnityEngine;
using VRShooting.Scripts.Gun.AutoShotGun.DataType;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun
{
    public class AutoShotGunObject : GunBehaviour
    {
        /// <summary>
        /// 自動発射が有効化どうか
        /// </summary>
        private bool _isAutoShot = false;
        
        /// <summary>
        /// 弾の発射間隔
        /// </summary>
        private float _fireInterval;
        
        /// <summary>
        /// 経過時間をカウントする変数
        /// </summary>
        private float _countTimeSeconds;

        private IEffectData _effectData;

        protected override void Start()
        {
            base.Start();
            
            // エフェクトデータの取得
            TryGetComponent(out _effectData);
            
            // 発射間隔を計算
            _fireInterval = 1f / gunData.GetFloatData(AutoShotGunDataType.FireRate);
        }

        public override void Shoot()
        {
            // 自動発射を有効化
            _isAutoShot = true;
            
            // タイムカウントをリセット
            _countTimeSeconds = 0f;
            
            // 弾を撃つ
            bulletPool.Get();
            
            // 効果音の再生
            var shootSFX = _effectData.GetAudioClip(AutoShotGunEffectDataType.Shoot);
            AudioPlayer.PlayOneShotAudioAtPoint(shootSFX, _muzzle.transform.position, 0.6f);
            
            // 振動
            SendHaptics();
        }

        public override void OnReleaseTrigger()
        {
            // 自動発射を無効化
            _isAutoShot = false;
        }
        
        public void Update()
        {
            // 自動発射が有効でなければ処理を中断
            if (!_isAutoShot) return;
            
            _countTimeSeconds += Time.deltaTime;
            // タイムカウントがインターバルを越えていない場合、処理を中断
            if (_countTimeSeconds < _fireInterval) return;
            
            // タイムカウントをリセット
            _countTimeSeconds = 0.0f;
            
            // 弾を撃つ
            bulletPool.Get();
            
            // 効果音の再生
            var shootSFX = _effectData.GetAudioClip(AutoShotGunEffectDataType.Shoot);
            AudioPlayer.PlayOneShotAudioAtPoint(shootSFX, _muzzle.transform.position, 0.6f);
            
            // 振動
            SendHaptics(true);
        }

        private void SendHaptics(bool isAutoShot = false)
        {
            var hapticsAmplitude = gunData.GetFloatData(AutoShotGunDataType.HapticsAmplitude);
            var hapticsDuration = gunData.GetFloatData(AutoShotGunDataType.HapticsDuration);
            if (isAutoShot)
            {
                hapticsAmplitude *= gunData.GetFloatData(AutoShotGunDataType.AutoShotHapticsAmplitudeDamping);
                hapticsDuration  *= gunData.GetFloatData(AutoShotGunDataType.AutoShotHapticsDurationDamping);
            }
            var controller = PlayerStorage.instance.GetCurrentController();
            controller.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
        }
    }
}
