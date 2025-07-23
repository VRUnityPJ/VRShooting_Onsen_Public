using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VRShooting.Scripts.Enemy;

namespace VRShooting.Scripts.Stage
{
    /// <summary>
    /// ステージ上に存在している敵の情報を保持する。
    /// シングルトンで利用可能。
    /// </summary>
    public class EnemyStorage : MonoBehaviour
    {
        /// <summary>
        /// EnemyCountのシングルトン
        /// </summary>
        public static EnemyStorage instance;
        
        /// <summary>
        /// 敵のリスト
        /// </summary>
        private List<DroneObject> _enemyList = new ();

        /// <summary>
        /// ウェーブがスタートしていることを示すフラグ
        /// </summary>
        private bool _isStartWave = false;

        private void Awake()
        {
            // DontDestroyには入れないシングルトン
            #region Singleton

            // シングルトンの作成
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            #endregion
        }

        private void OnDestroy()
        {
            // シングルトンの破棄
            if (instance == this)
                instance = null;
        }

        /// <summary>
        /// 敵のリストに加える
        /// </summary>
        /// <param name="drone"></param>
        public void Add(DroneObject drone)
        {
            _enemyList.Add(drone);
            
            // フラグを立てる
            if (!_isStartWave)
                _isStartWave = true;
        }

        /// <summary>
        /// 敵のリストから削除する
        /// </summary>
        /// <param name="drone"></param>
        public void Remove(DroneObject drone)
        {
            _enemyList.Remove(drone);
        }

        /// <summary>
        /// 全ての敵オブジェクトを破壊する。
        /// （EnemyCountの責務から外れてる気がするけど仕方なし）
        /// </summary>
        public void DestroyAllEnemy()
        {
            // 全ての敵オブジェクトを削除
            foreach (var enemyObj in _enemyList)
            {
                Destroy(enemyObj.gameObject);
            }
            
            // リストを初期状態に戻す
            _enemyList.Clear();
            
            // 全ての敵オブジェクトを破壊することはウェーブが終わるということなのでFalseにする
            _isStartWave = false;
        }

        /// <summary>
        /// 現在ステージ上に存在している敵の数を取得する。
        /// </summary>
        /// <returns></returns>
        public int GetEnemyCount()
        {
            return _enemyList.Count;
        }

        /// <summary>
        /// ウェーブの全ての敵が倒されるまで待機する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UniTask WaitForAllEnemyDestroyed(CancellationToken token)
        {
            var isAllEnemyDestroyed = (_enemyList.Count == 0) && _isStartWave;
            return UniTask.WaitUntil(() => isAllEnemyDestroyed, cancellationToken: token);
        }
    }
}
