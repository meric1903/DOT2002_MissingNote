using UnityEngine;

public class FlareProjectile : MonoBehaviour {
    void Start() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);
        Destroy(gameObject, 5f); // 5 saniye sonra yok et
    }
}