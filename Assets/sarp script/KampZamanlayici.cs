using UnityEngine;
using System.Collections;

public class KampZamanlayici : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject isaretFishegiPrefab; 
    public float beklemeSuresi = 3f; 

    private bool tetiklendi = false;

    private void OnTriggerEnter(Collider other)
    {
        // Giren objenin Tag'i "Player" mı diye bakar
        if (other.CompareTag("Player") && !tetiklendi)
        {
            tetiklendi = true;
            Debug.Log("Geri sayim basladi...");
            StartCoroutine(FishekGeriSayim());
        }
    }

    IEnumerator FishekGeriSayim()
    {
        yield return new WaitForSeconds(beklemeSuresi);
        FishekAtesle();
    }

    void FishekAtesle()
    {
        RaycastHit hit;
        // Işını objenin olduğu yerden yukarı fırlatır
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 100f))
        {
            Instantiate(isaretFishegiPrefab, hit.point, Quaternion.identity);
        }
        else
        {
            Vector3 havadaNokta = transform.position + (Vector3.up * 20f);
            Instantiate(isaretFishegiPrefab, havadaNokta, Quaternion.identity);
        }
        Debug.Log("Raycast ile fishek atildi!");
    }
}