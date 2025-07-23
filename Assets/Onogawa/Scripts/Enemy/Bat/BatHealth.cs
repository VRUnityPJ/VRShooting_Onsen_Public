using UniRx;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.Enemy
{
    /// <summary>
    /// 敵の体力を管理する
    /// </summary>
    public class BatHealth : MonoBehaviour, IEnemyHealth, IDamageable
    {
        /// <summary>
        /// 体力の値が変更されたときのイベント
        /// </summary>
        public IReadOnlyReactiveProperty<float> OnChangeHealth => _health;

        /// <summary>
        /// 現在の体力を保持し、値が変更されるとイベントを発行する
        /// </summary>
        [SerializeField] private ReactiveProperty<float> _health;

        /// <summary>
        /// 死亡ステートに移行させるのに使う
        /// </summary>
        private IBatStateController _batStateController;

        /// <summary>
        /// 体力の最小値
        /// </summary>
        private const float MIN_HEALTH = 0f;

        private void Start()
        {
            // batStateControllerを取得
            if(!TryGetComponent(out _batStateController))
                Debug.Log("IbatStateControllerを実装しているコンポネントがアタッチされていません");

        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Damage {damage}");
            _health.Value -= damage;

            // 体力が下限を下回ったとき、死亡ステートに移行する
            if (_health.Value <= MIN_HEALTH)
            {
                _batStateController.ExecuteTrigger(BatStateTrigger.ToDead);
            }
        }
    }
}
