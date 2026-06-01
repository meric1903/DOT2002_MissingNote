using UnityEngine;
using UnityEngine.UI;

public class EnvanterSlot : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Image ikon;

    private EsyaVerisi icindekiEsya;

    public void SlotuDoldur(EsyaVerisi yeniEsya)
    {
        icindekiEsya = yeniEsya;
        ikon.sprite = icindekiEsya.esyaIkonu;
        ikon.enabled = true;
    }

    public void SlotuTemizle()
    {
        icindekiEsya = null;
        ikon.sprite = null;
        ikon.enabled = false;
    }

    // YENİ EKLENDİ: Faremizle kutuya tıkladığımızda çalışacak kod
    public void SlotaTiklandi()
    {
        if (icindekiEsya != null)
        {
            // Eğer tıklanan şey bir silahsa, Ekipman sistemine haber ver!
            if (icindekiEsya.tur == EsyaTuru.Silah)
            {
                EkipmanSistemi.Instance.SilahKusan(icindekiEsya);
            }
            else
            {
                Debug.Log(icindekiEsya.esyaAdi + " kullanıldı!"); // İleride sağlık kiti için burayı yazacağız
            }
        }
    }
}