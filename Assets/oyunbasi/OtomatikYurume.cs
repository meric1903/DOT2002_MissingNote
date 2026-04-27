using UnityEngine;
using System.Collections;

public class OtomatikYurume : MonoBehaviour
{
    public float yurumeHizi = 2f; // Karakterin hýzý
    public float yurumeSuresi = 6f; // Kaç saniye boyunca yürüyecek?

    private bool yuruyorMu = true;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(YurumeSenaryosu());
    }

    void Update()
    {
        // Karakter yuruyorMu "true" olduðu sürece dümdüz ileri gider
        if (yuruyorMu)
        {
            transform.Translate(Vector3.forward * yurumeHizi * Time.deltaTime);
        }
    }

    IEnumerator YurumeSenaryosu()
    {
        // Belirlediðin süre (örneðin 6 saniye) kadar bekle
        yield return new WaitForSeconds(yurumeSuresi);

        // Süre dolunca yürümeyi kes ve animasyonu durdur
        yuruyorMu = false;
        if (anim != null) anim.speed = 0;

        Debug.Log("Karakter hedefe vardý ve durdu.");
    }
}