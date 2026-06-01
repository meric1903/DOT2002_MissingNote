using System.Collections.Generic;
using UnityEngine;

public class EnvanterSistemi : MonoBehaviour
{
    // Singleton yapısı: Herhangi bir kod, "EnvanterSistemi.Instance..." yazarak buraya ulaşabilir.
    public static EnvanterSistemi Instance;

    [Header("Çanta Hafızası")]
    public List<EsyaVerisi> cantadakiEsyalar = new List<EsyaVerisi>();

    // Çantaya bir şey eklendiğinde UI'a haber vermek için bir tetikleyici (Event) oluşturuyoruz
    public delegate void EnvanterDegisti();
    public event EnvanterDegisti OnEnvanterDegisti;

    void Awake()
    {
        // Oyunda sadece 1 tane Envanter Sistemi olduğundan emin oluyoruz
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Yerdeki eşyayı aldığımızda PickupSystem bu fonksiyonu çağıracak
    public void Ekle(EsyaVerisi yeniEsya)
    {
        cantadakiEsyalar.Add(yeniEsya);
        
        // Çanta değişti! UI'a "Kendini güncelle" diye bağırıyoruz
        if (OnEnvanterDegisti != null)
        {
            OnEnvanterDegisti.Invoke();
        }
    }
}