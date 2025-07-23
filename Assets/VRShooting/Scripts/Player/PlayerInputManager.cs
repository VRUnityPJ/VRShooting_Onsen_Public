using UnityEngine;
using UniRx;
using System;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// プレイヤーの入力イベントを管理するスクリプト
    /// </summary>
    public class PlayerInputManager : MonoBehaviour, IPlayerInputManager
    {
        /// <summary>
        /// 銃を発射するボタンが押されたときに発行されるイベント
        /// </summary>
        public IObservable<Unit> OnTriggerShoot => _onTriggerShoot;
        /// <summary>
        /// 銃を発射するボタンが押されたことを通知するイベントを発行する
        /// </summary>
        private readonly Subject<Unit> _onTriggerShoot = new ();
        
        /// <summary>
        /// 銃を発射するボタンが離されたことを通知するイベント
        /// </summary>
        public IObservable<Unit> OnReleaseShoot => _onReleaseShoot;
        /// <summary>
        /// 銃を発射するボタンが離されたことを通知するイベントを発行する
        /// </summary>
        private readonly Subject<Unit> _onReleaseShoot = new ();
        
        /// <summary>
        /// 銃を切り替えるボタンが押されたときに発行されるイベント
        /// </summary>
        public IObservable<Unit> OnChangeWeapon => _onChangeWeapon;
        /// <summary>
        /// 銃を切り替えるボタンが押されたことを通知するイベントを発行する
        /// </summary>
        private readonly Subject<Unit> _onChangeWeapon = new();

        /// <summary>
        /// 左手での入力があったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnInputLeftHand => _onInputLeftHand;
        
        /// <summary>
        /// 左手での入力があったときにイベントを発火する
        /// </summary>
        private readonly Subject<Unit> _onInputLeftHand = new();
        
        /// <summary>
        /// 右手での入力があったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnInputRightHand => _onInputRightHand;
        
        /// <summary>
        /// 右手での入力があったときにイベントを発火する
        /// </summary>
        private readonly Subject<Unit> _onInputRightHand = new();
        
        /// <summary>
        /// InputSystemActionで作成された入力を管理するクラス
        /// </summary>
        private MainInput _input;

        private void Awake()
        {
            // MainInputのインスタンスを作成
            _input = new MainInput();
            // Inputの有効化
            _input.Enable();
            
            // 入力イベントのサブスクライブ
            // 発射
            _input.Player.Fire.started      += _ => _onTriggerShoot.OnNext(Unit.Default);
            // 発射ボタンを離したことを通知
            _input.Player.Fire.canceled     += _ => _onReleaseShoot.OnNext(Unit.Default);
            // 切り替え
            _input.Player.ChangeGun.started += _ => _onChangeWeapon.OnNext(Unit.Default);
            
            // 利き手での入力
            // 左手
            _input.Player.LeftInput.started  += _ => _onInputLeftHand.OnNext(Unit.Default);
            // 右手
            _input.Player.RightInput.started += _ => _onInputRightHand.OnNext(Unit.Default);
        }
    }
}
 
    
    
