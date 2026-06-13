using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Menü Bağlantıları")]
    public GameObject pauseMenuPaneli;
    public GameObject ayarlarPaneli; 

    private bool oyunDurduMu = false;

    void Start()
    {
        oyunDurduMu = false;
        Time.timeScale = 1f;

        if (pauseMenuPaneli != null) pauseMenuPaneli.SetActive(false);
        if (ayarlarPaneli != null) ayarlarPaneli.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (oyunDurduMu) OyunaDevamEt();
            else OyunuDurdur();
        }
    }

    public void OyunaDevamEt()
    {
        pauseMenuPaneli.SetActive(false); 
        if (ayarlarPaneli != null) ayarlarPaneli.SetActive(false); 
        
        Time.timeScale = 1f;              
        oyunDurduMu = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OyunuDurdur()
    {
        pauseMenuPaneli.SetActive(true);  
        Time.timeScale = 0f;              
        oyunDurduMu = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void AyarlariAc()
    {
        pauseMenuPaneli.SetActive(false);
        if (ayarlarPaneli != null) ayarlarPaneli.SetActive(true);
    }

    public void AyarlardanGeriDon()
    {
        if (ayarlarPaneli != null) ayarlarPaneli.SetActive(false);
        pauseMenuPaneli.SetActive(true);
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(1); 
    }
}