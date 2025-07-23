using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VRShooting.Scripts.Enemy.Interfaces;

namespace Onogawa.Scripts.Enemy.Bat
{
    public class BatVisualController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _eyeRenderer;
        [SerializeField] private MeshRenderer _bodyRenderer;
        [SerializeField] private SkinnedMeshRenderer _wingRenderer;
        [SerializeField] private Material _redEyeMaterial;

        private Material defaultMaterial;
        private IBatStateController _batStateController;
        private TrailRenderer _trail;

        private void Start()
        {
            if(!TryGetComponent(out _batStateController))
                Debug.Log("BatStateControllerが取得できません");

            if(!TryGetComponent(out _trail))
                Debug.Log("TrailRendererが取得できません");

            defaultMaterial = _eyeRenderer.materials[0];
            _trail.enabled = false;

            // _batStateController.OnEnterIdle += SetDefaultEye;
            _batStateController.OnEnterIdle += ResetBodyRimLight;
            _batStateController.OnEnterIdle += ResetWingRimLight;
            _batStateController.OnEnterIdle += () => _trail.enabled = false;

            // _batStateController.OnEnterHold += SetRedEye;
            _batStateController.OnEnterHold += SetBodyRimLight;
            _batStateController.OnEnterHold += SetWingRimLight;
            _batStateController.OnEnterHold += () => _trail.enabled = true;
        }

        private void SetRedEye()
        {
            var mats = new Material[1];
            mats[0] = _redEyeMaterial;
            _eyeRenderer.materials = mats;
        }

        private void SetDefaultEye()
        {
            var mats = new Material[1];
            mats[0] = defaultMaterial;
            _eyeRenderer.materials = mats;
        }

        private void SetBodyRimLight()
        {
            _bodyRenderer.materials[0]?.SetColor("_LightColor",Color.red);
        }

        private void ResetBodyRimLight()
        {
            _bodyRenderer.materials[0]?.SetColor("_LightColor",Color.black);
        }

        private void SetWingRimLight()
        {
            _wingRenderer.materials[0]?.SetColor("_LightColor",Color.red);
        }

        private void ResetWingRimLight()
        {
            _wingRenderer.materials[0]?.SetColor("_LightColor",Color.black);
        }

    }
}