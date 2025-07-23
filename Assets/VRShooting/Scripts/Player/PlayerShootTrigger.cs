using System;
using UnityEngine;
using UniRx;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// プレイヤーが銃を発射する機能を提供するクラス
    /// </summary>
    public class PlayerShootTrigger : MonoBehaviour,IPlayerShootTrigger
    {
        /// <summary>
        /// プレイヤーの入力イベント
        /// </summary>
        private IPlayerInputManager _input;
        /// <summary>
        /// 装備している銃の情報
        /// </summary>
        private IPlayerGunHandler _gunHandler;
        /// <summary>
        /// 装備している銃
        /// </summary>
        private IGunObject _equippedGun;

        private void Start()
        {
            TryGetComponent(out _input);
            TryGetComponent(out _gunHandler);

            // 銃を発射したときのイベントをサブスクライブ
            _input.OnTriggerShoot
                .Subscribe(_ => OnShoot())
                .AddTo(this);

            // トリガーを離したときのイベントをサブスクライブ
            _input.OnReleaseShoot
                .Subscribe(_ => OnReleaseTrigger());

            // 銃が変更されたときのイベントをサブスクライブ
            _gunHandler.OnChangeGun
                .Subscribe(gun => OnChangeGun(gun))
                .AddTo(this);
        }

        /// <summary>
        /// 銃を発射するときの処理
        /// </summary>
        private void OnShoot()
        {
            // GunHandlerから銃を取得できていない場合、例外をスローする
            if (_equippedGun == null)
                throw new NullReferenceException("GunHandlerから銃を取得していません。");

            // 弾を発射する
            _equippedGun.Shoot();
        }

        /// <summary>
        /// トリガーを離したときの処理
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void OnReleaseTrigger()
        {
            // GunHandlerから銃を取得できていない場合、例外をスローする
            if (_equippedGun == null)
                throw new NullReferenceException("GunHandlerから銃を取得していません。");

            // 弾を発射する
            _equippedGun.OnReleaseTrigger();
        }

        /// <summary>
        /// 銃が入れ替わったときの処理
        /// </summary>
        private void OnChangeGun(IGunObject gunObject)
        {
            _equippedGun = gunObject;
        }
    }
}


