using System.Collections.Generic;
using UnityEngine;

// Çantada hem eşyanın kendisini hem de kaç adet olduğunu tutan akıllı yeni paket şablonu
[System.Serializable]
public class EnvanterEsyasi
{
    public EsyaVerisi veri;
    public int adet;

    public EnvanterEsyasi(EsyaVerisi yeniVeri, int ilkAdet)
    {
        veri = yeniVeri;
        adet = ilkAdet;
    }
}

public class EnvanterSistemi : MonoBehaviour
{
    public static EnvanterSistemi Instance;

    [Header("Çanta Hafızası")]
    // Artık düz liste yerine adet bilgisini de taşıyan yeni listemizi kullanıyoruz
    public List<EnvanterEsyasi> cantadakiEsyalar = new List<EnvanterEsyasi>();

    public delegate void EnvanterDegisti();
    public event EnvanterDegisti OnEnvanterDegisti;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Yerdeki eşya alındığında tetiklenen fonksiyon
    public void Ekle(EsyaVerisi yeniEsya)
    {
        // Çantada bu eşyadan zaten var mı diye kontrol et
        EnvanterEsyasi varOlan = cantadakiEsyalar.Find(x => x.veri == yeniEsya);

        if (varOlan != null)
        {
            // Eğer varsa yeni slot açma, sadece adetini 1 artır! (İstifleme)
            varOlan.adet++;
        }
        else
        {
            // Eğer çantada ilk defa görüyorsak listede yeni yuva aç ve adetini 1 yap
            cantadakiEsyalar.Add(new EnvanterEsyasi(yeniEsya, 1));
        }
        
        if (OnEnvanterDegisti != null)
        {
            OnEnvanterDegisti.Invoke();
        }
    }

    // Sağlık kiti basıldığında çantadaki adeti azaltan yeni yardımcı fonksiyonumuz
    public void EsyaAdetDus(EsyaVerisi esya)
    {
        EnvanterEsyasi varOlan = cantadakiEsyalar.Find(x => x.veri == esya);
        if (varOlan != null)
        {
            varOlan.adet--;
            
            // Eğer adet sıfıra düştüyse çantadaki o slotu tamamen temizle
            if (varOlan.adet <= 0)
            {
                cantadakiEsyalar.Remove(varOlan);
            }
            
            if (OnEnvanterDegisti != null)
            {
                OnEnvanterDegisti.Invoke();
            }
        }
    }
}