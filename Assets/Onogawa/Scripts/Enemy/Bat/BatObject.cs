using System;
using Onogawa.Scripts.Enemy.Bat;
using UnityEngine;
using VRShooting.Scripts.Enemy.Interfaces;
using VRShooting.Scripts.Gun;
using VRShooting.Scripts.Stage;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Enemy
{


    public class BatObject : MonoBehaviour, IEnemyObject
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
            var spawnEffect = _effectData.GetParticleSystem(BatEffectDataType.Spawn);
            // VFXの再生
            Instantiate(spawnEffect, transform.position, Quaternion.identity);

            // サウンドの取得
            var spawnSFX = _effectData.GetAudioClip(BatEffectDataType.Spawn);
            // サウンドの再生
            AudioPlayer.PlayOneShotAudioAtPoint(spawnSFX, transform.position);
        }

        public void OnDestroy()
        {
            // EnemyStorage.instance?.Remove(this);
        }
    }
}