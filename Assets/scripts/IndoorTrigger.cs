using UnityEngine;

public class IndoorTrigger : MonoBehaviour
{
    [Header("Ayarlar")]
    public string playerTag = "Player"; // Karakterinin etiketi

    // Karakter kutunun ÝÇÝNE GÝRDÝÐÝNDE çalýþýr
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Ana kameradaki kendi kodumuz olan CameraFollow'u bul ve Ýç Mekan modunu aktif et
            CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
            if (camScript != null)
            {
                camScript.SetIndoorMode(true);
            }
        }
    }

    // Karakter kutudan ÇIKTIÐINDA çalýþýr
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Ana kameradaki kodu bul ve Ýç Mekan modunu kapat (Dýþarýya dön)
            CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
            if (camScript != null)
            {
                camScript.SetIndoorMode(false);
            }
        }
    }
}