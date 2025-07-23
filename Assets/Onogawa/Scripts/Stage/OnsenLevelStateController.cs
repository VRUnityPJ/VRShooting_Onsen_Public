using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using VRShooting.Scripts.Stage.Interfaces;

namespace Onogawa.Scripts.Stage
{
    /// <summary>
    /// ゲームループを回しているクラス。 SpawnLevelScaleを利用して敵をスポーンさせている。
    /// ゲームの進行状況に応じて、イベントを発行している。
    /// </summary>
    public class OnsenLevelStateController : MonoBehaviour, ILevelStateController
    {
        /// <summary>
        /// 通常のスポーン処理が開始したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartNormalSpawnTime => _onStartNormalSpawnTime;

        /// <summary>
        /// 通常のスポーン処理が終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndNormalSpawnTime => _onEndNormalSpawnTime;

        /// <summary>
        /// ボーナスタイムが開始したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartBonusTime => _onStartBonusTime;

        /// <summary>
        /// ボーナスタイムが終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndBonusTime => _onEndBonusTime;

        /// <summary>
        /// ゲーム開始までの遅延が終了して、ゲームが開始されたときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnStartGame => _onStartGame;

        /// <summary>
        /// ゲームが終了したときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnEndGame => _onEndGame;

        /// <summary>
        /// 通常のスポーン処理が開始したことを通知する
        /// </summary>
        private Subject<Unit> _onStartNormalSpawnTime = new();

        /// <summary>
        /// 通常のスポーン処理が終了したことを通知する
        /// </summary>
        private Subject<Unit> _onEndNormalSpawnTime = new();

        /// <summary>
        /// ボーナスタイムが開始したことを通知する
        /// </summary>
        private Subject<Unit> _onStartBonusTime = new();

        /// <summary>
        /// ボーナスタイムが終了したことを通知する
        /// </summary>
        private Subject<Unit> _onEndBonusTime = new();

        /// <summary>
        /// ゲームが開始したことを通知する
        /// </summary>
        private Subject<Unit> _onStartGame = new();

        /// <summary>
        /// ゲームが終了したことを通知する
        /// </summary>
        private Subject<Unit> _onEndGame = new();

        /// <summary>
        /// ステージの制限時間
        /// </summary>
        [SerializeField, MinValue(1f)]
        private float _stagePlayTime;

        /// <summary>
        /// ゲーム開始までの遅延時間
        /// </summary>
        [SerializeField, MinValue(0f)]
        private float _gameStartDelaySeconds = 10f;

        /// <summary>
        /// レベル単位のスポナー
        /// </summary>
        private ISpawnLevelScale _levelSpawner;

        /// <summary>
        /// ゲームを開始してからの経過時間
        /// </summary>
        private float _timeCount;

        /// <summary>
        /// 経過時間のカウントを開始しているかどうか
        /// </summary>
        private bool _isStartTimeCount = false;

        /// <summary>
        /// 遅延の最小値
        /// </summary>
        private const float MIN_DELAY_TIME = 0f;

        /// <summary>
        /// 秒からミリ秒単位に変換したいときに使う変数
        /// </summary>
        private const int SECONDS_TO_MILLI_SECONDS = 1000;

        private CancellationToken _token;

        private void Start()
        {
            // 変数の初期処理
            TryGetComponent(out _levelSpawner);
            _timeCount = 0f;
            _isStartTimeCount = false;
            _token = this.GetCancellationTokenOnDestroy();

        }

        /// <summary>
        /// Playerの準備が完了したときに呼ばれる関数
        /// </summary>
        public async UniTaskVoid OnPlayerReady()
        {
            // ゲーム開始までの遅延をかける
            if (_gameStartDelaySeconds > MIN_DELAY_TIME)
            {
                var startDelay = Mathf.Abs((int)_gameStartDelaySeconds * SECONDS_TO_MILLI_SECONDS);
                await UniTask.Delay(startDelay, cancellationToken: _token);
            }

            // ゲーム開始を通知
            _onStartGame.OnNext(Unit.Default);

            // 通常のゲーム進行を実行
            await StartNormalSpawnTime();

            // ゲーム終了を通知
            _onEndGame.OnNext(Unit.Default);
        }

        private void Update()
        {
            // タイムカウント
            if(_isStartTimeCount)
                _timeCount += Time.deltaTime;
        }

        /// <summary>
        /// 通常のゲーム進行を開始する
        /// </summary>
        private async UniTask StartNormalSpawnTime()
        {
            // スポーンの開始を通知
            _onStartNormalSpawnTime.OnNext(Unit.Default);

            // 経過時間のカウントを開始
            _isStartTimeCount = true;

            // スポナーを起動
            await _levelSpawner.StartNormalSpawn(_token);

            // スポーンの終了を通知
            _onEndNormalSpawnTime.OnNext(Unit.Default);
        }

        public int GetTime()
        {
            return (int)(_stagePlayTime-_timeCount);
        }

        //TODO: デバッグ用、後で消す
        [Button]
        private void DebugOnEnd() => _onEndGame.OnNext(Unit.Default);
    }
}