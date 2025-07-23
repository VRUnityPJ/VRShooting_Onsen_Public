using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public class TutorialStateController : MonoBehaviour,ITutorialStateController
    {
        /// <summary>
        /// チュートリアルが始まったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartTutorial => _onStartTutorial;
        /// <summary>
        /// コウモリが破壊されたときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnBatDestroyed => _onBatDestroyed;
        /// <summary>
        /// Animationが始まった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartAnimation => _onStartAnimation;
        /// <summary>
        /// Animationが始まった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndAnimation => _onEndAnimation;
        /// <summary>
        /// チュートリアルが終わった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndTutorial => _onEndTutorial;

        private readonly Subject<Unit> _onStartTutorial = new();
        private readonly Subject<Unit> _onBatDestroyed = new();
        private readonly Subject<Unit> _onStartAnimation = new();
        private readonly Subject<Unit> _onEndAnimation = new();
        private readonly Subject<Unit> _onEndTutorial = new();

        private ITutorialBat _bat;
        private ITutorialAnimation _animation;

        private async void Start()
        {
            //コンポーネントの取得
            _bat = GetComponentInChildren<ITutorialBat>();
            if (_bat == null)
            {
                Debug.LogError("チュートリアルBatが取得できません");
            }

            if(!TryGetComponent(out _animation))
            {
                Debug.LogError("チュートリアルAnimationが取得できません");
            }

            TutorialFlow().Forget();
        }

        /// <summary>
        /// チュートリアルを進める関数
        /// </summary>
        private async UniTask TutorialFlow()
        {
            var token = this.GetCancellationTokenOnDestroy();
            //チュートリアル開始
            _onStartTutorial.OnNext(Unit.Default);

            //コウモリの破壊を待つ
            await WaitBatDestroyed(token);

            //アニメーションの開始
            _onStartAnimation.OnNext(Unit.Default);

            //アニメーションを待つ
            await WaitAnimation(token);

            //チュートリアル終了
            _onEndTutorial.OnNext(Unit.Default);
        }

        /// <summary>
        /// コウモリが撃たれるのを待つ関数
        /// </summary>
        private async UniTask WaitBatDestroyed(CancellationToken token)
        {
            await _bat.OnDead
                .First()
                .ToUniTask(cancellationToken:token);

            _onBatDestroyed.OnNext(Unit.Default);
        }

        /// <summary>
        /// アニメーション処理を待つ関数
        /// </summary>
        private async UniTask WaitAnimation(CancellationToken token)
        {
            //終了を待つ
            await _animation.OnEndAnimationProcess
                .First()
                .ToUniTask(cancellationToken: token);

            //全体に通知
            _onEndAnimation.OnNext(Unit.Default);
        }
    }
}