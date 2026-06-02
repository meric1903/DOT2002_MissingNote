using UnityEngine;
using UnityEngine.UI;

public class EnvanterSlot : MonoBehaviour
{
    [Header("Eşya Verisi")]
    public EsyaVerisi secilenEsya; // Orijinal 'secilenEsya' verisi

    // Görselin butonun arkasında kalmasını engelleyen yardımcı tarayıcı
    private Image EnDogruIkonResminiBul()
    {
        Image[] resimler = GetComponentsInChildren<Image>(true);
        foreach (Image img in resimler)
        {
            if (img.gameObject != this.gameObject)
            {
                return img;
            }
        }
        return GetComponent<Image>();
    }

    // EnvanterUI.cs kodunun çağırdığı ve arayüzü dolduran fonksiyon
    public void SlotuDoldur(EsyaVerisi yeniEsya)
    {
        secilenEsya = yeniEsya;
        Image ikon = EnDogruIkonResminiBul();

        if (ikon != null && secilenEsya != null)
        {
            ikon.sprite = secilenEsya.esyaIkonu; // Orijinal 'esyaIkonu'
            ikon.type = Image.Type.Simple;
            ikon.preserveAspect = true; 
            ikon.enabled = true;
            ikon.color = Color.white; 

            // Tıklamanın arkadaki butona tık diye geçmesini sağlar
            ikon.raycastTarget = false; 
        }
    }

    // EnvanterUI.cs kodunun çağırdığı ve slotu temizleyen fonksiyon
    public void SlotuTemizle()
    {
        secilenEsya = null;
        Image ikon = EnDogruIkonResminiBul();

        if (ikon != null)
        {
            ikon.sprite = null;
            
            if (ikon.gameObject != this.gameObject)
            {
                ikon.enabled = false;
            }
            else
            {
                ikon.type = Image.Type.Simple;
                ikon.color = Color.white;
            }
        }
    }

    /// <summary>
    /// İSTEDİĞİN ESKİ HALİ: Çantada slota tıklandığında çalışan orijinal fonksiyon
    /// </summary>
    public void SlotaTiklandi()
    {
        if (secilenEsya == null) return;

        // EsyaVerisi.cs içindeki 'tur' ve 'EsyaTuru.Silah' kontrolü
        if (secilenEsya.tur == EsyaTuru.Silah) 
        {
            if (EkipmanSistemi.Instance != null)
            {
                // EsyaVerisi.cs içindeki orijinal 'esyaPrefab'
                EkipmanSistemi.Instance.SilahKusan(secilenEsya.esyaPrefab);
            }
        }
        // EsyaVerisi.cs içindeki 'EsyaTuru.Tuketilebilir' kontrolü
        else if (secilenEsya.tur == EsyaTuru.Tuketilebilir)
        {
            Debug.Log("Tüketilebilir eşya kullanıldı: " + secilenEsya.esyaAdi);
        }
    }
}