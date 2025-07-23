using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using StateMachineTemplate.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;

namespace VRShooting.Scripts.Enemy
{
    public enum BatState
    {
        Idle,
        Found,
        Hold,
        Dead
    }

    public enum BatStateTrigger
    {
        ToIdle,
        ToFound,
        ToHold,
        ToDead
    }
    public class BatStateController : MonoBehaviour, IBatStateController
    {
        public event Action OnEnterIdle;
        public event Action OnExitIdle;
        public event Action<float> OnUpdateIdle;
        public event Action OnEnterFound;
        public event Action OnExitFound;
        public event Action<float> OnUpdateFound;
        public event Action OnEnterHold;
        public event Action OnExitHold;
        public event Action<float> OnUpdateHold;
        public event Action OnEnterDead;
        public event Action OnExitDead;
        public event Action<float> OnUpdateDead;

        /// <summary>
        /// 敵の初期ステート
        /// </summary>
        [SerializeField] private BatState _initialState;

        private StateMachine<BatState, BatStateTrigger> batStateMachine => _batStateMachine;
        private StateMachine<BatState, BatStateTrigger> _batStateMachine;

        // Start is called before the first frame update
        void Start()
        {
            StateInitialize();
        }

        // Update is called once per frame
        void Update()
        {
            _batStateMachine.Update(Time.deltaTime);
        }

        private void StateInitialize()
        {
            _batStateMachine = new StateMachine<BatState, BatStateTrigger>();
            // 各ステートのセットアップ
            _batStateMachine.SetupState
            (
                BatState.Idle,
                () => OnEnterIdle?.Invoke(),
                () => OnExitIdle?.Invoke(),
                deltaTime => OnUpdateIdle?.Invoke(deltaTime)
            );
            _batStateMachine.SetupState
            (
                BatState.Found,
                () => OnEnterFound?.Invoke(),
                () => OnExitFound?.Invoke(),
                deltaTime => OnUpdateFound?.Invoke(deltaTime)
            );
            _batStateMachine.SetupState
            (
                BatState.Hold,
                () => OnEnterHold?.Invoke(),
                () => OnExitHold?.Invoke(),
                deltaTime => OnUpdateHold?.Invoke(deltaTime)
            );
            _batStateMachine.SetupState
            (
                BatState.Dead,
                () => OnEnterDead?.Invoke(),
                () => OnExitDead?.Invoke(),
                deltaTime => OnUpdateDead?.Invoke(deltaTime)
            );

            // ステートの遷移条件を設定
            _batStateMachine.AddTransition
            (
                BatState.Idle,
                BatState.Found,
                BatStateTrigger.ToFound
            );
            _batStateMachine.AddTransition
            (
                BatState.Found,
                BatState.Found,
                BatStateTrigger.ToFound
            );
            _batStateMachine.AddTransition
            (
                BatState.Hold,
                BatState.Found,
                BatStateTrigger.ToFound
            );
            _batStateMachine.AddTransition
            (
                BatState.Found,
                BatState.Idle,
                BatStateTrigger.ToIdle
            );
            _batStateMachine.AddTransition
            (
                BatState.Found,
                BatState.Hold,
                BatStateTrigger.ToHold
            );
            _batStateMachine.AddTransition
            (
                BatState.Idle,
                BatState.Dead,
                BatStateTrigger.ToDead
            );
            _batStateMachine.AddTransition
            (
                BatState.Found,
                BatState.Dead,
                BatStateTrigger.ToDead
            );
            _batStateMachine.AddTransition
            (
                BatState.Hold,
                BatState.Dead,
                BatStateTrigger.ToDead
            );
            _batStateMachine.AddTransition
            (
                BatState.Hold,
                BatState.Idle,
                BatStateTrigger.ToIdle
            );

            _batStateMachine.Initialize(_initialState);
        }
        public void ExecuteTrigger(BatStateTrigger trigger)
        {
            _batStateMachine.ExecuteTrigger(trigger);
        }
    }
}
