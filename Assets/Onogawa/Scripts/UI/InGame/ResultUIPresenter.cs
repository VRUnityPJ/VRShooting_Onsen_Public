using Cysharp.Threading.Tasks;
using Onogawa.Scripts.Enemy.Firefly;
using Onogawa.Scripts.Point;
using Onogawa.Scripts.Point.interfaces;
using UniRx;
using UnitScript;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VRShooting.Scripts.Stage.Interfaces;

namespace Onogawa.Scripts.UI.InGame
{
    public class ResultUIPresenter : MonoBehaviour
    {
        /// <summary>
        /// ILevelStateControllerを持つオブジェクト
        /// </summary>
        [FormerlySerializedAs("_lscContainer")] [Header("Model")] [SerializeField]
        private InterfaceProvider<ILevelStateController> lscProvider;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _openAudioClip;

        /// <summary>
        /// ポイント取得用
        /// </summary>
        [Inject] private IPointHolder _pointHolder;
        [Inject] private IResultUIViewer _viewer;

        /// <summary>
        /// 残りホタル数取得用
        /// </summary>
        [Inject] private FireflySearcher _fireflySearcher;

        private ILevelStateController _levelStateController;

        private void Start()
        {
            // levelStateControllerを取得する
            _levelStateController = lscProvider.Interface;

            // イベントのサブスクライブ
            _levelStateController.OnEndGame
                .Subscribe(_ => OnEndGame().Forget())
                .AddTo(this);

            // UIを非表示にする
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ゲームが終了したときに呼び出される処理
        /// </summary>
        private async UniTask OnEndGame()
        {
            //1f待たないとビルド時にバグる
            await UniTask.Delay(1);

            var curPoint = _pointHolder.Point;
            var totalPoint = _pointHolder.GetTotalPoint();
            var sum = _fireflySearcher.SumFireflies;
            var cur = _fireflySearcher.CurrentFireflies;
            var bonus = _pointHolder.RestFireflyPoint * cur;

            //Viewに渡す用のデータを作成
            var result = new ResultViewModel(curPoint, totalPoint, bonus, sum, cur);

            _viewer.View(result);

            // 開いたときの音を再生
            _audioSource.PlayOneShot(_openAudioClip);
        }
    }
}