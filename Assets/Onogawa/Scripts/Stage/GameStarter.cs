using Cysharp.Threading.Tasks;
using Onogawa.Scripts.Point.interfaces;
using UnitScript;
using UnityEngine;
using VContainer;
using VRShooting.Scripts.Stage.Interfaces;

namespace Onogawa.Scripts.Stage
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private InterfaceProvider<ILevelStateController> lscProvider;
        [SerializeField] private int _delay;

        private ILevelStateController _lsc;

        private const int SECOND_TO_MILLISECOND = 1000;

        private void Start() => Init().Forget();

        private async UniTask Init()
        {
            _lsc = lscProvider.Interface;

            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(_delay*SECOND_TO_MILLISECOND,cancellationToken:token);

            _lsc.OnPlayerReady();
        }
    }
}
