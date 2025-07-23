using Onogawa.Scripts.Point.interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VRShooting.Scripts.Enemy;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Stage.Interfaces;
using VRShooting.Scripts.Utility;
using UniRx;

namespace Onogawa.Scripts.Enemy.Bat
{
    /// <summary>
    /// 死亡ステートで自身を破壊するときの処理を記述する。
    /// </summary>
    public class BatDestroyer : MonoBehaviour
    {

        [SerializeField] private float _hapticsAmplitude = 0.8f;
        [SerializeField] private float _hapticsDuration = 0.4f;

        [Inject] private IPointHolder _pointHolder;
        [Inject] private ILevelStateController _lsc;

        /// <summary>
        /// スコアの上昇量
        /// </summary>
        [SerializeField] private int _addScore = 100;

        /// <summary>
        /// 各ステートのイベントをサブスクライブするのに使う
        /// </summary>
        private IBatStateController _batStateController;

        /// <summary>
        /// エフェクトのデータを取り出すのに使う
        /// </summary>
        private IEffectData _effectData;

        /// <summary>
        /// ゲームが進行しているか
        /// </summary>
        private bool _isGamePlaying = true;

        private void Start()
        {
            // EnemyStateControllerを取得する
            if(!TryGetComponent(out _batStateController))
                Debug.Log("EnemyStateControllerを取得できません。");

            // EffectDataの取り出し
            if(!TryGetComponent(out _effectData))
                Debug.Log("EffectDataを取得できません。");

            _lsc.OnEndGame
                .Subscribe(_ =>
                {
                    _isGamePlaying = false;
                })
                .AddTo(this);

            // 各状態のイベントをサブスクライブ
            _batStateController.OnEnterDead += OnEnterDeadState;
        }

        private void Update()
        {
            // デバッグ専用のコード
#if UNITY_EDITOR
            if(Keyboard.current.pKey.wasPressedThisFrame)
                _batStateController.ExecuteTrigger(BatStateTrigger.ToDead);
#endif
        }

        /// <summary>
        /// Deadステートに移行した瞬間に実行する処理
        /// </summary>
        private void OnEnterDeadState()
        {
            Debug.Log("DESTROY");

            //エフェクトをVFXに変更
            var deadEffect = _effectData.GetVisualEffect(BatEffectDataType.Despawn);

            // エフェクトの再生
            Instantiate(deadEffect, transform.position, Quaternion.identity);

            // サウンドの取得
            var deadSFX = _effectData.GetAudioClip(BatEffectDataType.Despawn);
            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(deadSFX, transform.position);

            // 振動させる
            var controller = PlayerStorage.instance.GetCurrentController();
            controller.SendHapticImpulse(_hapticsAmplitude, _hapticsDuration);

            // ゲームが進行していたら
            if (_isGamePlaying)
            {
                // 撃破ポイントを加算
                _pointHolder.AddPoint(_addScore);
            }

            // オブジェクトの破壊
            Destroy(this.gameObject);
        }
    }
}