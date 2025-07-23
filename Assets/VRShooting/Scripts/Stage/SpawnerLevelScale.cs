using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using VRShooting.Scripts.Stage.Interfaces;
using Random = UnityEngine.Random;


namespace VRShooting.Scripts.Stage
{
    /// <summary>
    /// 敵をスポーンさせるクラス。
    /// スポーンの実行範囲は、ウェーブごと、またはボーナスタイムと大きい。
    /// </summary>
    public class SpawnerLevelScale : MonoBehaviour, ISpawnLevelScale
    {
        /// <summary>
        /// ウェーブが切り替わった時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnChangeWave => _waveChange;
        private Subject<Unit> _waveChange = new();
        /// <summary>
        /// ウェーブが全て終了した時に発火されるイベント
        /// </summary>
        public IObservable<Unit> OnFinishedFinalWave => _finishedFinalWave;
        private Subject<Unit> _finishedFinalWave = new();

        /// <summary>
        /// 現在のウェーブ数
        /// </summary>
        public int waveCount => _waveCount;

        /// <summary>
        /// ウェーブごとの設定が保存されているリスト
        /// </summary>
        [Header("Wave Setting"), SerializeField]
        private List<WaveSetting> _waveSpawnerSettings;

        /// <summary>
        /// ウェーブ間のインターバルの秒数
        /// </summary>
        [SerializeField, MinValue(0f)]
        private float _waveIntervalSeconds = 5f;

        /// <summary>
        /// ランダムに起動するスポナーの数
        /// </summary>
        [SerializeField, MinValue(0)]
        private int _bonusSpawnerNumber = 3;
        /// <summary>
        /// 現在のウェーブ数
        /// </summary>
        private int _waveCount = 1;

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        private CancellationToken _token;

        /// <summary>
        /// 秒からミリ秒単位に変換するときに使う変数
        /// </summary>
        private const int SECONDS_TO_MILLI_SECONDS = 1000;

        /// <summary>
        /// 通常のゲーム進行を開始する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask StartNormalSpawn(CancellationToken token)
        {
            // 現在のウェーブを表す変数
            _waveCount = 1;

            // ウェーブ数を取得
            var maxWave = _waveSpawnerSettings.Count;

            // ウェーブ間の待機時間を計算
            var waveInterval = (int)_waveIntervalSeconds * SECONDS_TO_MILLI_SECONDS;

            // ウェーブごとにスポーンさせる
            foreach (var waveSetting in _waveSpawnerSettings)
            {
                // ウェーブが終わるまで待機する
                await SpawnOneWave(waveSetting, token);

                // 経過ウェーブをカウント
                _waveCount++;

                // 最終ウェーブであればここで処理を終了
                if (_waveCount >= maxWave) return;

                // ウェーブの変更を通知
                _waveChange.OnNext(Unit.Default);

                // ウェーブ間のインターバル分だけ待機する
                await UniTask.Delay(waveInterval, cancellationToken: token);
            }
        }

        /// <summary>
        /// １ウェーブ分だけスポナーを起動する
        /// </summary>
        /// <param name="waveSetting">該当ウェーブの設定</param>
        /// <param name="token"></param>
        private async UniTask SpawnOneWave(WaveSetting waveSetting, CancellationToken token)
        {
            // スポナーを起動
            foreach (var spawner in waveSetting.spawners)
            {
                //spawner.SpawnAsync(token).Forget();
            }

            // ウェーブが終わるまで待機するタスクを作成
            var waveTime = (int)waveSetting.waveTimeSeconds * SECONDS_TO_MILLI_SECONDS;
            var waveTimeCount = UniTask.Delay(waveTime, cancellationToken: token);

            // ウェーブの制限時間に達するか、敵が全員倒されていればウェーブを終了する
            await UniTask.WhenAny(waveTimeCount, WaitForDestroyAllEnemy(waveSetting, token));

            // カウントをリセット
            EnemyStorage.instance.DestroyAllEnemy();
        }

        /// <summary>
        /// 全ての敵が倒されるまで待機するタスク
        /// </summary>
        /// <param name="waveSetting"></param>
        /// <param name="token"></param>
        private async UniTask WaitForDestroyAllEnemy(WaveSetting waveSetting, CancellationToken token)
        {
            // 全ての敵のスポーンが終わるまで待機する
            await UniTask.WaitUntil(() => IsEndSpawn(waveSetting), cancellationToken: token);

            // 全ての敵が倒されるまで待機する
            await EnemyStorage.instance.WaitForAllEnemyDestroyed(token);
            Debug.Log("Destroyed All Enemy");
        }

        /// <summary>
        /// 現在のウェーブの全てのスポナーがスポーン処理を完了しているかどうかを調べる
        /// </summary>
        /// <param name="waveSetting"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsEndSpawn(WaveSetting waveSetting)
        {
            // 全てのスポーンが終わっているかどうかのフラグ
            var isEndSpawn = true;

            foreach (var spawner in waveSetting.spawners)
            {
                // 既にスポーンが終わっている場合は次のスポナーを調べる
                if (spawner.isEndSpawn) continue;

                // 以下、スポーンが終わっていない場合である前提で処理を進める
                // スポーンが終了していないスポナーがあるのでfalseで処理を中断する
                isEndSpawn = false;
                break;
            }

            // 結果を返す
            return isEndSpawn;
        }

        /// <summary>
        /// ボーナスタイム用のスポーン処理。非同期で実行。
        /// </summary>
        /// <param name="token"></param>
        public async UniTask SpawnBonusTimeAsync(CancellationToken token)
        {
            List<UniTask> spawnTaskList = new ();

            // スポナーの起動をループ
            while (true)
            {
                for (int i = 0; i < _bonusSpawnerNumber; i++)
                {
                    // シード値を設定
                    Random.InitState((int)System.DateTime.Now.Ticks);

                    // ウェーブ数を取得
                    var maxWave = _waveSpawnerSettings.Count;
                    // 指定ウェーブをランダムで選択
                    var waveIndex = Random.Range(0, maxWave);

                    // スポナーの格納数を取得
                    var spawnersSize = _waveSpawnerSettings[waveIndex].spawners.Count;
                    // スポナーを指定するインデックスをランダムで選択
                    var spawnerIndex = Random.Range(0, spawnersSize);

                    // スポーンタスクを保存
                    //spawnTaskList.Add(_waveSpawnerSettings[waveIndex].spawners[spawnerIndex].SpawnAsync(token));
                }

                // スポナーを起動する
                await UniTask.WhenAll(spawnTaskList);

                // 次のスポーンで起動するスポナーを追加
                _bonusSpawnerNumber++;

                // タスクのリストをリセット
                spawnTaskList.Clear();
            }
        }
    }
}