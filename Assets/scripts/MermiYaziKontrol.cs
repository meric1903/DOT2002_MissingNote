using UnityEngine;
using TMPro;

public class MermiYaziKontrol : MonoBehaviour
{
    private TextMeshProUGUI mermiText;
    private Transform silahtutucuBellegi;

    void Awake()
    {
        mermiText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameObject tutucuObjesi = GameObject.Find("silahtutucu");
        if (tutucuObjesi != null)
        {
            silahtutucuBellegi = tutucuObjesi.transform;
        }
    }

    void Update()
    {
        bool sinematikOynuyor = NisanKontrol.Instance != null && NisanKontrol.Instance.sinematikOynuyor;
        if (Time.timeScale == 0f || sinematikOynuyor)
        {
            mermiText.enabled = false;
            return;
        }

        if (silahtutucuBellegi != null && silahtutucuBellegi.childCount > 0)
        {
            SilahKontrol aktifSilah = silahtutucuBellegi.GetChild(0).GetComponent<SilahKontrol>();
            
            if (aktifSilah != null)
            {
                mermiText.enabled = true;
                
                // DEĞİŞTİRİLDİ: Artık ikinci kısımda kapasiteyi değil, CEBİMİZDEKİ yedek mermiyi gösteriyor!
                mermiText.text = aktifSilah.GetMevcutMermi() + " / " + aktifSilah.GetToplamMermi();
            }
            else
            {
                mermiText.enabled = false;
            }
        }
        else
        {
            mermiText.enabled = false;
        }
    }
}