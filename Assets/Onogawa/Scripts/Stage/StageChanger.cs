using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Onogawa.Scripts.UI.InGame;
using UniRx;
using UnitScript;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using VContainer;
using VRShooting.Scripts.Stage.Interfaces;

namespace Onogawa.Scripts.Stage
{
    public class StageChanger : MonoBehaviour
    {
        [SerializeField] private InterfaceProvider<ILevelStateController> lscProvider;
        private ILevelStateController lsc;
        [Inject] private IResultUIViewer _resultUIViewer;
        [Header("Parameters")] [SerializeField, MinValue(0f)]
        private float _delay;

        [FormerlySerializedAs("_sceneName")] [SerializeField, Scene]
        private string _nextScene;

        // ReSharper disable Unity.PerformanceAnalysis
        void Start()
        {
            // キャンセルトークンの取得
            var token = this.GetCancellationTokenOnDestroy();
            lsc = lscProvider.Interface;

            _resultUIViewer.OnFinishedResult
                .Subscribe(_ => ChangeStageAsync(token).Forget())
                .AddTo(this);
        }

        private async UniTask ChangeStageAsync(CancellationToken token)
        {
            // 遅延をかける
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);

            // シーンの読み込み
            SceneManager.LoadSceneAsync(_nextScene);
        }
    }
}
