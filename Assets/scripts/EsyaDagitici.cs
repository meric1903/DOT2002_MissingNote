using UnityEngine;

public class EsyaDagitici : MonoBehaviour
{
    [Header("Ne Dağıtılacak?")]
    public GameObject esyaPrefab; // Can kiti veya mermi kutusu prefabını buraya at

    [Header("Kaç Tane Dağıtılacak?")]
    public int miktar = 50;

    [Header("Dağıtım Alanı Genişliği (X ve Z)")]
    public float alanGenisligi = 500f; // Haritanın boyutuna göre burayı büyütebilirsin

    // DİKKAT: Bu kod oyunu başlatmadan çalışır!
    [ContextMenu("Eşyaları Haritaya Saç!")]
    public void EsyalariDagit()
    {
        for (int i = 0; i < miktar; i++)
        {
            // Rastgele bir X ve Z noktası bul
            float rastgeleX = Random.Range(-alanGenisligi / 2, alanGenisligi / 2);
            float rastgeleZ = Random.Range(-alanGenisligi / 2, alanGenisligi / 2);

            // Objenin gökyüzündeki başlangıç noktasını belirle
            Vector3 gokyuzuNoktasi = new Vector3(transform.position.x + rastgeleX, transform.position.y + 500f, transform.position.z + rastgeleZ);

            // Gökyüzünden aşağıya doğru bir ışın atıp yeri (zemini) bul
            if (Physics.Raycast(gokyuzuNoktasi, Vector3.down, out RaycastHit hit, 1000f))
            {
                // Yere çarptığı noktaya objeyi yarat
                GameObject yeniEsya = Instantiate(esyaPrefab, hit.point, Quaternion.identity);
                
                // Hiyerarşi kirlenmesin diye bu dağıtıcı objenin içinde grupla
                yeniEsya.transform.parent = this.transform;
            }
        }
        Debug.Log("<color=cyan>" + miktar + " adet eşya haritaya başarıyla saçıldı!</color>");
    }
}