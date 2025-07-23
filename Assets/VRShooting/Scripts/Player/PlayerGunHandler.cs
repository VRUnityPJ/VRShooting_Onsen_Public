using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VRShooting.Scripts.Gun;
using UniRx;

namespace VRShooting.Scripts.Player
{
    public class PlayerGunHandler : MonoBehaviour,IPlayerGunHandler
    {
        /// <summary>
        /// 現在装備している武器タイプが変更されたときに発行されるイベント
        /// </summary>
        public IReadOnlyReactiveProperty<GunVariant> OnChangeGunVariant => _currentGunVariant;
        /// <summary>
        /// 現在の武器タイプ (未使用)
        /// </summary>
        private readonly ReactiveProperty<GunVariant> _currentGunVariant = new ();
        /// <summary>
        /// 装備している武器が変更されたときに発行されるイベント
        /// </summary>
        public IReadOnlyReactiveProperty<IGunObject> OnChangeGun => _currentGun;
        /// <summary>
        /// 現在装備している武器
        /// </summary>
        private readonly ReactiveProperty<IGunObject> _currentGun = new ();
        /// <summary>
        /// プレイヤーの利き手の情報にアクセスできるデータ
        /// </summary>
        private IPlayerHandData _handData;
        /// <summary>
        /// 武器が装備できるかどうか
        /// </summary>
        private bool _canEquipment = false;

        private void Start()
        {
            // このときは武器を装備する準備が整っていないためTrue
            _canEquipment = false;
            
            // 武器の装備先を取得するためにHandDataを取得
            TryGetComponent(out _handData);
            Debug.Assert(_handData != null);

            // 利き手が変更されたとき、武器の位置を変える
            _handData.OnChangeHandType
                .Subscribe(dominantHand => _currentGun.Value?.Equipped(_handData.GetHandPosition()))
                .AddTo(this);
            
            // 武器の装備先を取得できるようになったのでtrue
            _canEquipment = true;
        }
        
        // 現在装備している武器のインターフェースを返す
        public IGunObject GetEquippedGun()
        {
            // 現在装備している武器を取得
            var equippedWeapon = _currentGun.Value;
            // 装備している武器がなかったら例外をスローする
            if (equippedWeapon == null)
                throw new NullReferenceException("現在装備している銃がありません");
            
            // 現在装備している武器を返す
            return equippedWeapon;
        }
        
        // 武器を装備する
        public void EquipGun(IGunObject equippedGun)
        {
            // 今装備している武器があったら、装備を解除
            _currentGun.Value?.RemoveEquipment();

            // 新しい武器を生成して銃を切り替え
            var gun = equippedGun.InstantiateGunObject();
            _currentGun.Value = gun;
            _currentGun.Value.Equipped(_handData.GetHandPosition());
        }
        
        // 武器が装備できるようになるまで待機する
        public async UniTask WaitForReadyEquipment(CancellationToken token)
        {
            // CanEquipmentがTrueになるまで待機
            await UniTask.WaitUntil(() => _canEquipment, cancellationToken: token);
        }
    }
}

