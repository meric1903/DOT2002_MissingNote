using UnityEngine;

public class FlareZone : MonoBehaviour {
    public GameObject fisekPrefab; // 1. Adımda yaptığın Prefab
    public Transform cikisNoktasi; // 2. Adımda yaptığın Boş Obje
    private bool ateslendiMi = false;

    private void OnTriggerEnter(Collider other) {
        // Eğer giren objenin Tag'i "Player" ise (Karakterine Player tag'ini vermeyi unutma!)
        if (other.CompareTag("Player") && !ateslendiMi) {
            GokyuzuKontrolVeAtesle();
        }
    }

    void GokyuzuKontrolVeAtesle() {
        RaycastHit hit;
        // Çıkış noktasından yukarı doğru 50 metre ışın gönder
        if (Physics.Raycast(cikisNoktasi.position, Vector3.up, out hit, 50f)) {
            Debug.Log("Üstte engel var: " + hit.collider.name + " - Ateşlenmedi!");
        } else {
            Instantiate(fisekPrefab, cikisNoktasi.position, Quaternion.identity);
            ateslendiMi = true; 
            Debug.Log("Gökyüzü açık, fişek fırlatıldı!");
        }
    }
}