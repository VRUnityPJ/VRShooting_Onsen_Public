using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting.Scripts.Enemy.Interfaces
{
    public interface IBatStateController
    {
        event Action OnEnterIdle;
        event Action OnExitIdle;
        event Action<float> OnUpdateIdle;
        event Action OnEnterFound;
        event Action OnExitFound;
        event Action<float> OnUpdateFound;
        event Action OnEnterHold;
        event Action OnExitHold;
        event Action<float> OnUpdateHold;
        event Action OnEnterDead;
        event Action OnExitDead;
        event Action<float> OnUpdateDead;
        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        void ExecuteTrigger(BatStateTrigger trigger);
    }
}

