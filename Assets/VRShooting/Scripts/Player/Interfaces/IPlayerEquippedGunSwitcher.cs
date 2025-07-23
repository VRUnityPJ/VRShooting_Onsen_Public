using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System;
using VRShooting.Scripts.Gun;
namespace VRShooting.Scripts.Player
{
    public interface IPlayerEquippedGunSwitcher
    {
       public void AddGun(GunVariant gunName,IGunObject gunInstance) ;
    }
}

