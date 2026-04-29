using UnityEngine;
using System.Collections;
using Unity.Cinemachine; // Arkadaþýnýn kameralarý için gerekli

public class SinematikYonetici : MonoBehaviour
{
    [Header("Zaman Ayarý")]
    public float sinematikSuresi = 15f;

    [Header("Objeler")]
    public GameObject anaKarakter; // Senin oynadýðýn karakter
    public GameObject sinematikSistemi; // Ýçinde arkadaþýnýn kameralarý olan grup

    void Start()
    {
        // Oyun baþlar baþlamaz senaryoyu baþlat
        StartCoroutine(SinematikSenaryosu());
    }

    IEnumerator SinematikSenaryosu()
    {
        // --- 1. AÞAMA: SÝNEMATÝK BAÞLIYOR ---

        // Kendi karakterini tamamen gizle (Görünmez olur ve tuþlar/hareket iptal olur)
        if (anaKarakter != null) anaKarakter.SetActive(false);

        // Ana kameradaki kendi özel kodunu (CameraFollow) kapat ki Cinemachine ile çakýþmasýn
        CameraFollow ozelKameram = Camera.main.GetComponent<CameraFollow>();
        if (ozelKameram != null) ozelKameram.enabled = false;


        // --- 2. AÞAMA: BEKLEME ---

        // Belirlediðin süre kadar (15 saniye) hiçbir þey yapmadan bekle (Sinematik oynuyor)
        yield return new WaitForSeconds(sinematikSuresi);


        // --- 3. AÞAMA: SÝNEMATÝK BÝTTÝ, OYUN BAÞLIYOR ---

        // Arkadaþýnýn sinematik sistemini (kameralarý) tamamen kapat
        if (sinematikSistemi != null) sinematikSistemi.SetActive(false);

        // Ana kameranýn beynini (Cinemachine Brain) kapat ki kamera serbest kalsýn
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null) brain.enabled = false;

        // Kendi karakterini görünür yap ve mekanikleri geri aç
        if (anaKarakter != null) anaKarakter.SetActive(true);

        // Ana kameradaki kendi özel kodunu tekrar aktif et ki kameran çalýþsýn
        if (ozelKameram != null) ozelKameram.enabled = true;

        Debug.Log("Sinematik bitti, kontrol sende!");
    }
}