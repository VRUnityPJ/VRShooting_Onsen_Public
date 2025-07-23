using System;
using UnityEngine;

namespace VRShooting.Shader
{
    public class CameraPositionGetter : MonoBehaviour
    {
        private Material mat;

        private void Start()
        {
            mat = GetComponent<MeshRenderer>().materials[1];
        }

        private void Update()
        {
            var pos = Camera.main.transform.position; 
            // mat.SetVector("_CameraPos",); 
            mat.SetFloat("_CameraPosX", pos.x);
            mat.SetFloat("_CameraPosY", pos.y);
            mat.SetFloat("_CameraPosZ", pos.z);
        }
    }
}