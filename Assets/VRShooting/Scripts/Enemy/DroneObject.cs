using System;
using UnityEngine;
using VRShooting.Scripts.Enemy.Drone.DataType;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Enemy
{


    public class DroneObject : MonoBehaviour, IEnemyObject
    {
        private IEffectData _effectData;

        private void Start()
        {

            // EffectDataの取り出し
            if(!TryGetComponent(out _effectData))
                Debug.Log("EffectDataを取得できません。");

            // スポーンしたときの初期処理
            SpawnInitialize();
        }

        public void SpawnInitialize()
        {
            // EnemyStorage.instance.Add(this);

            // VFXの取得
            var spawnEffect = _effectData.GetParticleSystem(DroneEnemyEffectDataType.Spawn);
            // VFXの再生
            Instantiate(spawnEffect, transform.position, Quaternion.identity);

            // サウンドの取得
            var spawnSFX = _effectData.GetAudioClip(DroneEnemyEffectDataType.Spawn);
            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(spawnSFX, transform.position);
        }

        public void OnDestroy()
        {
            // EnemyStorage.instance?.Remove(this);
        }
    }
}