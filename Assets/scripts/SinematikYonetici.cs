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

    [Header("Not Sistemi")]
    public GameObject notPaneli; 

    void Start()
    {
        if (notPaneli != null) notPaneli.SetActive(false); 
        StartCoroutine(SinematikSenaryosu());
    }

    IEnumerator SinematikSenaryosu()
    {
        if (NisanKontrol.Instance != null) NisanKontrol.Instance.sinematikOynuyor = true;
        if (anaKarakter != null) anaKarakter.SetActive(false);

        CameraFollow ozelKameram = Camera.main.GetComponent<CameraFollow>();
        if (ozelKameram != null) ozelKameram.enabled = false;

        yield return new WaitForSeconds(sinematikSuresi);

        // --- SİNEMATİK BİTTİ, KARAKTERE GEÇİLİYOR ---
        if (sinematikSistemi != null) sinematikSistemi.SetActive(false);

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null) brain.enabled = false;

        if (anaKarakter != null) anaKarakter.SetActive(true);
        if (ozelKameram != null) ozelKameram.enabled = true;

        // KAMERANIN OTURMASI İÇİN ÇOK KISA BEKLE (0.1 Saniye)
        // Oyuncu önce kendi karakterine geçtiğini hisseder, sonra not açılır.
        yield return new WaitForSeconds(1f);

        // --- ŞİMDİ NOTU GÖSTER VE OYUNU DONDUR ---
        if (notPaneli != null)
        {
            notPaneli.SetActive(true); 
            Time.timeScale = 0f;       
            
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else
        {
            OyunaBasla();
        }
    }

    public void NotuKapat()
    {
        if (notPaneli != null) notPaneli.SetActive(false); 
        OyunaBasla(); 
    }

    private void OyunaBasla()
    {
        Time.timeScale = 1f; 
        
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        if (NisanKontrol.Instance != null) NisanKontrol.Instance.sinematikOynuyor = false;

        Debug.Log("Not okundu, hikaye başladı!");
    }
}