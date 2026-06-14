using UnityEngine;

public class FinalNotKontrol : MonoBehaviour
{
    [Header("Final Notu Ayarları")]
    public GameObject oyunSonuPaneli; // Inspector'dan 'OyunSonu' objeni buraya at

    void Start()
    {
        if (oyunSonuPaneli != null) oyunSonuPaneli.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu küpe girdiği an çalışır
        if (other.CompareTag("Player"))
        {
            if (oyunSonuPaneli != null)
            {
                oyunSonuPaneli.SetActive(true);
                Time.timeScale = 0f; // Oyunu dondur
                
                Cursor.lockState = CursorLockMode.None; // Fareyi serbest bırak
                Cursor.visible = true;
            }
        }
    }
}