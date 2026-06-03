using UnityEngine;
using System.Collections;
using Unity.Cinemachine; 

public class SinematikYonetici : MonoBehaviour
{
    [Header("Zaman Ayarı")]
    public float sinematikSuresi = 15f;

    [Header("Objeler")]
    public GameObject anaKarakter; 
    public GameObject sinematikSistemi; 

    void Start()
    {
        StartCoroutine(SinematikSenaryosu());
    }

    IEnumerator SinematikSenaryosu()
    {
        // --- 1. AŞAMA: SİNEMATİK BAŞLIYOR ---

        // YENİ EKLENDİ: Sinematik başladığını Aim sistemine haber ver (Nişangah gizlenir)
        if (NisanKontrol.Instance != null) NisanKontrol.Instance.sinematikOynuyor = true;

        if (anaKarakter != null) anaKarakter.SetActive(false);

        CameraFollow ozelKameram = Camera.main.GetComponent<CameraFollow>();
        if (ozelKameram != null) ozelKameram.enabled = false;


        // --- 2. AŞAMA: BEKLEME ---
        yield return new WaitForSeconds(sinematikSuresi);


        // --- 3. AŞAMA: SİNEMATİK BİTTİ, OYUN BAŞLIYOR ---

        if (sinematikSistemi != null) sinematikSistemi.SetActive(false);

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null) brain.enabled = false;

        if (anaKarakter != null) anaKarakter.SetActive(true);

        if (ozelKameram != null) ozelKameram.enabled = true;

        // YENİ EKLENDİ: Sinematik bittiğini Aim sistemine haber ver (Nişangah geri gelir)
        if (NisanKontrol.Instance != null) NisanKontrol.Instance.sinematikOynuyor = false;

        Debug.Log("Sinematik bitti, kontrol sende!");
    }
}