
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Stage.Interfaces;
using UniRx;
using Random = UnityEngine.Random;

namespace Onogawa.Scripts.Stage
{
    public sealed class EnemySpawner : IDisposable
    {
        [Inject] private IObjectResolver _resolver;
        private ILevelStateController _lsc;

        /// <summary>
        /// 全てのスポーンが終わったかどうか
        /// </summary>
        public bool isEndSpawn => _isEndSpawn;

        /// <summary>
        /// スポーンが終わっているかどうかを示すフラグ
        /// </summary>
        private bool _isEndSpawn = false;

        /// <summary>
        /// ゲーム終了時に破棄するためのリスト
        /// </summary>
        private readonly List<GameObject> _releaseObjList = new List<GameObject>();
        private readonly IDisposable _disposable;

        public EnemySpawner(ILevelStateController lsc)
        {
            _lsc = lsc;
            _disposable = _lsc.OnEndGame
                .Subscribe(_ => ReleaseEnemyObjects());
        }

        /// <summary>
        /// 敵をスポーンさせる
        /// </summary>
        /// <param name="enemySpawnData"></param>
        /// <param name="token"></param>
        public async UniTask SpawnAsync(EnemySpawnData enemySpawnData, CancellationToken token)
        {
            _isEndSpawn = false;

            // スポーンボックスの回転を取得
            var spawnBoxQuaternion = enemySpawnData.SpawnRoomBox.transform.rotation;

            // スポーンする範囲を計算
            var spawnBoxScale = enemySpawnData.SpawnRoomBox.localScale;
            var spawnHalfScaleX = Mathf.Abs(spawnBoxScale.x / 2f);
            var spawnHalfScaleY = Mathf.Abs(spawnBoxScale.y / 2f);
            var spawnHalfScaleZ = Mathf.Abs(spawnBoxScale.z / 2f);

            foreach (var spawnEnemyConfig in enemySpawnData.SpawnSetting.enemyList)
            {
                // スポーンする数
                var spawnNumber = spawnEnemyConfig.spawnNumber;

                // スポーン間隔を計算
                var spawnInterval = (int)(spawnEnemyConfig.secondDuration / spawnNumber * 1000);

                for (int i = 0; i < spawnNumber; i++)
                {
                    // スポーンする座標を計算
                    var dx = Random.Range(-spawnHalfScaleX, spawnHalfScaleX);
                    var dy = Random.Range(-spawnHalfScaleY, spawnHalfScaleY);
                    var dz = Random.Range(-spawnHalfScaleZ, spawnHalfScaleZ);
                    var spawnPositionDelta = spawnBoxQuaternion * new Vector3(dx, dy, dz);
                    var spawnPosition = spawnPositionDelta + enemySpawnData.gameObject.transform.position;

                    // 敵オブジェクトをスポーン
                    var spawnedObject = _resolver.Instantiate(spawnEnemyConfig.EnemyObject);
                    //リストに格納
                    _releaseObjList.Add(spawnedObject);
                    spawnedObject.transform.position = spawnPosition;

                    // インターバル分だけ待機
                    // UniTask.Delay( 待機したい時間をミリ秒単位で指定, キャンセルトークン )
                    await UniTask.Delay(spawnInterval, cancellationToken: token);
                }
            }
            _isEndSpawn = true;
        }

        /// <summary>
        /// 生成した敵オブジェクトにnullを格納する
        /// </summary>
        private void ReleaseEnemyObjects()
        {
            for (int i = 0; i < _releaseObjList.Count; i++)
            {
                _releaseObjList[i] = null;
            }
        }

        public void Dispose() => _disposable.Dispose();
    }
}