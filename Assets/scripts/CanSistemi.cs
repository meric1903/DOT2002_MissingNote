using UnityEngine;

public class CanSistemi : MonoBehaviour
{
    // Hayvanların ve sağlık çantalarının bu koda dışarıdan kolayca ulaşabilmesi için Singleton
    public static CanSistemi Instance;

    [Header("Can Ayarları")]
    public float maksimumCan = 100f;
    private float mevcutCan;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Oyun başladığında canı fulle
        mevcutCan = maksimumCan;
    }

    /// <summary>
    /// Vahşi hayvanlar (Ayı, Kurt) bize saldırdığında bu fonksiyonu çağıracak
    /// </summary>
    /// <param name="hasarMiktari">Hayvanın vuruş gücü</param>
    public void HasarAl(float hasarMiktari)
    {
        if (mevcutCan <= 0) return; // Karakter zaten öldüyse çalışma

        mevcutCan -= hasarMiktari;
        Debug.Log("Karakter Hasar Aldı! Kalan Can: " + mevcutCan);

        // Can sıfırın altına düşerse ölme fonksiyonunu tetikle
        if (mevcutCan <= 0)
        {
            mevcutCan = 0;
            KarakterOldu();
        }
    }

    /// <summary>
    /// Sağlık çantası kullanıldığında canı yenileyen fonksiyon
    /// </summary>
    public void CanEkle(float iyilestirmeMiktari)
    {
        if (mevcutCan <= 0) return; // Ölüye can basılamaz

        mevcutCan += iyilestirmeMiktari;
        
        // Canın maksimum sınırı aşmasını engelle
        if (mevcutCan > maksimumCan)
        {
            mevcutCan = maksimumCan;
        }

        Debug.Log("Can Yenilendi! Mevcut Can: " + mevcutCan);
    }

    private void KarakterOldu()
    {
        Debug.LogError("KARAKTER ÖLDÜ! Oyun bitti ekranı buraya bağlanacak.");
        // İleride buraya ölüm animasyonu veya "Yeniden Başla" paneli ekleyeceğiz
    }

    // Arayüzün (UI) can barını doldurabilmesi için gerekli köprü fonksiyonlar
    public float GetMevcutCan() { return mevcutCan; }
    public float GetMaksimumCan() { return maksimumCan; }
    public bool CanIhtiyaciVarMi() { return mevcutCan < maksimumCan; }
}