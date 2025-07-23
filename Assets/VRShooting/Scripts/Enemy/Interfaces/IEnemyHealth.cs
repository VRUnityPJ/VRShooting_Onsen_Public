using UniRx;

namespace VRShooting.Scripts.Enemy.Interfaces
{
    public interface IEnemyHealth
    {
        IReadOnlyReactiveProperty<float> OnChangeHealth { get; }

    }
}
