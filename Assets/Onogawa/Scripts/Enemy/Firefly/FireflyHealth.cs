using Onogawa.Scripts.Enemy.Firefly.interfaces;
using UniRx;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;

namespace Onogawa.Scripts.Enemy.Firefly
{
    /// <summary>
    /// 敵の体力を管理する
    /// </summary>
    public class FireflyHealth : MonoBehaviour, IEnemyHealth, IDamageable,IFireflyHealth
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
        private IFireflyStateController _fireflyStateController;

        /// <summary>
        /// 体力の最小値
        /// </summary>
        private const float MIN_HEALTH = 0f;

        private void Start()
        {
            // EnemyStateControllerを取得
            if(!TryGetComponent(out _fireflyStateController))
                Debug.Log("IEnemyStateControllerを実装しているコンポネントがアタッチされていません");
        }

        public void TakeDamage(float damage)
        {
            _health.Value -= damage;

            // 体力が下限を下回ったとき、死亡ステートに移行する
            if (_health.Value <= MIN_HEALTH)
            {
                _fireflyStateController.ExecuteTrigger(FireflyStateTrigger.ToDead);
            }
        }

        public void ReceiveAttack()
        {
            _fireflyStateController.ExecuteTrigger(FireflyStateTrigger.ToDead);
        }
    }
}
