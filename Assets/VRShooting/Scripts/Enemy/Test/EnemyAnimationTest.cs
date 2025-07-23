using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRShooting.Scripts.Enemy.Test
{
    public class EnemyAnimationTest : MonoBehaviour
    {
        public Animator animator;

        private void Update()
        {
            // 死亡モーションの再生
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                animator.SetTrigger("Dead");
            }

            // 浮遊モーションからリセット
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                animator.SetTrigger("Reset");
            }
        }
    }
}