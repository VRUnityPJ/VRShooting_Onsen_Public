using System;
using UnityEngine;
using VRShooting.Scripts.Stage;

namespace VRShooting.Scripts.Player
{
    public class PlayerObject : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーの目の位置
        /// </summary>
        public Transform eyeTransform => _eyeTransform;
        [SerializeField] private Transform _eyeTransform;

        private void Start()
        {
            // PlayerStorageに自身の情報を送る
            PlayerStorage.instance.AddPlayerObject(this);
        }
    }
}