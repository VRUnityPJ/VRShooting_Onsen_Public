using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace VRShooting.Scripts.Stage.Interfaces
{
    public interface ISpawnLevelScale
    {
        public IObservable<Unit> OnChangeWave { get; }
        public IObservable<Unit> OnFinishedFinalWave { get; }
        public int waveCount { get; }
        public UniTask StartNormalSpawn(CancellationToken token);
        public UniTask SpawnBonusTimeAsync( CancellationToken token);
    }
}
