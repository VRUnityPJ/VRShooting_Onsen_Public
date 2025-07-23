using NaughtyAttributes;
using Onogawa.Scripts.Point.interfaces;
using UniRx;
using UnitScript;
using UnityEngine;
using VContainer;
using VRShooting.Scripts.Stage.Interfaces;

namespace Onogawa.Scripts.UI.InGame.TutorialUI
{
    public class Ready : MonoBehaviour
    {
        [SerializeField] private InterfaceProvider<ILevelStateController> _lscProvider;
        [Inject] private IPointHolder _pointHolder;

        private ILevelStateController _lsc;
        private ITutorialStateController _tsc;

        private void Start()
        {
            _lsc = _lscProvider.Interface;
            if(_lsc == null)
                Debug.Log("ILevelStateControllerが取得できません");
            if(!TryGetComponent(out _tsc))
                Debug.LogError("ITutorialStateControllerが取得できません");

            _tsc.OnEndTutorial
                .First()
                .Subscribe(_ => ReadyToPlay())
                .AddTo(this);

        }

        //TODO Debugなので後でButton消す
        [Button]
        private void ReadyToPlay()
        {
            _lsc.OnPlayerReady();

            //ポイントを初期化
            _pointHolder.InitializePoint();

            gameObject.SetActive(false);
        }
    }
}