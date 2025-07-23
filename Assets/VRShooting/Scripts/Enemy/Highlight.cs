using System;
using UniRx;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;

namespace VRShooting.Scripts.Enemy
{
    /// <summary>
    /// 敵のハイライトを制御するクラス
    /// </summary>
    public class Highlight : MonoBehaviour
    {
        /// <summary>
        /// エネミーの最大HP
        /// </summary>
        [SerializeField] private float _maxHealth;
        private Material[] materials;
        private EnemyHealth _enemyHealth;

         
        private void Start()
        {
            int i = 0;
            materials = new Material[2];
            foreach (var renders in GetComponentsInChildren<MeshRenderer>())
            {
                materials[i] = renders.materials[1];
                i++;
            }

            if (!TryGetComponent(out _enemyHealth))
                Debug.Log("EnemyHealthが取得できていません");

            _enemyHealth.OnChangeHealth.Subscribe(value =>
            {
                ChangeHighlight(value / _maxHealth);
            });

        }

        private void ChangeHighlight(float _rate)
        {
            foreach (var mat in materials)
            {
                mat.SetColor("_RimColor", new Color(1, _rate, 0,1));
            }
        }
    }
}