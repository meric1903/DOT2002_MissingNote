using UnityEngine;
using UnityEngine.UI;
using TMPro; // Sayı yazısını kontrol etmek için gerekli kütüphane

public class EnvanterSlot : MonoBehaviour
{
    [Header("Eşya Verisi")]
    public EsyaVerisi secilenEsya; 

    // Görselin butonun arkasında kalmasını engelleyen orijinal tarayıcın
    private Image EnDogruIkonResminiBul()
    {
        Image[] resimler = GetComponentsInChildren<Image>(true);
        foreach (Image img in resimler)
        {
            if (img.gameObject != this.gameObject) return img;
        }
        return GetComponent<Image>();
    }

    // Slotun sağ altına ekleyeceğimiz sayı metnini otomatik bulan yeni yardımcı fonksiyon
    private TMP_Text EnDogruAdetYazisiniBul()
    {
        return GetComponentInChildren<TMP_Text>(true);
    }

    // Yeni adet sistemine göre revize edilen doldurma fonksiyonu
    public void SlotuDoldur(EnvanterEsyasi cantaEsyasi)
    {
        secilenEsya = cantaEsyasi.veri;
        Image ikon = EnDogruIkonResminiBul();
        TMP_Text adetYazisi = EnDogruAdetYazisiniBul();

        // 1. İKON AYARI
        if (ikon != null && secilenEsya != null)
        {
            ikon.sprite = secilenEsya.esyaIkonu; 
            ikon.type = Image.Type.Simple;
            ikon.preserveAspect = true; 
            ikon.enabled = true;
            ikon.color = Color.white; 
            ikon.raycastTarget = false; 
        }

        // 2. ADET SAYISI AYARI
        if (adetYazisi != null)
        {
            // Eğer eşya adeti 1'den büyükse sağ altta adeti göster
            if (cantaEsyasi.adet > 1)
            {
                adetYazisi.text = cantaEsyasi.adet.ToString();
                adetYazisi.enabled = true;
            }
            else
            {
                // 1 taneyse sayı kalabalığı yapmasın diye gizle
                adetYazisi.enabled = false;
            }
        }
    }

    public void SlotuTemizle()
    {
        secilenEsya = null;
        Image ikon = EnDogruIkonResminiBul();
        TMP_Text adetYazisi = EnDogruAdetYazisiniBul();

        if (ikon != null)
        {
            ikon.sprite = null;
            if (ikon.gameObject != this.gameObject) ikon.enabled = false;
            else { ikon.type = Image.Type.Simple; ikon.color = Color.white; }
        }

        if (adetYazisi != null)
        {
            adetYazisi.enabled = false; // Temizlenince sayıyı da kapat
        }
    }

    // Orijinal 'SlotaTiklandi' buton fonksiyonun
    public void SlotaTiklandi()
    {
        if (secilenEsya == null) return;

        // 1. DURUM: SİLAH İSE
        if (secilenEsya.tur == EsyaTuru.Silah) 
        {
            if (EkipmanSistemi.Instance != null)
            {
                EkipmanSistemi.Instance.SilahKusan(secilenEsya.esyaPrefab);
            }
        }
        // 2. DURUM: TÜKETİLEBİLİR (SAĞLIK ÇANTASI) İSE
        else if (secilenEsya.tur == EsyaTuru.Tuketilebilir)
        {
            if (CanSistemi.Instance != null)
            {
                // Can barımız maksimum değerden az ise (Yani cana ihtiyaç varsa)
                if (CanSistemi.Instance.CanIhtiyaciVarMi())
                {
                    // Can barına 25 birim can ekle
                    CanSistemi.Instance.CanEkle(25f);
                    
                    // Sağlık çantasını tükettiğimiz için adetini 1 düşür
                    EnvanterSistemi.Instance.EsyaAdetDus(secilenEsya);
                }
                else
                {
                    Debug.Log("Canın zaten tamamen dolu, sağlık çantasını harcayamazsın!");
                }
            }
        }
    }
}