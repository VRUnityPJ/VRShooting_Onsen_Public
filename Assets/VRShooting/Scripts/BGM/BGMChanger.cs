using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VRShooting.Scripts.Stage;
using UniRx;
using UnitScript;
using VRShooting.Scripts.Stage.Interfaces;

namespace VRShooting.Scripts.BGM
{
    public class BGMChanger : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private InterfaceProvider<ILevelStateController> lscProvider;
        private ILevelStateController _levelStateController;
        [SerializeField] private AudioClip _bonusBGM;
        [SerializeField] private float _fadeTimeSeconds;

        private CancellationToken _token;

        private void Start()
        {
            _levelStateController = lscProvider.Interface;
            _token = this.GetCancellationTokenOnDestroy();

            _levelStateController.OnStartBonusTime
                .Subscribe(_ => FadeChangeBGM(_bonusBGM, _audioSource.volume).Forget())
                .AddTo(this);
        }

        private async UniTask FadeChangeBGM(AudioClip changeBGM, float originalVolume)
        {
            await _audioSource.DOFade(0f, _fadeTimeSeconds).ToUniTask(cancellationToken: _token);
            _audioSource.clip = _bonusBGM;
            _audioSource.Play();
            _audioSource.DOFade(originalVolume, _fadeTimeSeconds);
        }
    }
}