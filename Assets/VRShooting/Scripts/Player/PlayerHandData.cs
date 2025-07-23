using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRShooting.Scripts.Stage;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// 利き手
    /// </summary>
    public enum HandType
    {
        Right,
        Left
    }
    /// <summary>
    /// プレイヤーの利き手の情報を保持する
    /// </summary>
    public class PlayerHandData : MonoBehaviour,IPlayerHandData
    {
        /// <summary>
        /// 利き手が変更されたときに発行されるイベント
        /// </summary>
        public IReadOnlyReactiveProperty<HandType> OnChangeHandType => _handType;
        /// <summary>
        /// 利き手の情報を格納する変数
        /// </summary>
        private readonly ReactiveProperty<HandType> _handType = new (HandType.Right);
        /// <summary>
        /// ゲーム開始時にどの手を利き手にするかを示す変数
        /// </summary>
        [SerializeField] private HandType _initialHandType;
        /// <summary>
        /// 左手のオブジェクトのTransform
        /// </summary>
        [SerializeField, Required] private ActionBasedController _leftHand;
        /// <summary>
        /// 右手のオブジェクトのTransform
        /// </summary>
        [SerializeField, Required] private ActionBasedController _rightHand;

        private void Start()
        {
            // 別のシーンから情報を取得するのであればここを置き換える
            _handType.Value = _initialHandType;

            // PlayerHandDataの登録
            PlayerStorage.instance.AddPlayerHandData(this);
        }

        /// <summary>
        /// 利き手を変更する。
        /// </summary>
        /// <param name="handType"></param>
        public void SetHandType(HandType handType)
        {
            _handType.Value = handType;
        }

        /// <summary>
        /// 現在の利き手の位置を取得する
        /// </summary>
        /// <returns></returns>
        public Transform GetHandPosition()
        {
            switch (_handType.Value)
            {
                case HandType.Right:
                    return _rightHand.transform;
                case HandType.Left:
                    return _leftHand.transform;
                default:
                    // 万が一のためデフォルトで右手の位置を返す
                    return _rightHand.transform;
            }
        }

        /// <summary>
        /// 利き手のXRControllerを取得する
        /// </summary>
        /// <returns></returns>
        public ActionBasedController GetController()
        {
            switch (_handType.Value)
            {
                case HandType.Right:
                    return _rightHand;
                case HandType.Left:
                    return _leftHand;
                default:
                    // 万が一のためデフォルトで右手の位置を返す
                    return _rightHand;
            }
        }
    }
}


