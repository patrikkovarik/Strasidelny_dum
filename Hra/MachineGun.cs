using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tøída reprezentující MachineGun pro støílení a nièení BossAI.
/// </summary>
public class MachineGun : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    public KeyCode interactKey = KeyCode.Mouse0;
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;
    public AudioClip soundClipShot;
    public AudioSource soundSourceShot;
    public AudioClip soundClipReload;
    public AudioSource soundSourceReload;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(interactKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            muzzleFlash.Play();
            soundSourceShot.Play();
            Shoot();
        }
        else if (Input.GetKeyUp(interactKey))
        {
            soundSourceShot.Stop();
        }
    }

    /// <summary>
    /// Metoda pro støílení a ubírání zdraví a poèet nábojù.
    /// </summary>
    void Shoot()
    {
        currentAmmo--;
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            BossAI enemy = hitInfo.transform.GetComponent<BossAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGO, 2f);
        }
    }

    /// <summary>
    /// Metoda pro pøebijení zbranì
    /// </summary>
    /// <returns>vrací èas pøebití</returns>
    IEnumerator Reload()
    {
        soundSourceShot.Stop();
        soundSourceReload.Play();
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded.");
    }
}
