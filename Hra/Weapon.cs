using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f; // poškození zpùsobené zbraní
    public float range = 100f; // maximální vzdálenost støely
    public Camera fpsCam; // odkaz na kameru hráèe
    public ParticleSystem muzzleFlash; // efekt výstøelu
    public GameObject impactEffect; // efekt zásahu
    public float fireRate = 15f; // rychlost støelby
    private float nextTimeToFire = 0f; // èas, kdy bude další støela povolena
    public KeyCode interactKey = KeyCode.Mouse0;
    public int maxAmmo = 30; // maximální kapacita zásobníku
    private int currentAmmo; // aktuální poèet nábojù v zásobníku
    public float reloadTime = 1f; // doba nutná k pøebití
    private bool isReloading = false; // indikuje, zda probíhá pøebíjení
    public AudioClip soundClipShot;
    public AudioSource soundSourceShot;
    public AudioClip soundClipReload;
    public AudioSource soundSourceReload;
    public AudioClip soundClipHit;
    public AudioSource soundSourceHit;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
        {
            return; // pokud probíhá pøebíjení, nelze støílet
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return; // pokud je prázdný zásobník, nelze støílet
        }

        if (Input.GetKeyDown(interactKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            muzzleFlash.Play(); // spustí efekt výstøelu
            soundSourceShot.Play();
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            EnemyAILevelThree enemy = hitInfo.transform.GetComponent<EnemyAILevelThree>();
            if (enemy != null)
            {
                soundSourceHit.Play();
                Destroy(enemy.gameObject);
                Debug.Log("Nepøátelský objekt znièen!");

            }


            GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // vytvoøí efekt zásahu
            Destroy(impactGO, 2f); // znièí efekt zásahu po 2 sekundách
        }
    }

    IEnumerator Reload()
    {
        soundSourceReload.Play();
        isReloading = true;
        Debug.Log("Pøebíjení");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Pøebíjení dokonèeno");
    }
}