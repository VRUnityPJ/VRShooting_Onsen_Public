using UnityEngine;
/// <summary>
/// カメラの位置をリムライティングマテリアルにセットするクラス
/// カメラにアタッチする
/// </summary>
public class CameraPositionSetter : MonoBehaviour
{
    [SerializeField]private Material mat;
    void Update()
    {
        var pos = Camera.main.transform.position; 
        // mat.SetVector("_CameraPos",trans.position);
        mat.SetFloat("_CameraPosX", pos.x);
        mat.SetFloat("_CameraPosY", pos.y);
        mat.SetFloat("_CameraPosZ", pos.z);
    }
}
