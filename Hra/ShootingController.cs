using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*  
 Tento k�d neni dod�lan� a v budoucnu bude p�id�n do hry
 */
public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab; // prefab kulek
    public Transform bulletSpawn; // pozice, kde se maj� kule vytv��et
    public float fireRate = 0.5f; // rychlost vyst�elov�n�
    public float bulletSpeed = 10f; // rychlost kulek
    public int maxAmmo = 6; // maxim�ln� po�et kulek
    public float reloadTime = 1f; // doba, po kterou se bude nab�jet zbra�
    public PlayerMovement characterController; // odkaz na CharacterController
    private int currentAmmo; // po�et zb�vaj�c�ch kulek
    private bool isReloading = false; // indik�tor, zda se zbra� nab�j�
    private float lastShotTime; // �as posledn�ho v�st�elu

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) // pokud se zbra� nab�j�, nem��e se st��let
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) // p�i stisknut� R se zbra� nabije
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time - lastShotTime > fireRate && currentAmmo > 0) // stisk lev�ho tla��tka my�i zp�sob� v�st�el
        {
            Shoot();
        }
    }


    void Shoot()
    {
        characterController.enabled = false; // deaktivace pohybu hr��e

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bullet.transform.forward * bulletSpeed;
        currentAmmo--;
        lastShotTime = Time.time;
        StartCoroutine(EnableMovementAfterDelay(0.5f)); // po st�elb� se pohyb op�t aktivuje
    }

    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        characterController.enabled = true; // aktivace pohybu hr��e po zpo�d�n�
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo; // nabit� kule
        isReloading = false;
    }
}