using System;

namespace Onogawa.Scripts.Enemy.Firefly.interfaces
{
    public interface IFireflyStateController
    {
        event Action OnEnterIdle;
        event Action OnExitIdle;
        event Action<float> OnUpdateIdle;
        event Action OnEnterMove;
        event Action OnExitMove;
        event Action<float> OnUpdateMove;
        event Action OnEnterDead;
        event Action OnExitDead;
        event Action<float> OnUpdateDead;

        void ExecuteTrigger(FireflyStateTrigger trigger);
    }
}

