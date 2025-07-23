using System;
using Onogawa.Scripts.Point.interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VRShooting.Scripts.Enemy.Drone.DataType;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Enemy
{
    /// <summary>
    /// 死亡ステートで自身を破壊するときの処理を記述する。
    /// </summary>
    public class DroneDestroyer : MonoBehaviour
    {
        [Inject] private IPointHolder _pointHolder;
        [SerializeField] private float _hapticsAmplitude = 0.8f;

        [SerializeField] private float _hapticsDuration = 0.4f;

        /// <summary>
        /// スコアの上昇量
        /// </summary>
        [SerializeField] private int _addScore = 100;

        /// <summary>
        /// 各ステートのイベントをサブスクライブするのに使う
        /// </summary>
        private IEnemyStateController _enemyStateController;

        /// <summary>
        /// エフェクトのデータを取り出すのに使う
        /// </summary>
        private IEffectData _effectData;

        private void Start()
        {
            // EnemyStateControllerを取得する
            if(!TryGetComponent(out _enemyStateController))
                Debug.Log("EnemyStateControllerを取得できません。");

            // EffectDataの取り出し
            if(!TryGetComponent(out _effectData))
                Debug.Log("EffectDataを取得できません。");

            // 各状態のイベントをサブスクライブ
            _enemyStateController.OnEnterDead += OnEnterDeadState;
        }

        private void Update()
        {
            // デバッグ専用のコード
#if UNITY_EDITOR
            if(Keyboard.current.pKey.wasPressedThisFrame)
                _enemyStateController.ExecuteTrigger(EnemyStateTrigger.ToDead);
#endif
        }

        /// <summary>
        /// Deadステートに移行した瞬間に実行する処理
        /// </summary>
        private void OnEnterDeadState()
        {
            // エフェクトの取得
            // var deadEffect = _effectData.GetParticleSystem(DroneEnemyEffectDataType.Explosion);

            //エフェクトをVFXに変更
            var deadEffect = _effectData.GetVisualEffect(DroneEnemyEffectDataType.Explosion);

            // エフェクトの再生
            Instantiate(deadEffect, transform.position, Quaternion.identity);

            // サウンドの取得
            var deadSFX = _effectData.GetAudioClip(DroneEnemyEffectDataType.Explosion);
            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(deadSFX, transform.position);

            // 振動させる
            var controller = PlayerStorage.instance.GetCurrentController();
            controller.SendHapticImpulse(_hapticsAmplitude, _hapticsDuration);

            // 撃破ポイントを加算
            _pointHolder.AddPoint(_addScore);

            // オブジェクトの破壊
            Destroy(this.gameObject);
        }
    }
}