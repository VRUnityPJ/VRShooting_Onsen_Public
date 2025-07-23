using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Audio
{
    public class RandomAmbientSoundEffectPlayer : MonoBehaviour
    {
        /// <summary>
        /// 前の音がなり終わってから次の音が鳴るインターバルの最小大
        /// </summary>
        [SerializeField, MinMaxSlider(0, 100)]
        private Vector2Int _maxMinIntervalSec;

        /// <summary>
        /// 音量
        /// </summary>
        [SerializeField]
        private float _volume = 1.0f;

        private IAudioData<AmbientSoundEffectDataType> _seData;
        private CancellationToken _token;
        private AmbientSoundEffectDataType[] _allKeys;

        private void Start()
        {
            //必要な要素の取得
            if (!TryGetComponent(out _seData))
                Debug.Log("Dataが取得できません");
            _token = this.GetCancellationTokenOnDestroy();

            //keyの取得
            _allKeys = _seData.GetAllKeys();

            //開始
            Play(_token).Forget();
        }

        private async UniTaskVoid Play(CancellationToken token)
        {
            //最初に短時間のランダム時間待つ
            var initialDelaySeconds = (GetRandomIntervalTime() / 5);
            await UniTask.Delay(TimeSpan.FromSeconds(initialDelaySeconds), cancellationToken: token);

            for (;;)
            {
                //ランダムなSEを取得
                var se = GetRandomSe();
                //再生
                AudioPlayer.PlayOneShotAudioAtPoint(se, transform.position,_volume);
                //再生中のSEの長さを取得
                var lastSeDuration = (int)se.length;
                //待機秒数を計算
                var waitSeconds = GetRandomIntervalTime() + lastSeDuration;
                //待機
                await UniTask.Delay(TimeSpan.FromSeconds(waitSeconds), cancellationToken: token);
            }
        }

        /// <summary>
        /// ランダムなインターバル時間を取得する関数
        /// </summary>
        private int GetRandomIntervalTime()
        {
            //最大インターバル時間を含めたランダムな時間を取得
            var result = UnityEngine.Random.Range(_maxMinIntervalSec[0], _maxMinIntervalSec[1]+1);
            return result;
        }

        /// <summary>
        /// ランダムなSEを取得する関数
        /// </summary>
        private AudioClip GetRandomSe()
        {
            var keyIndex = UnityEngine.Random.Range(0, _allKeys.Length);
            var randomSe = _seData.GetAudioClip(_allKeys[keyIndex]);
            return randomSe;
        }
    }
}