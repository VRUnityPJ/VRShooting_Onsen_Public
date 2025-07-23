using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using StateMachineTemplate.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;

namespace VRShooting.Scripts.Enemy
{
    public enum EnemyState
    {
        Idle,
        Move,
        Dead
    }

    public enum EnemyStateTrigger
    {
        ToIdle,
        ToMove,
        ToDead
    }
    public class EnemyStateController : MonoBehaviour, IEnemyStateController
    {
        public event Action OnEnterIdle;
        public event Action OnExitIdle;
        public event Action<float> OnUpdateIdle;
        public event Action OnEnterMove;
        public event Action OnExitMove;
        public event Action<float> OnUpdateMove;
        public event Action OnEnterDead;
        public event Action OnExitDead;
        public event Action<float> OnUpdateDead;

        /// <summary>
        /// 敵の初期ステート
        /// </summary>
        [SerializeField] private EnemyState _initialState;

        private StateMachine<EnemyState, EnemyStateTrigger> enemyStateMachine => _enemyStateMachine;
        private StateMachine<EnemyState, EnemyStateTrigger> _enemyStateMachine;

        // Start is called before the first frame update
        void Start()
        {
            StateInitialize();
        }

        // Update is called once per frame
        void Update()
        {
            _enemyStateMachine.Update(Time.deltaTime);
        }

        private void StateInitialize()
        {
            _enemyStateMachine = new StateMachine<EnemyState, EnemyStateTrigger>();
            // 各ステートのセットアップ
            _enemyStateMachine.SetupState
            (
                EnemyState.Idle,
                () => OnEnterIdle?.Invoke(),
                () => OnExitIdle?.Invoke(),
                deltaTime => OnUpdateIdle?.Invoke(deltaTime)
            );
            _enemyStateMachine.SetupState
            (
                EnemyState.Move,
                () => OnEnterMove?.Invoke(),
                () => OnExitMove?.Invoke(),
                deltaTime => OnUpdateMove?.Invoke(deltaTime)
            );
            _enemyStateMachine.SetupState
            (
                EnemyState.Dead,
                () => OnEnterDead?.Invoke(),
                () =>OnExitDead?.Invoke(),
                deltaTime => OnUpdateDead?.Invoke(deltaTime)
            );

            // ステートの遷移条件を設定
            _enemyStateMachine.AddTransition
            (
                EnemyState.Idle,
                EnemyState.Move,
                EnemyStateTrigger.ToMove
            );
            _enemyStateMachine.AddTransition
            (
                EnemyState.Move,
                EnemyState.Idle,
                EnemyStateTrigger.ToIdle
            );
            _enemyStateMachine.AddTransition
            (
                EnemyState.Idle,
                EnemyState.Dead,
                EnemyStateTrigger.ToDead
            );
            _enemyStateMachine.AddTransition
            (
                EnemyState.Move,
                EnemyState.Dead,
                EnemyStateTrigger.ToDead
            );
        }
        public void ExecuteTrigger(EnemyStateTrigger trigger)
        {
            _enemyStateMachine.ExecuteTrigger(trigger);
        }
    }
}
