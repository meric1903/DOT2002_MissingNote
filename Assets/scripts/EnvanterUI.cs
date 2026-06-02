using UnityEngine;
using UnityEngine.InputSystem;

public class EnvanterUI : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public GameObject cantaPaneli;
    public Transform slotGrupObjesi;

    private EnvanterSlot[] slotlar;
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
            cantaAcikMi = !cantaAcikMi;
            cantaPaneli.SetActive(cantaAcikMi);

            if (cantaAcikMi)
            {
                Time.timeScale = 0f; 
                Cursor.visible = true; 
                Cursor.lockState = CursorLockMode.None; 
            }
            else
            {
                Time.timeScale = 1f; 
                Cursor.visible = false; 
                Cursor.lockState = CursorLockMode.Locked; 
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
                // SlotuDoldur fonksiyonuna artık adetli paketi paslıyoruz
                slotlar[i].SlotuDoldur(EnvanterSistemi.Instance.cantadakiEsyalar[i]);
            }
        }
    }
}