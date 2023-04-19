using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f; // po�kozen� zp�soben� zbran�
    public float range = 100f; // maxim�ln� vzd�lenost st�ely
    public Camera fpsCam; // odkaz na kameru hr��e
    public ParticleSystem muzzleFlash; // efekt v�st�elu
    public GameObject impactEffect; // efekt z�sahu
    public float fireRate = 15f; // rychlost st�elby
    private float nextTimeToFire = 0f; // �as, kdy bude dal�� st�ela povolena
    public KeyCode interactKey = KeyCode.Mouse0;
    public int maxAmmo = 30; // maxim�ln� kapacita z�sobn�ku
    private int currentAmmo; // aktu�ln� po�et n�boj� v z�sobn�ku
    public float reloadTime = 1f; // doba nutn� k p�ebit�
    private bool isReloading = false; // indikuje, zda prob�h� p�eb�jen�
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
            return; // pokud prob�h� p�eb�jen�, nelze st��let
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return; // pokud je pr�zdn� z�sobn�k, nelze st��let
        }

        if (Input.GetKeyDown(interactKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            muzzleFlash.Play(); // spust� efekt v�st�elu
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
                Debug.Log("Nep��telsk� objekt zni�en!");

            }


            GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // vytvo�� efekt z�sahu
            Destroy(impactGO, 2f); // zni�� efekt z�sahu po 2 sekund�ch
        }
    }

    IEnumerator Reload()
    {
        soundSourceReload.Play();
        isReloading = true;
        Debug.Log("P�eb�jen�");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("P�eb�jen� dokon�eno");
    }
}