using UnityEngine;
using UnityEngine.InputSystem;

namespace VRShooting.Scripts.Gun.Test
{
    public class GunObjectTester : MonoBehaviour
    {
        public GunBaseObject _gunBaseObject;

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _gunBaseObject.Shoot();
            }
        }
    }
}