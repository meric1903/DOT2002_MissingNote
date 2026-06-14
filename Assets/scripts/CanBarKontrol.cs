using UnityEngine;
using UnityEngine.UI;

public class CanBarKontrol : MonoBehaviour
{
    [Header("Sinematik Takibi")]
    // Sahnendeki 'sinematik' objesini buraya bağlayabilirsin (Boş bırakırsan otomatik arar)
    public GameObject sinematikObjesi; 

    private Slider canSlideri;
    private bool gorsellerAcikMi = true;

    void Start()
    {
        canSlideri = GetComponent<Slider>();

        // Eğer Inspector'dan elinle sürüklemediysen, sahnede ismi "sinematik" olan objeyi otomatik bulur
        if (sinematikObjesi == null)
        {
            sinematikObjesi = GameObject.Find("sinematik");
        }
    }

    void Update()
    {
        // 🎬 SİNEMATİK KONTROLÜ:
        // Eğer sahnede sinematik objesi varsa ve şu an AKTİFSE (oynuyorsa) can barını gizle
        if (sinematikObjesi != null && sinematikObjesi.activeInHierarchy)
        {
            CanBariniGoster(false);
            return; // Kodun aşağıya devam edip barı güncellemesini engelle
        }

        // Sinematik bittiyse (PASİF olduysa) can barını görünür yap ve canı anlık güncelle
        CanBariniGoster(true);

        if (canSlideri != null && CanSistemi.Instance != null)
        {
            canSlideri.maxValue = CanSistemi.Instance.GetMaksimumCan();
            canSlideri.value = CanSistemi.Instance.GetMevcutCan();
        }
    }

    // Can barının altındaki tüm görselleri tek tek kapatıp açan güvenli fonksiyon
    private void CanBariniGoster(bool durum)
    {
        if (gorsellerAcikMi == durum) return; // Zaten istenen durumdaysa gereksiz çalışma

        Image[] gorseller = GetComponentsInChildren<Image>(true);
        foreach (Image img in gorseller)
        {
            img.enabled = durum;
        }
        gorsellerAcikMi = durum;
    }
}