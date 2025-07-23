using UniRx;
using System;

namespace VRShooting.Scripts.Player
{
    public interface IPlayerInputManager
    {
        /// <summary>
        /// 銃を発射するボタンが押されたときに発行されるイベント
        /// </summary>
        public IObservable<Unit> OnTriggerShoot { get; }

        /// <summary>
        /// 銃を発射するボタンが離されたことを通知するイベント
        /// </summary>
        public IObservable<Unit> OnReleaseShoot { get; }

        /// <summary>
        /// 銃を切り替えるボタンが押されたときに発行されるイベント
        /// </summary>
        public IObservable<Unit> OnChangeWeapon { get; }
        
        /// <summary>
        /// 左手での入力があったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnInputLeftHand { get; }
        
        /// <summary>
        /// 右手での入力があったときに発火されるイベント
        /// </summary>
        public IObservable<Unit> OnInputRightHand { get; }

    }
}

