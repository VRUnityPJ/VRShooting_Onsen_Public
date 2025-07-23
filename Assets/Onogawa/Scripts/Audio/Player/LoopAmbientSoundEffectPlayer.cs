using System;
using UnityEngine;
using VRShooting.Scripts.Utility;

namespace Onogawa.Scripts.Audio
{
    /// <summary>
    /// 一つの環境音をループさせてPlayするクラス
    /// </summary>
    public class LoopAmbientSoundEffectPlayer : MonoBehaviour
    {
        [SerializeField] private AmbientSoundEffectDataType _seType;
        private IAudioData<AmbientSoundEffectDataType> _seData;

        private void Start()
        {
            if(!TryGetComponent(out _seData))
                Debug.Log("AudioDataが取得できません");

            var audioSource = AudioPlayer.PlayLoopAudioAtPoint(_seData.GetAudioClip(_seType), transform.position);
        }
    }
}