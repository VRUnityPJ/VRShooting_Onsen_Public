using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UniRx;
using VRShooting.Scripts.DataBase.GunDataBase;
using VRShooting.Scripts.Gun;

namespace VRShooting.Scripts.Player
{
    /// <summary>
    /// プレイヤーの武器の入れ替え機能を提供する
    /// </summary>
    public class PlayerEquippedGunSwitcher : MonoBehaviour
    {
        /// <summary>
        /// 装備できる銃のリスト
        /// </summary>
        [SerializeField]
        private List<GunVariant> _equipGunMap;

        /// <summary>
        /// 銃のデータベース
        /// </summary>
        [SerializeField, Required]
        private GunVariantDataBase _gunDataBase;

        /// <summary>
        /// ストラテジーパターンを適用するためのDictionary
        /// </summary>
        private Dictionary<GunVariant, IGunObject> _strategyGunData = new();

        /// <summary>
        /// プレイヤーの銃の装備状況を管理する
        /// </summary>
        private IPlayerGunHandler _gunHandler;

        /// <summary>
        /// プレイヤーの入力イベントを管理する
        /// </summary>
        private IPlayerInputManager _inputManager;
        
        /// <summary>
        /// 銃の参照先の配列番号を示す
        /// </summary>
        private int _gunIndex = 0;

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        private CancellationToken _token = new();
        
        /// <summary>
        /// 銃の参照先の下限
        /// </summary>
        private const int MIN_GUN_INDEX = 0;
        
        private async void Start()
        {
            // 変数の初期化
            _token = this.GetCancellationTokenOnDestroy();
            _gunIndex = MIN_GUN_INDEX;
            TryGetComponent(out _gunHandler);
            TryGetComponent(out _inputManager);
            SetupStrategyGunData();

            // 銃が装備できるようになるまで待機
            await _gunHandler.WaitForReadyEquipment(_token);

            // 最初の銃を装備する
            var initialKey = _equipGunMap[_gunIndex];
            var initialGun = _gunDataBase.GetGunObject(initialKey);
            _gunHandler.EquipGun(initialGun);
            
            // 武器変更の入力イベントをサブスクライブ
            _inputManager.OnChangeWeapon
                .Subscribe(_ => OnChangeWeapon())
                .AddTo(gameObject);
        }

        /// <summary>
        /// ストラテジーパターンを適用するためのDictionaryを構築する。
        /// </summary>
        private void SetupStrategyGunData()
        {
            // equipGunMapで使う銃のデータを取得
            foreach (var variantKey in _equipGunMap)
            {
                // IGunObjectを取得してDictionaryに追加
                var gun = _gunDataBase.GetGunObject(variantKey);
                _strategyGunData.Add(variantKey, gun);
            }
        }

        /// <summary>
        /// 武器が変更されるときの処理
        /// </summary>
        private void OnChangeWeapon()
        {
            // 参照先をインクリメント
            _gunIndex++;

            // 上限を超えていたらリセットする
            if (_gunIndex >= _equipGunMap.Count)
                _gunIndex = 0;

            // 次に装備する武器を取得
            var key = _equipGunMap[_gunIndex];
            var nextGun = _strategyGunData.GetValueOrDefault(key);

            // 武器を変更
            _gunHandler.EquipGun(nextGun);
        }
    }
}