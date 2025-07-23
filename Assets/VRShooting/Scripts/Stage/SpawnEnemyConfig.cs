using NaughtyAttributes;
using UnitScript;
using UnityEngine;
using UnityEngine.Serialization;
using VRShooting.Scripts.Enemy;
using VRShooting.Scripts.Enemy.Interfaces;

namespace VRShooting.Scripts.Stage
{
    [System.Serializable]
    public class SpawnEnemyConfig
    {
        /// <summary>
        /// スポーンする敵オブジェクト
        /// </summary>
        public GameObject EnemyObject => _enemyObject;
        /// <summary>
        /// スポーンさせる数
        /// </summary>
        public int spawnNumber => _spawnNumber;
        /// <summary>
        /// スポーンし続ける時間。
        /// スポーン間隔はこの変数をスポーン数で割った数値になる。
        /// </summary>
        public float secondDuration => _secondDuration;

        [SerializeField, Required]
        private GameObject _enemyObject;
        [SerializeField, MinValue(1)]
        private int  _spawnNumber;
        [SerializeField, MinValue(0f)]
        private float _secondDuration;
    }
}