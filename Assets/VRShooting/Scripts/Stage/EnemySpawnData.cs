using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Unity.VisualScripting;
using VContainer;
using VContainer.Unity;


namespace VRShooting.Scripts.Stage
{
    /// <summary>
    /// 敵のスポーン位置を設定するクラス
    /// </summary>
    public class EnemySpawnData : MonoBehaviour
    {
        [Inject] private IObjectResolver _resolver;
        /// <summary>
        /// 全てのスポーンが終わったかどうか
        /// </summary>
        public bool isEndSpawn => _isEndSpawn;

        /// <summary>
        /// スポーン設定
        /// </summary>
        [SerializeField, Required]
        private EnemySpawnSetting _spawnSetting;

        /// <summary>
        /// スポーン範囲を表すボックス。
        /// </summary>
        [SerializeField, Required]
        private Transform _spawnRoomBox;

        /// <summary>
        /// スポーンが終わっているかどうかを示すフラグ
        /// </summary>
        private bool _isEndSpawn = false;

        public EnemySpawnSetting SpawnSetting => _spawnSetting;
        public Transform SpawnRoomBox => _spawnRoomBox;

    }
}