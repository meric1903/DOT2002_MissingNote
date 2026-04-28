using UnityEngine;

public class KapiOtomasyon : MonoBehaviour
{
    public Animation kapiAnimasyonu;
    private bool acildi = false;

    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu gelince ve kapý daha önce açýlmadýysa
        if (other.CompareTag("Player") && !acildi)
        {
            // Animasyonu oynat
            kapiAnimasyonu.Play("Kapi_Acil");

            // Animasyon bittikten sonra kapýnýn eski haline dönmesini engelle
            kapiAnimasyonu["Kapi_Acil"].wrapMode = WrapMode.ClampForever;

            acildi = true; // Kapýyý açýk olarak iþaretle
        }
    }
}