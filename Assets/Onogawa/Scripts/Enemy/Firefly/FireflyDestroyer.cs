using Onogawa.Scripts.Enemy.Firefly.interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Enemy.Firefly
{
    /// <summary>
    /// 死亡ステートで自身を破壊するときの処理を記述する。
    /// </summary>
    public class FireflyDestroyer : MonoBehaviour
    {

        /// <summary>
        /// 各ステートのイベントをサブスクライブするのに使う
        /// </summary>
        private IFireflyStateController _enemyStateController;

        /// <summary>
        /// エフェクトのデータを取り出すのに使う
        /// </summary>
        private IEffectData _effectData;

        private void Start()
        {
            // FireflyStateControllerを取得する
            if(!TryGetComponent(out _enemyStateController))
                Debug.Log("FireflyStateControllerを取得できません。");

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
            if(Keyboard.current.oKey.wasPressedThisFrame)
                _enemyStateController.ExecuteTrigger(FireflyStateTrigger.ToDead);
#endif
        }

        /// <summary>
        /// Deadステートに移行した瞬間に実行する処理
        /// </summary>
        private void OnEnterDeadState()
        {
            //子オブジェクトを解除
            ReleaseChildren();

            //SE再生
            var se = _effectData.GetAudioClip(FireflyEffectDataType.Despawn);
            AudioPlayer.PlayOneShotAudioAtPoint(se, transform.position);

            // オブジェクトの破壊
            Destroy(gameObject);
        }

        private void ReleaseChildren()
        {
            var parent = transform;
            var childCount = parent.childCount;

            //View以外の子オブジェクトを解放
            for (int i = childCount - 1; i > 0; i--)
            {
                var child = parent.GetChild(i);
                child.SetParent(null);
            }
        }
    }
}