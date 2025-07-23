using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace VRShooting.Scripts.Stage.Interfaces
{
    public interface ILevelStateController
    {
        /// <summary>
        /// 通常のスポーン処理が開始したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartNormalSpawnTime { get; }

        /// <summary>
        /// 通常のスポーン処理が終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndNormalSpawnTime { get; }

        /// <summary>
        /// ボーナスタイムが開始したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartBonusTime { get; }

        /// <summary>
        /// ボーナスタイムが終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndBonusTime { get; }

        /// <summary>
        /// ゲーム開始までの遅延が終了して、ゲームが開始されたときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartGame { get; }

        /// <summary>
        /// ゲームが終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndGame { get; }

        public int GetTime();
        public UniTaskVoid OnPlayerReady();
    }
}
