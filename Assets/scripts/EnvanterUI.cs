using UnityEngine;
using UnityEngine.InputSystem;

public class EnvanterUI : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public GameObject cantaPaneli;
    public Transform slotGrupObjesi;

    private EnvanterSlot[] slotlar;
    
    // Çantanın açık/kapalı durumunu takip edeceğimiz anahtar
    private bool cantaAcikMi = false; 

    void Start()
    {
        cantaPaneli.SetActive(false);
        slotlar = slotGrupObjesi.GetComponentsInChildren<EnvanterSlot>();
        EnvanterSistemi.Instance.OnEnvanterDegisti += ArayuzuGuncelle;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            // Durumu tersine çevir (Açıksa kapat, kapalıysa aç)
            cantaAcikMi = !cantaAcikMi;
            cantaPaneli.SetActive(cantaAcikMi);

            if (cantaAcikMi)
            {
                // ÇANTA AÇILDI: Oyunu durdur ve fareyi serbest bırak
                Time.timeScale = 0f; // Zamanı dondurur (hareket ve animasyonlar durur)
                Cursor.visible = true; // Fare ikonunu görünür yap
                Cursor.lockState = CursorLockMode.None; // Fareyi ekranın ortasına kilitlemeyi bırak
            }
            else
            {
                // ÇANTA KAPANDI: Oyunu devam ettir ve fareyi gizle
                Time.timeScale = 1f; // Zamanı normale döndür
                Cursor.visible = false; // Fareyi gizle
                Cursor.lockState = CursorLockMode.Locked; // Fareyi tekrar FPS moduna (ekran ortasına) kilitle
            }
        }
    }

    public void ArayuzuGuncelle()
    {
        for (int i = 0; i < slotlar.Length; i++)
        {
            slotlar[i].SlotuTemizle();
        }

        for (int i = 0; i < EnvanterSistemi.Instance.cantadakiEsyalar.Count; i++)
        {
            if (i < slotlar.Length) 
            {
                slotlar[i].SlotuDoldur(EnvanterSistemi.Instance.cantadakiEsyalar[i]);
            }
        }
    }
}