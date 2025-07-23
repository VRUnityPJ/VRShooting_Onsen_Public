using System;
using Onogawa.Scripts.Enemy.Firefly.interfaces;
using StateMachineTemplate.Scripts;
using UnityEngine;

namespace Onogawa.Scripts.Enemy.Firefly
{
    public enum FireflyState
    {
        Idle,
        Move,
        Dead
    }

    public enum FireflyStateTrigger
    {
        ToIdle,
        ToMove,
        ToDead
    }
    public class FireflyStateController : MonoBehaviour, IFireflyStateController
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
        [SerializeField] private FireflyState _initialState;

        private StateMachine<FireflyState, FireflyStateTrigger> _fireflyStateMachine;

        // Start is called before the first frame update
        private void Start()
        {
            StateInitialize();
        }

        // Update is called once per frame
        private void Update()
        {
            _fireflyStateMachine.Update(Time.deltaTime);
        }

        private void StateInitialize()
        {
            _fireflyStateMachine = new StateMachine<FireflyState, FireflyStateTrigger>();
            // 各ステートのセットアップ
            _fireflyStateMachine.SetupState
            (
                FireflyState.Idle,
                () => OnEnterIdle?.Invoke(),
                () => OnExitIdle?.Invoke(),
                deltaTime => OnUpdateIdle?.Invoke(deltaTime)
            );
            _fireflyStateMachine.SetupState
            (
                FireflyState.Move,
                () => OnEnterMove?.Invoke(),
                () => OnExitMove?.Invoke(),
                deltaTime => OnUpdateMove?.Invoke(deltaTime)
            );
            _fireflyStateMachine.SetupState
            (
                FireflyState.Dead,
                () => OnEnterDead?.Invoke(),
                () =>OnExitDead?.Invoke(),
                deltaTime => OnUpdateDead?.Invoke(deltaTime)
            );

            // ステートの遷移条件を設定
            _fireflyStateMachine.AddTransition
            (
                FireflyState.Idle,
                FireflyState.Move,
                FireflyStateTrigger.ToMove
            );
            _fireflyStateMachine.AddTransition
            (
                FireflyState.Move,
                FireflyState.Idle,
                FireflyStateTrigger.ToIdle
            );
            _fireflyStateMachine.AddTransition
            (
                FireflyState.Idle,
                FireflyState.Dead,
                FireflyStateTrigger.ToDead
            );
            _fireflyStateMachine.AddTransition
            (
                FireflyState.Move,
                FireflyState.Dead,
                FireflyStateTrigger.ToDead
            );

            _fireflyStateMachine.Initialize(_initialState);
        }
        public void ExecuteTrigger(FireflyStateTrigger trigger)
        {
            _fireflyStateMachine.ExecuteTrigger(trigger);
        }
    }
}
