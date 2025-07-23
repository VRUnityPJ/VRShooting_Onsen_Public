using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnitScript;
using UnityEngine;
using UnityEngine.InputSystem;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Stage.Interfaces;

namespace VRShooting.Scripts.UI
{
    public class WaveUIPresenter : MonoBehaviour
    {
        [SerializeField] private InterfaceProvider<ISpawnLevelScale> spawnScaleProvider;
        [SerializeField, Required] private TextMeshProUGUI _waveText;
        [SerializeField, Required] private AudioSource _audioSource;

        [Header("Animation Setting")]
        [SerializeField] private float _startScale = 0f;
        [SerializeField] private float _endScale = 1f;
        [SerializeField] private float _scaleAnimationTime = 1f;
        [SerializeField] private float _intervalTime = 1f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip _openAudio;
        [SerializeField] private AudioClip _closeAudio;

        private ISpawnLevelScale _spawnerLevelScale;

        /// <summary>
        /// 自身のRectTransform
        /// </summary>
        private RectTransform _rect;

        /// <summary>
        /// 初期化してあるかどうか
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// アニメーションを再生中かどうか
        /// </summary>
        private bool _isPlayingAnimation = false;

        private CancellationToken _token;

        private void Start()
        {
            _spawnerLevelScale = spawnScaleProvider.Interface;
            _token = this.GetCancellationTokenOnDestroy();

            // 最初は自分で実行する
            OnChangeWave();

            // イベントをサブスクライブ
            _spawnerLevelScale.OnChangeWave
                .Subscribe(_ => OnChangeWave())
                .AddTo(this);
        }

        private void OnChangeWave()
        {
            _waveText.text = $"Wave {_spawnerLevelScale.waveCount}";
            PlayAnimation(_token);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                PlayAnimation(_token).Forget();
            }
#endif
        }

        private async UniTask PlayAnimation(CancellationToken token)
        {
            // 初期化してなければ初期化する
            if(!_isInitialized)
                Initialize();

            // アニメーションが既に再生中であれば再生しない
            if (_isPlayingAnimation) return;

            _isPlayingAnimation = true;

            // UIの状態を最初の状態に戻す
            _rect.localScale = new Vector3(_startScale, _startScale + 0.1f, 1f);
            _waveText.alpha = 0f;

            // メニューを開く音を再生
            _audioSource.PlayOneShot(_openAudio);

            // スケール変更のアニメーション
            await _rect.DOScaleX(_endScale, _scaleAnimationTime / 2f)
                .SetEase(Ease.OutCirc).ToUniTask(cancellationToken: token);
            await _rect.DOScaleY(_endScale, _scaleAnimationTime / 2f)
                .SetEase(Ease.OutCirc).ToUniTask(cancellationToken: token);

            // テキストを点滅
            await _waveText.DOFade(1f, 0.2f)
                .SetLoops(3, LoopType.Yoyo).ToUniTask(cancellationToken: token);

            // テキストを読ませる時間を作る
            await UniTask.Delay((int)(_intervalTime * 1000), cancellationToken: token);

            // 閉じるときの音を再生
            _audioSource.PlayOneShot(_closeAudio);

            // テキストを隠す
            _waveText.DOFade(0f, 0.2f);

            // クローズ
            await _rect.DOScaleY(_startScale + 0.1f, _scaleAnimationTime / 2f)
                .SetEase(Ease.OutCirc).ToUniTask(cancellationToken: token);
            await _rect.DOScaleX(_startScale, _scaleAnimationTime / 2f)
                .SetEase(Ease.OutCirc).ToUniTask(cancellationToken: token);

            _isPlayingAnimation = false;
        }

        private void Initialize()
        {
            _rect = GetComponent<RectTransform>();
            _token = this.GetCancellationTokenOnDestroy();
            _isInitialized = true;
        }
    }
}