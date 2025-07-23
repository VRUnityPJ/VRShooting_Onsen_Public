using System;
using UnityEngine;
using UnityEngine.VFX;

namespace VRShooting.Scripts.Gun
{
    public interface IEffectData
    {
        /// <summary>
        /// VFX Graphで作成されたVFXを取得する
        /// </summary>
        /// <param name="key">データを取り出すためのキー</param>
        /// <typeparam name="TKey">キーの型。Enum型で指定する</typeparam>
        /// <returns>Visual Effect</returns>
        public VisualEffect GetVisualEffect<TKey>(TKey key) where TKey : Enum;
        /// <summary>
        /// Particle Systemで作成されたVFXを取得する
        /// </summary>
        /// <param name="key">データを取り出すためのキー</param>
        /// <typeparam name="TKey">キーの型。Enum型で指定する</typeparam>
        /// <returns>ParticleSystem</returns>
        public ParticleSystem GetParticleSystem<TKey>(TKey key) where TKey : Enum;
        /// <summary>
        /// AudioClipを取得する
        /// </summary>
        /// <param name="key">データを取り出すためのキー</param>
        /// <typeparam name="TKey">キーの型。Enum型で指定する</typeparam>
        /// <returns>AudioClip</returns>
        public AudioClip GetAudioClip<TKey>(TKey key) where TKey : Enum;
    }
}