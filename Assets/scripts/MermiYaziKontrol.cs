using UnityEngine;
using TMPro;

public class MermiYaziKontrol : MonoBehaviour
{
    private TextMeshProUGUI mermiText;
    
    [Header("Bağlantılar")]
    [Tooltip("Karakterin elindeki silahın oluşturulduğu asıl yuvayı (Örn: SilahTutucu objesini) buraya sürükleyin")]
    public Transform silahtutucuBellegi; 

    void Awake()
    {
        mermiText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Sinematik oynuyorsa yazıyı gizle
        bool sinematikOynuyor = NisanKontrol.Instance != null && NisanKontrol.Instance.sinematikOynuyor;
        if (Time.timeScale == 0f || sinematikOynuyor)
        {
            mermiText.enabled = false;
            return;
        }

        // Silah tutucu yuvası dolu mu? (İçinde silah var mı?)
        if (silahtutucuBellegi != null && silahtutucuBellegi.childCount > 0)
        {
            // Yuvadaki ilk objenin (silahın) içindeki mermi kodunu al
            SilahKontrol aktifSilah = silahtutucuBellegi.GetChild(0).GetComponent<SilahKontrol>();
            
            if (aktifSilah != null)
            {
                // Her şey tamsa yazıyı GÖRÜNÜR YAP ve mermiyi yaz!
                mermiText.enabled = true;
                mermiText.text = aktifSilah.GetMevcutMermi() + " / " + aktifSilah.GetToplamMermi();
            }
            else
            {
                // Silah var ama kodu yoksa gizle
                mermiText.enabled = false;
            }
        }
        else
        {
            // Silah tutucu yuvası boşsa yazıyı gizle
            mermiText.enabled = false;
        }
    }
}