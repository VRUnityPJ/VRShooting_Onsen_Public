using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRShooting.Scripts.Player;

namespace VRShooting.Scripts.Stage
{
    public class PlayerStorage : MonoBehaviour
    {
        /// <summary>
        /// PlayerStorageのシングルトン
        /// </summary>
        public static PlayerStorage instance;
        
        /// <summary>
        /// プレイヤーオブジェクト
        /// </summary>
        private PlayerObject _playerObject;

        private IPlayerHandData _handData;
        
        private void Awake()
        {
            // DontDestroyに入れないシングルトン
            #region Singleton
            
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            #endregion
        }

        private void OnDestroy()
        {
            // シーンを切り替えるときなどはシングルトンを破棄する
            if(instance == this)
                instance = null;
        }

        /// <summary>
        /// PlayerObjectの情報を受け取る
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayerObject(PlayerObject player)
        {
            _playerObject = player;
        }

        /// <summary>
        /// プレイヤーのハンドデータを受け取る
        /// </summary>
        /// <param name="handData"></param>
        public void AddPlayerHandData(IPlayerHandData handData)
        {
            _handData = handData;
        }

        /// <summary>
        /// プレイヤーの目の位置を取得する。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">PlayerObjectまたはEyeTransformが取得できないときにスローされる。</exception>
        public Transform GetPlayerEyeTransform()
        {
            // プレイヤーオブジェクトが取得出来ていない場合、例外をスローする
            if (_playerObject == null)
                throw new NullReferenceException("PlayerStorageがPlayerObjectを取得できていません。");
            
            // プレイヤーオブジェクトからEyeTransformが取得出来ていない場合、例外をスローする
            if(_playerObject.eyeTransform == null)
                throw new NullReferenceException(
                    "PlayerEyeTransformが取得できませんでした。PlayerObjectにeyeTransformが設定されているか確認してください。");
            
            return _playerObject.eyeTransform;
        }

        /// <summary>
        /// 利き手のXRControllerを取得する
        /// </summary>
        /// <returns></returns>
        public ActionBasedController GetCurrentController()
        {
            if (_handData == null)
                throw new NullReferenceException("PlayerHandDataが取得できていません。");
            
            return _handData.GetController();
        }
    }
}