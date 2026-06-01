using UnityEngine;
using UnityEngine.UI; // Arayüz işlemleri için gerekli

public class EnvanterSlot : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Image ikon; // Eşyanın resmini göstereceğimiz bileşen

    private EsyaVerisi icindekiEsya;

    // Depodan gelen veriyi kutuya çizen fonksiyon
    public void SlotuDoldur(EsyaVerisi yeniEsya)
    {
        icindekiEsya = yeniEsya;
        ikon.sprite = icindekiEsya.esyaIkonu;
        ikon.enabled = true; // İkonu görünür yap
    }

    // Kutuyu boşaltan fonksiyon
    public void SlotuTemizle()
    {
        icindekiEsya = null;
        ikon.sprite = null;
        ikon.enabled = false; // İkonu gizle
    }
}