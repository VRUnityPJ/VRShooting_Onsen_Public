using UnityEngine;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Player.Test
{
    public enum TestEffectDataType
    {
        VFX,
        Audio,
        ParticleSystem,
        ExplosionVFX,
        ExplosionAudio,
        ExplosionParticle,
        BoomSound,
        SampleSound,
        SampleVFX,
    }
    public class EffectDataTest : MonoBehaviour
    {
        [SerializeField] private GenericEffectData<TestEffectDataType> _effectData;
    }
}