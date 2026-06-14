using UnityEngine;
using UnityEngine.SceneManagement; 

public class CanSistemi : MonoBehaviour
{
    public static CanSistemi Instance;

    [Header("Can Ayarları")]
    public float maksimumCan = 100f;
    private float mevcutCan;

    [Header("Ölüm Ekranı")]
    public GameObject oyunBittiPaneli; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mevcutCan = maksimumCan;
        
        if (oyunBittiPaneli != null) oyunBittiPaneli.SetActive(false);
    }

    public void HasarAl(float hasarMiktari)
    {
        if (mevcutCan <= 0) return; 

        mevcutCan -= hasarMiktari;
        Debug.Log("Karakter Hasar Aldı! Kalan Can: " + mevcutCan);

        if (mevcutCan <= 0)
        {
            mevcutCan = 0;
            KarakterOldu();
        }
    }

    public void CanEkle(float iyilestirmeMiktari)
    {
        if (mevcutCan <= 0) return; 
        mevcutCan += iyilestirmeMiktari;
        if (mevcutCan > maksimumCan) mevcutCan = maksimumCan;
    }

    private void KarakterOldu()
    {
        Time.timeScale = 0f;

        if (oyunBittiPaneli != null)
        {
            oyunBittiPaneli.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OyunuYenidenBaslat()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public float GetMevcutCan() { return mevcutCan; }
    public float GetMaksimumCan() { return maksimumCan; }
    public bool CanIhtiyaciVarMi() { return mevcutCan < maksimumCan; }
}