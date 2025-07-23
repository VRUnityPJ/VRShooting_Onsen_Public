using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.Player.Test
{
    public class PlayerGunHandlerTest : MonoBehaviour
    {
        public PlayerGunHandler gunHandler;

        [Header("テスト対象の銃")] 
        [SerializeField] private GunBehaviour _debugTargetGun;
        // [SerializeField] private GunBehaviour _secondGun;
        private CancellationToken _token;
        private async void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
            // プレイヤーが武器を装備できるようになるまで待機
            await gunHandler.WaitForReadyEquipment(_token);
            // 銃を装備
            gunHandler.EquipGun(_debugTargetGun);
        }

        // private void Update()
        // {
        //     if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //     {
        //         gunHandler.EquipGun(_firstGunBase);
        //     }
        //     else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //     {
        //         gunHandler.EquipGun(_secondGun);
        //     }
        // }
    }
}