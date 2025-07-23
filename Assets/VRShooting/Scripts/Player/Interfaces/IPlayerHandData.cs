using UnityEngine;
using UniRx;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRShooting.Scripts.Player
{
    public interface IPlayerHandData
    {
        /// <summary>
        /// 利き手が変更されたときに発行されるイベント
        /// </summary>
        public IReadOnlyReactiveProperty<HandType> OnChangeHandType { get; }
        /// <summary>
        /// 利き手を変更する。
        /// </summary>
        /// <param name="handType"></param>
        public void SetHandType(HandType handType);
        /// <summary>
        /// 現在の利き手の位置を取得する
        /// </summary>
        /// <returns></returns>
        public Transform GetHandPosition();

        /// <summary>
        /// 利き手のXRControllerを取得する
        /// </summary>
        /// <returns></returns>
        public ActionBasedController GetController();
    }
}

