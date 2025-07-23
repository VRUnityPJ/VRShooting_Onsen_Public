using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Enemy.Bat
{
    public class BatSoundController : MonoBehaviour
    {
        private IBatStateController _batStateController;
        private IEffectData _effectData;

        private void Start()
        {
            if(!TryGetComponent(out _batStateController))
                Debug.Log("BatStateControllerが取得できません");
            if(!TryGetComponent(out _effectData))
                Debug.Log("EffectDataが取得できません");

            _batStateController.OnEnterHold += PlayOnHoldSound;
        }

        private void PlayOnHoldSound()
        {
            // サウンドの取得
            var spawnSFX = _effectData.GetAudioClip(BatEffectDataType.Hold);
            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(spawnSFX, transform.position);
        }
    }
}