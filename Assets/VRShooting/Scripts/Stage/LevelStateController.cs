using System;
using System.Threading;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using VRShooting.Scripts.Stage.Interfaces;

namespace VRShooting.Scripts.Stage
{
    /// <summary>
    /// ゲームループを回しているクラス。 SpawnLevelScaleを利用して敵をスポーンさせている。
    /// ゲームの進行状況に応じて、イベントを発行している。
    /// </summary>
    public class LevelStateController : MonoBehaviour, ILevelStateController
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

        private async UniTaskVoid Start()
        {
            // 変数の初期処理
            TryGetComponent(out _levelSpawner);
            _timeCount = 0f;
            _isStartTimeCount = false;
            _token = this.GetCancellationTokenOnDestroy();

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

            // ボーナスタイムをやる時間があったら実行
            if (_timeCount < _stagePlayTime)
            {
                var bonusTime = _stagePlayTime - _timeCount;
                await StartBonusTime(bonusTime, _token);
            }

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

        /// <summary>
        /// ゲーム終了時、時間が余ったらボーナスタイムに移行する
        /// </summary>
        private async UniTask StartBonusTime(float bonusTime, CancellationToken token)
        {
            // CancellationTokenSourceの生成
            var bonusWaveCts = new CancellationTokenSource();

            // スポーンの開始を通知
            _onStartBonusTime.OnNext(Unit.Default);

            // ****ボーナスタイムの開始****
            // ボーナスタイムのタイムカウントを開始
            CountBonusTime(bonusTime, bonusWaveCts, token).Forget();

            // ボーナスタイムのスポーン開始
            try
            {
                await _levelSpawner.SpawnBonusTimeAsync(bonusWaveCts.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log($"ボーナスタイムのスポーン処理が終了しました. {e.Message}");
            }

            // 敵の消去
            EnemyStorage.instance.DestroyAllEnemy();

            // ボーナスタイムの終了を通知
            _onEndBonusTime.OnNext(Unit.Default);

            // 経過時間のカウントを終了
            _isStartTimeCount = false;
        }

        /// <summary>
        /// ボーナスタイムのカウントを開始する。
        /// ボーナスタイムが終了するとctsで渡したCancellationTokenSourceのCancelを実行する。
        /// </summary>
        /// <param name="bonusTimeSeconds"></param>
        /// <param name="cts"></param>
        /// <param name="token"></param>
        private async UniTask CountBonusTime(float bonusTimeSeconds, CancellationTokenSource cts, CancellationToken token)
        {
            // ボーナスタイムをミリ秒単位に変換
            var bonusTime = (int)(bonusTimeSeconds * SECONDS_TO_MILLI_SECONDS);

            // ボーナスタイム分だけ待機
            await UniTask.Delay(bonusTime, cancellationToken: token);

            // キャンセルを実行する
            cts.Cancel();
        }

        public int GetTime()
        {
            return (int)(_stagePlayTime-_timeCount);
        }

        public async UniTaskVoid OnPlayerReady() { }
    }
}