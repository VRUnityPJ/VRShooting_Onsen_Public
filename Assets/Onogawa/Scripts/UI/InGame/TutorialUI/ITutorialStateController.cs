using System;
using UniRx;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public interface ITutorialStateController
    {
        /// <summary>
        /// チュートリアルが始まったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartTutorial { get; }
        /// <summary>
        /// コウモリが破壊されたときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnBatDestroyed { get; }
        /// <summary>
        /// Animationが始まった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartAnimation { get; }
        /// <summary>
        /// Animationが始まった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndAnimation { get; }
        /// <summary>
        /// チュートリアルが終わった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndTutorial { get; }
    }
}