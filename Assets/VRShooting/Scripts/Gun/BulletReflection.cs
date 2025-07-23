using UnityEngine;

public class BulletReflection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        var rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;
        
        // 入射ベクトル（速度）
        var inDirection = rb.velocity;
        // 法線ベクトル
        var inNormal = transform.up;
        // 反射ベクトル（速度）
        var result = Vector3.Reflect(inDirection, inNormal);
        
        rb.velocity = result;
    }
    //壁や地面にアタッチするもので、銃や弾にはアタッチしない
}
