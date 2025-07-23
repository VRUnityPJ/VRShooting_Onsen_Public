using UnityEngine;
using VRShooting.Scripts.Gun.DataType;
using VRShooting.Scripts.Stage;

namespace VRShooting.Scripts.Gun
{
    public class ReflectionGunObject : GunBehaviour
    {
        private IEffectData _effectData;
        protected override void Start()
        {
            base.Start();
            TryGetComponent(out _effectData);
        }

        public override void Shoot()
        {
            // オブジェクトプールから弾を取り出す
            // 銃側のスクリプトはこれだけで十分機能を果たす
            bulletPool.Get();
            
            // コントローラーに振動を与える
            var hapticsAmplitude = gunData.GetFloatData(ReflectionGunDataType.HapticsAmplitude);
            var hapticsDuration  = gunData.GetFloatData(ReflectionGunDataType.HapticsDuration);
            var controller = PlayerStorage.instance.GetCurrentController();
            controller.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
            
            // 発射音を再生
            var clip = _effectData.GetAudioClip(ReflectionGunEffectDataType.Shoot);
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}