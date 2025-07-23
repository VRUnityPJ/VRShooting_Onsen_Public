using NaughtyAttributes;
using Onogawa.Scripts.Point;
using Onogawa.Scripts.Point.interfaces;
using TMPro;
using UniRx;
using UnitScript;
using UnityEngine;
using VContainer;
using VRShooting.Scripts.Stage.Interfaces;

namespace VRShooting.Scripts.UI
{
    public class ScoreUIPresenter : MonoBehaviour
    {
        [Inject] private IPointHolder _pointHolder;
        [SerializeField] private InterfaceProvider<ILevelStateController> lscProvider;
        private ILevelStateController _levelStateController;
        [SerializeField, Required] private TextMeshProUGUI _scoreText;
        [SerializeField, Required] private TextMeshProUGUI _timeText;

        private void Start()
        {
            _levelStateController = lscProvider.Interface;

            _levelStateController.OnStartGame
                .Subscribe(_ => gameObject.SetActive(true))
                .AddTo(this);

            _levelStateController.OnEndGame
                .Subscribe(_ => gameObject.SetActive(false))
                .AddTo(this);

            gameObject.SetActive(false);
        }

        private void Update()
        {
            // テキストを更新
            var score = _pointHolder.Point.ToString();
            var time = _levelStateController.GetTime();
            _scoreText.text = $"Score : {score}";
            _timeText.text = $"Time : {time} sec";
        }
    }
}