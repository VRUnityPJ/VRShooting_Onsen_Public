using System.Collections.Generic;
using UnityEngine;

namespace VRShooting.Scripts.Stage
{
    [CreateAssetMenu(fileName = "EnemySpawnSetting", menuName = "CreateEnemySpawnSetting")]
    public class EnemySpawnSetting : ScriptableObject
    {
        public List<SpawnEnemyConfig> enemyList => _enemyList;
        [SerializeField] private List<SpawnEnemyConfig> _enemyList;
    }
}
