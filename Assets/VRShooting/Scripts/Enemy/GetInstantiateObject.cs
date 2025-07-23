using UnityEngine;
using VRShooting.Scripts.Stage;

namespace VRShooting.Scripts.Enemy
{
    public class  GetInstantiateObject: MonoBehaviour
    {
        public GameObject sourcePrefab; // プレハブの参照を保持する変数
        internal EnemyStorage _enemyAllDead;
        public void SetSourcePrefab(GameObject prefab)
        {
            sourcePrefab = prefab;
        }

        public void GetSourcePrefab()
        {
            _enemyAllDead = sourcePrefab.GetComponent<EnemyStorage>();
            Debug.Log("Got SpawnerLevelScale");
        }
    }
}