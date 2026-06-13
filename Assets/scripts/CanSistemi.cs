using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değişimi için şart

public class CanSistemi : MonoBehaviour
{
    public static CanSistemi Instance;

    [Header("Can Ayarları")]
    public float maksimumCan = 100f;
    private float mevcutCan;

    [Header("Ölüm Ekranı (Baran Tasarlayınca Buraya Sürükle)")]
    public GameObject oyunBittiPaneli; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        mevcutCan = maksimumCan;
        
        // Oyun başında panelin kapalı olduğundan emin olalım
        if (oyunBittiPaneli != null) oyunBittiPaneli.SetActive(false);
    }

    public void HasarAl(float hasarMiktari)
    {
        if (mevcutCan <= 0) return; 

        mevcutCan -= hasarMiktari;
        Debug.Log("Karakter Hasar Aldı! Kalan Can: " + mevcutCan);

        if (mevcutCan <= 0)
        {
            mevcutCan = 0;
            KarakterOldu();
        }
    }

    public void CanEkle(float iyilestirmeMiktari)
    {
        if (mevcutCan <= 0) return; 
        mevcutCan += iyilestirmeMiktari;
        if (mevcutCan > maksimumCan) mevcutCan = maksimumCan;
    }

    private void KarakterOldu()
    {
        Debug.Log("KARAKTER ÖLDÜ! Oyun donduruluyor.");

        // 1. Oyunu dondur
        Time.timeScale = 0f;

        // 2. Baran'ın panelini aktif et
        if (oyunBittiPaneli != null)
        {
            oyunBittiPaneli.SetActive(true);
        }

        // 3. Farenin kilitlenmesini kaldır (Butonlara tıklayabilmen için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Butona basınca çalışacak fonksiyon
    public void OyunuYenidenBaslat()
    {
        Time.timeScale = 1f; // Zamanı geri başlat
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Mevcut sahneyi yeniden yükle
    }

    public float GetMevcutCan() { return mevcutCan; }
    public float GetMaksimumCan() { return maksimumCan; }
    public bool CanIhtiyaciVarMi() { return mevcutCan < maksimumCan; }
}