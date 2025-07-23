using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// プレイヤーが持つ銃にアクセスするときにつかうインターフェース
    /// </summary>
    public interface IPlayerGunHandler
    {
        /// <summary>
        /// 装備している銃を変更するときに呼び出される
        /// </summary>
        public IReadOnlyReactiveProperty<GunVariant> OnChangeGunVariant { get; }
        /// <summary>
        /// 装備している銃を変更するときに呼び出される
        /// </summary>
        public IReadOnlyReactiveProperty<IGunObject> OnChangeGun { get; }
        /// <summary>
        /// 装備している銃にアクセスする
        /// </summary>
        /// <returns></returns>
        public IGunObject GetEquippedGun();
        /// <summary>
        /// 銃を装備する
        /// </summary>
        /// <param name="equippedGun"></param>
        public void EquipGun(IGunObject equippedGun);
        /// <summary>
        /// プレイヤーが武器を装備できるまで待機する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UniTask WaitForReadyEquipment(CancellationToken token);
    }
}

