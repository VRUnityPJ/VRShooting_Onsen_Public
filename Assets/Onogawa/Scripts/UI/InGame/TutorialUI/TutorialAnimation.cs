using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using UniRx;
using UnitScript;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public class TutorialAnimation : MonoBehaviour,ITutorialAnimation
    {
        [SerializeField] private GameObject _firstElement;
        [SerializeField] private Transform[] _numbersTrans;
        [SerializeField] private Transform _goTrans;
        [SerializeField] private Color _completedColor = Color.green;
        [SerializeField] private int _countDownInterval = 500;
        [SerializeField] private AudioClip _numberPoppedSe;
        [SerializeField] private AudioClip _goPoppedSe;

        public IObservable<Unit> OnStartAnimationProcess => _onStartAnimationProcess;
        public IObservable<Unit> OnEndAnimationProcess => _onEndAnimationProcess;

        private readonly Subject<Unit> _onStartAnimationProcess = new();
        private readonly Subject<Unit> _onEndAnimationProcess = new();

        private ITutorialStateController _tsc;

        private async void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();

            if(!TryGetComponent(out _tsc))
                Debug.Log("TutorialStateControllerが取得できません");

            //チュートリアルが終わったらSequenceを全開放しReadyUIを非アクティブに
            _tsc.OnEndTutorial
                .First()
                .Subscribe(_ =>
                {
                    KillAllSequence();
                    gameObject.SetActive(false);
                });


            // アニメーション開始の通知を待つ
            // 処理が複雑ならSubscribeのほうがいいかも
            await _tsc.OnStartAnimation
                .First()
                .ToUniTask(cancellationToken: token);

            //いらない要素を見えなくする
            _firstElement.SetActive(false);

            //処理の開始を通知
            _onStartAnimationProcess.OnNext(Unit.Default);

            await ShowAnimation(token);

            //処理の終了を通知
            _onEndAnimationProcess.OnNext(Unit.Default);
        }

        /// <summary>
        /// カウントダウンの演出を処理する関数
        /// </summary>
        private async UniTask ShowAnimation(CancellationToken token)
        {
            Debug.Log("チュートリアルアニメーションやるよ");

            //カウントダウン開始
            PopNumberSeq(_numbersTrans[0]).Restart();
            await UniTask.Delay(_countDownInterval, cancellationToken: token);
            PopNumberSeq(_numbersTrans[1]).Restart();
            await UniTask.Delay(_countDownInterval, cancellationToken: token);
            PopNumberSeq(_numbersTrans[2]).Restart();

            await UniTask.Delay(1500, cancellationToken: token);

            //カウントダウンナンバー退場
            SideMovementSequence(_numbersTrans[0]).Restart();
            SideMovementSequence(_numbersTrans[1]).Restart();
            SideMovementSequence(_numbersTrans[2]).Restart();

            await UniTask.Delay(1000, cancellationToken: token);

            //Go!!! UIの登場
            PopGoSeq(_goTrans).Restart();

            await UniTask.Delay(1000, cancellationToken: token);

            //Go!!! UIの退場とその完了を待つ
            var isCompleteExit = false;
            var goExitSeq = SideMovementSequence(_goTrans).OnComplete(()  => isCompleteExit = true);
            goExitSeq.Restart();

            await UniTask.WaitUntil(() => isCompleteExit, cancellationToken: token);

            Debug.Log("チュートリアルアニメーションやったよ");
        }

        /// <summary>
        /// カウントダウンナンバーの出現Sequence
        /// </summary>
        private Sequence PopNumberSeq(Transform trans)
        {
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => trans.gameObject.SetActive(true))
                .Append(trans.DOLocalMoveY(80,0))
                .Append(trans.DOLocalMoveY(-3,1).SetEase(Ease.InOutQuad))
                .AppendCallback(() => AudioPlayer.PlayOneShotAudioAtPoint(_numberPoppedSe,trans.position))
                .Append(trans.DOLocalMoveY(0,0.3f))
                .Join(trans.GetComponent<Text>().DOColor(_completedColor,0.3f))
                .SetAutoKill(false)
                .SetLink(gameObject)
                .SetId(this)
                .Pause();
            return seq;
        }

        /// <summary>
        /// Goテキストの出現Sequence
        /// </summary>
        private Sequence PopGoSeq(Transform trans)
        {
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => trans.gameObject.SetActive(true))
                .AppendCallback(() => AudioPlayer.PlayOneShotAudioAtPoint(_goPoppedSe,trans.position)).SetDelay(0.2f)
                .Append(trans.DOLocalMoveX(-150,0.3f).From().SetEase(Ease.OutBack,0.1f))
                .SetAutoKill(false)
                .SetLink(gameObject)
                .SetId(this)
                .Pause();

            return seq;
        }

        /// <summary>
        /// UIを横に退場させるSequence
        /// </summary>
        private Sequence SideMovementSequence(Transform trans)
        {
            var seq = DOTween.Sequence();
            seq.Append(trans.DOLocalMoveX(150, 0.3f).SetRelative().SetEase(Ease.InBack, 0.2f))
                .SetAutoKill(false)
                .SetLink(gameObject)
                .SetId(this)
                .Pause();

            return seq;
        }

        /// <summary>
        /// Sequenceをすべて解放する
        /// </summary>
        private void KillAllSequence() => DOTween.Kill(this);
    }
}