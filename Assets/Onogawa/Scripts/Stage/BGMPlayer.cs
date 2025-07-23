using DG.Tweening;
using UniRx;
using UnitScript;
using UnityEngine;
using VRShooting.Scripts.Stage.Interfaces;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Stage
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private float _initialVolume = 0.1f;
        /// <summary>
        /// 最大ボリューム
        /// </summary>
        [SerializeField] private float _maxVolume = 1.0f;
        /// <summary>
        /// 音量が最大になるまでの時間（秒）
        /// </summary>
        [SerializeField] private float _fadeInDuration = 5;

        [SerializeField] private InterfaceProvider<ILevelStateController> _lscProvider;
        [SerializeField] private AudioClip _bgm;
        private ILevelStateController _lsc;

        private void Start()
        {
            _lsc = _lscProvider.Interface;

            _lsc.OnStartGame
                .Subscribe(_ => PlayBGM())
                .AddTo(this);
        }

        private void PlayBGM()
        {
            var audioSource = AudioPlayer.PlayLoopAudioAtPoint(_bgm, transform.position,_initialVolume);
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, _maxVolume, _fadeInDuration);
        }
    }
}
