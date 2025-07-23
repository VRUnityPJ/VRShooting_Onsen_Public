using UnityEngine;
using VRShooting.Scripts.Utility;

namespace VRShooting.Scripts.Gun.Test
{
    public enum DataType
    {
        Range,
        FireRate,
    }
    public class GunDataTest : MonoBehaviour
    {
        [SerializeField] private GenericWeaponData<DataType> _weaponData;

        private void Start()
        {
            Debug.Log($"Get Value : Float : {DataType.Range} :{_weaponData.GetFloatValue(DataType.Range)}");
            Debug.Log($"Get Value : Float : {DataType.FireRate} :{_weaponData.GetFloatValue(DataType.FireRate)}");
            
            Debug.Log($"Get Value : Int : {DataType.Range} :{_weaponData.GetIntValue(DataType.Range)}");
            Debug.Log($"Get Value : Int : {DataType.FireRate} :{_weaponData.GetIntValue(DataType.FireRate)}");
            
            Debug.Log($"Get Value : Bool : {DataType.Range} :{_weaponData.GetBoolValue(DataType.Range)}");
            Debug.Log($"Get Value : Bool : {DataType.FireRate} :{_weaponData.GetBoolValue(DataType.FireRate)}");
        }
    }
}