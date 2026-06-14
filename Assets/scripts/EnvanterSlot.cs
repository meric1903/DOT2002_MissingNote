using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class EnvanterSlot : MonoBehaviour
{
    [Header("Eşya Verisi")]
    public EsyaVerisi secilenEsya; 

    private Image EnDogruIkonResminiBul()
    {
        Image[] resimler = GetComponentsInChildren<Image>(true);
        foreach (Image img in resimler)
        {
            if (img.gameObject != this.gameObject) return img;
        }
        return GetComponent<Image>();
    }

    private TMP_Text EnDogruAdetYazisiniBul()
    {
        return GetComponentInChildren<TMP_Text>(true);
    }

    public void SlotuDoldur(EnvanterEsyasi cantaEsyasi)
    {
        secilenEsya = cantaEsyasi.veri;
        Image ikon = EnDogruIkonResminiBul();
        TMP_Text adetYazisi = EnDogruAdetYazisiniBul();

        if (ikon != null && secilenEsya != null)
        {
            ikon.sprite = secilenEsya.esyaIkonu; 
            ikon.type = Image.Type.Simple;
            ikon.preserveAspect = true; 
            ikon.enabled = true;
            ikon.color = Color.white; 
            ikon.raycastTarget = false; 
        }

        if (adetYazisi != null)
        {
            if (cantaEsyasi.adet > 1)
            {
                adetYazisi.text = cantaEsyasi.adet.ToString();
                adetYazisi.enabled = true;
            }
            else
            {
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
            adetYazisi.enabled = false; 
        }
    }

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
        // 2. DURUM: TÜKETİLEBİLİR İSE (Mermi Kutusu veya Sağlık Çantası)
        else if (secilenEsya.tur == EsyaTuru.Tuketilebilir)
        {
            // EĞER TIKLANAN EŞYA MERMİ KUTUSUYSA:
            if (secilenEsya.esyaAdi == "Mermi Kutusu")
            {
                GameObject silahTutucu = GameObject.Find("silahtutucu"); 
                
                if (silahTutucu != null && silahTutucu.transform.childCount > 0)
                {
                    SilahKontrol aktifSilah = silahTutucu.transform.GetChild(0).GetComponent<SilahKontrol>();
                    
                    if (aktifSilah != null)
                    {
                        int verilecekMermi = 10; // Kutudan çıkacak mermi sayısı
                        
                        aktifSilah.toplamMermi += verilecekMermi;
                        SilahKontrol.hafizaToplamMermi += verilecekMermi; 
                        
                        Debug.Log("<color=green>Şarjöre " + verilecekMermi + " mermi eklendi!</color>");
                        
                        // Mermiyi çantadan düş
                        EnvanterSistemi.Instance.EsyaAdetDus(secilenEsya);
                    }
                }
            }
            // EĞER TIKLANAN EŞYA SAĞLIK ÇANTASIYSA:
            else 
            {
                if (CanSistemi.Instance != null)
                {
                    if (CanSistemi.Instance.CanIhtiyaciVarMi())
                    {
                        CanSistemi.Instance.CanEkle(25f);
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
}