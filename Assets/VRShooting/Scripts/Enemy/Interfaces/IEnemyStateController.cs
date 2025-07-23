using System;

namespace VRShooting.Scripts.Enemy.Interfaces
{
    public interface IEnemyStateController
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trigger"></param>
        void ExecuteTrigger(EnemyStateTrigger trigger);
    }
}
