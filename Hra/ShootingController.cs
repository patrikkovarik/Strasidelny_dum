using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*  
 Tento kód neni dodìlaný a v budoucnu bude pøidán do hry
 */
public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab; // prefab kulek
    public Transform bulletSpawn; // pozice, kde se mají kule vytváøet
    public float fireRate = 0.5f; // rychlost vystøelování
    public float bulletSpeed = 10f; // rychlost kulek
    public int maxAmmo = 6; // maximální poèet kulek
    public float reloadTime = 1f; // doba, po kterou se bude nabíjet zbraò
    public PlayerMovement characterController; // odkaz na CharacterController
    private int currentAmmo; // poèet zbývajících kulek
    private bool isReloading = false; // indikátor, zda se zbraò nabíjí
    private float lastShotTime; // èas posledního výstøelu

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) // pokud se zbraò nabíjí, nemùže se støílet
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) // pøi stisknutí R se zbraò nabije
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time - lastShotTime > fireRate && currentAmmo > 0) // stisk levého tlaèítka myši zpùsobí výstøel
        {
            Shoot();
        }
    }


    void Shoot()
    {
        characterController.enabled = false; // deaktivace pohybu hráèe

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bullet.transform.forward * bulletSpeed;
        currentAmmo--;
        lastShotTime = Time.time;
        StartCoroutine(EnableMovementAfterDelay(0.5f)); // po støelbì se pohyb opìt aktivuje
    }

    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        characterController.enabled = true; // aktivace pohybu hráèe po zpoždìní
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo; // nabité kule
        isReloading = false;
    }
}