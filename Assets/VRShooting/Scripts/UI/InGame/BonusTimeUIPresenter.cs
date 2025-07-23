using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnitScript;
using UnityEngine;
using VRShooting.Scripts.Stage.Interfaces;

namespace VRShooting.Scripts.UI
{
    public class BonusTimeUIPresenter : MonoBehaviour
    {
        [SerializeField, Required] private InterfaceProvider<ILevelStateController> lscProvider;

        private ILevelStateController _levelStateController;

        [SerializeField, Required]
        private TextMeshProUGUI _bonusTimeText;

        /// <summary>
        /// アニメーションが終わったときの大きさの最終値
        /// </summary>
        [SerializeField]
        private float _endScale = 1.2f;

        private Tween _textTween;
        private CancellationToken _token;

        private void Start()
        {
            _levelStateController = lscProvider.Interface;

            _token = this.GetCancellationTokenOnDestroy();

            // イベントのサブスクライブ
            _levelStateController.OnStartBonusTime
                .Subscribe(_ => OnStartBonusTime())
                .AddTo(this);
            _levelStateController.OnEndBonusTime
                .Subscribe(_ => OnEndBonusTime(_token).Forget())
                .AddTo(this);

            // オブジェクトを非表示
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ボーナスタイムが始まった瞬間に呼び出されるテキスト
        /// </summary>
        private void OnStartBonusTime()
        {
            // UIの表示
            gameObject.SetActive(true);

            // テキストアニメーションの再生
            _textTween = _bonusTimeText.DOScale(_endScale, 0.3f).SetEase(Ease.InOutFlash)
                .SetLoops(7, LoopType.Yoyo);
        }

        /// <summary>
        /// ボーナスタイムが終了した瞬間に呼び出されるテキスト
        /// </summary>
        private async UniTask OnEndBonusTime(CancellationToken token)
        {
            // テキストアニメーションを停止
            _textTween.Kill();

            // テキストを非表示
            await _bonusTimeText.DOFade(0f, 1f).ToUniTask(cancellationToken: token);

            // UIを非表示
            gameObject.SetActive(false);
        }
    }
}