using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnitScript;
using UnityEngine;
using UnityEngine.InputSystem;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Stage.Interfaces;
namespace Onsen
{
    public class LevelWallSwitcher : MonoBehaviour
    {
        [SerializeField] private InterfaceProvider<ISpawnLevelScale> _spawnScaleProvider;
        /// <summary>
        /// 切り替え対象の壁をアタッチ、すべての壁を入れる必要はない
        /// </summary>
        [SerializeField, Required] private List<GameObject> _switchableWallList;
        [SerializeField] private int _switchLevelWave = 3;
        private ISpawnLevelScale _spawnerLevelScale;
        void Start()
        {
            _spawnerLevelScale = _spawnScaleProvider.Interface;

            // 最初は自分で実行する
            OnChangeWave();

            // イベントをサブスクライブ
            _spawnerLevelScale.OnChangeWave
                .Subscribe(_ => OnChangeWave())
                .AddTo(this);
        }
        /// <summary>
        /// Wave変更時呼び出し
        /// </summary>
        private void OnChangeWave()
        {
            if (_spawnerLevelScale.waveCount >= _switchLevelWave)
            {
                //壁を消すことでステージの拡大
                foreach (var wallObj in _switchableWallList)
                {
                    wallObj.SetActive(false);
                    //ここ全部消すことしかできないのが汎用性ない、任意の番号の壁のオンオフを切り替えが理想
                }
                Debug.Log("壁無効化");
            }
        }
    }

}
