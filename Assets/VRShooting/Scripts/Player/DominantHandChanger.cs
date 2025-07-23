using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// 利き手を変更するスクリプト
    /// </summary>
    public class DominantHandChanger : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーの入力を管理するクラス
        /// </summary>
        private IPlayerInputManager _inputManager;

        /// <summary>
        /// プレイヤーの利き手の情報を保持しているクラス
        /// </summary>
        private IPlayerHandData _handData;

        private void Start()
        {
            // コンポーネントの取得
            TryGetComponent(out _inputManager);
            TryGetComponent(out _handData);

            // 入力イベントのサブスクライブ
            // 左手の入力があったときは、利き手を左手に
            _inputManager.OnInputLeftHand
                .Subscribe(_ => _handData.SetHandType(HandType.Left))
                .AddTo(this);
            // 右手の入力があったときは、利き手を右手に
            _inputManager.OnInputRightHand
                .Subscribe(_ => _handData.SetHandType(HandType.Right))
                .AddTo(this);
        }
    }
}