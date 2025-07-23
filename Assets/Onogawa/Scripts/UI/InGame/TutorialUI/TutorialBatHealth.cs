using System;
using Onogawa.Scripts.UI.InGame.TutorialUI;
using UniRx;
using UnitScript;
using UnityEngine;
using UnityEngine.InputSystem;
using VRShooting.Scripts.Enemy.Drone.DataType;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Enemy.TutorialBat
{
    public class TutorialBatHealth : MonoBehaviour,IDamageable,ITutorialBat
    {
        /// <summary>
        /// ヒットポイント
        /// </summary>
        [SerializeField] private float _hitPoint = 100f;

        /// <summary>
        /// 破壊されたときに発火するイベント
        /// </summary>
        public IObservable<Unit> OnDead => _onDead;
        private readonly Subject<Unit> _onDead = new();

        private IEffectData _effectData;
        [SerializeField] private InterfaceProvider<ITutorialStateController> _tscProvider;
        private ITutorialStateController _tsc;

        private void Start()
        {
            // EffectDataの取り出し
            if(!TryGetComponent(out _effectData))
                Debug.Log("EffectDataを取得できません。");

            _tsc = _tscProvider.Interface;
            if(_tsc == null)
                Debug.Log("ITutorialStateControllerを取得できません");
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(Keyboard.current.dKey.wasPressedThisFrame)
                TakeDamage(100);
        }
#endif

        public void TakeDamage(float damage)
        {
            if(damage < 0)
                return;

            _hitPoint -= damage;
            if (_hitPoint <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            //エフェクトをVFXに変更
            var deadEffect = _effectData.GetVisualEffect(DroneEnemyEffectDataType.Explosion);

            // エフェクトの再生
            Instantiate(deadEffect, transform.position, Quaternion.identity);

            // サウンドの取得
            var deadSFX = _effectData.GetAudioClip(DroneEnemyEffectDataType.Explosion);

            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(deadSFX, transform.position);

            //=== 関数内ここまでコピペ BatDestroyer.cs ===

            //通知
            _onDead.OnNext(Unit.Default);

            //自身を破壊
            Destroy(gameObject);
        }
    }
}