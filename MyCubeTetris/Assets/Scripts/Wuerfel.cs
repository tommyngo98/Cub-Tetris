using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wuerfel : MonoBehaviour
{
    public GameObject wuerfelPrefab;
    List<GameObject> wuerfelListe;
    Rigidbody rb;
    bool ende = false;

    int punkte = 0;
    public Text punkteAnzeige;

    public AudioClip vorbei;
    public AudioClip entfernen;
    public AudioClip landen;
    public AudioClip verschieben;


    // Start is called before the first frame update
    void Start()
    {
        wuerfelListe = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ende)
        {
            return;
        }

        float xNeu = transform.position.x;
        float zNeu = transform.position.z;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xNeu++;
            if (xNeu > 2)
            {
                xNeu = 2;
            }
            AudioSource.PlayClipAtPoint(verschieben, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xNeu--;
            if (xNeu < -2)
            {
                xNeu = -2;
            }
            AudioSource.PlayClipAtPoint(verschieben, transform.position);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            zNeu++;
            if (zNeu > 2)
            {
                zNeu = 2;
            }
            AudioSource.PlayClipAtPoint(verschieben, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zNeu--;
            if (zNeu < -2)
            {
                zNeu = -2;
            }
            AudioSource.PlayClipAtPoint(verschieben, transform.position);
        }

        transform.position = new Vector3(xNeu, transform.position.y, zNeu);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 positionAlt = transform.position;
        AudioSource.PlayClipAtPoint(landen, transform.position);

        if (positionAlt.y < 4)
        {
            transform.position = new Vector3(0, 6, 0);
            rb.drag = rb.drag * 0.98f;
            Object objektVerweis = Instantiate(wuerfelPrefab, positionAlt, Quaternion.identity);
            GameObject spielObjektVerweis = (GameObject)objektVerweis;
            wuerfelListe.Add(spielObjektVerweis);

            Pruefen();
        }
        else
        {
            ende = true;
            AudioSource.PlayClipAtPoint(vorbei, transform.position);
        }
    }

    void Pruefen()
    {
        int zaehler = 0;
        for (int k = 0; k < wuerfelListe.Count; k++)
        {
            if (wuerfelListe[k].transform.position.y >= -2.75 && wuerfelListe[k].transform.position.y <= -2.35f)
            {
                zaehler++;
            }
        }
        if (zaehler == 25)
        {
            punkte++;
            punkteAnzeige.text = "Punkte: " + punkte;

            for (int k = wuerfelListe.Count - 1; k >= 0; k--)
            {
                if (wuerfelListe[k].transform.position.y >= -2.75f && wuerfelListe[k].transform.position.y <= -2.35f)
                {
                    Destroy(wuerfelListe[k]);
                    wuerfelListe.RemoveAt(k);
                    AudioSource.PlayClipAtPoint(entfernen, transform.position);

                }
            }
        }
    }
}
