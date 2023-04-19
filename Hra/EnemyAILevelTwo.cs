using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAILevelTwo : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip soundClip;
    public AudioSource soundSource;

    [Header("Pozice")]
    public Transform player;
    public Transform flashlight; // reference to the player's flashlight

    [Header("Enemy parametry")]
    public float moveSpeed = 5;
    public float maxDist = 10;
    public float playerHealth;
    public float minDist = 5;
    public float attackCooldown = 1f; // mezi�as mezi �toky
    public float detectionRadius = 10f; // radius detekce hr��e
    public float sneakModifier = 2f; // modifik�tor detekce pro hr��e, kte�� se pl��
    public float attackDamage = 1f; // po�kozen� �toku
    public float flashlightDetectionRadius = 5f; // radius detekce baterky
    public float flashlightDetectionAngle = 30f; // �hel detekce baterky
    public string names = "";

    [Header("Detekce")]
    public LayerMask detectionMask; // vrstva objekt�, se kter�mi se m� detekce prov�d�t
    private bool isPlayerDead = false;
    private float lastAttackTime; // �as posledn�ho �toku
    private PlayerMovement player_a;

    void Start()
    {
        lastAttackTime = -attackCooldown; // nastaven� �asu posledn�ho �toku na �as men�� ne� 0, aby byl umo�n�n prvn� �tok
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadPosition();
        }
        else
        {
            Debug.Log("Nov� hra");
        }
    }

    void Update()
    {
        player_a = FindObjectOfType<PlayerMovement>();
        playerHealth = player_a.maxHealth;

        if (!isPlayerDead && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            transform.LookAt(player);
            float currentDetectionRadius = detectionRadius;
            if (player_a.isCrouching) // pokud se hr�� pl��, zmen�� se radius detekce
            {
                currentDetectionRadius /= sneakModifier;
            }
            if (Vector3.Distance(transform.position, player.position) <= minDist && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
            }
            else if (CanDetectFlashlight())
            {
                MoveTowardsFlashlight();
            }
            else
            {
                MoveTowardsPlayer(currentDetectionRadius);
            }
        }
    }

    private bool CanDetectFlashlight()
    {
        // Check if the flashlight is within detection radius and angle
        Vector3 directionToFlashlight = flashlight.position - transform.position;
        float distanceToFlashlight = directionToFlashlight.magnitude;
        float angleToFlashlight = Vector3.Angle(transform.forward, directionToFlashlight);
        Debug.Log("sv�t� na enemy");
        return distanceToFlashlight <= flashlightDetectionRadius && angleToFlashlight <= flashlightDetectionAngle;
    }

    private void MoveTowardsFlashlight()
    {
        transform.LookAt(flashlight);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    private void MoveTowardsPlayer(float currentDetectionRadius)
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // raycast pro detekci objekt� ve sc�n�
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, currentDetectionRadius, detectionMask))
        {
            // pokud se detekuje kolize s objektem, enemy se posune do strany
            Vector3 dir = hit.normal;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        soundSource.Play();

        player_a.maxHealth--;
        Debug.Log("�ivoty:" + playerHealth);

        lastAttackTime = Time.time;

        if (playerHealth <= 0)
        {
            isPlayerDead = true;
            DetectPlayerDeath();
            SceneManager.LoadScene("Death");
        }
    }

    public void DetectPlayerDeath()
    {
        if (isPlayerDead)
        {
            // n�jak� akce, kter� se m� prov�st p�i smrti hr��e
            Debug.Log("Hr�� zem�el.");
        }
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(names + "Enemy2PositionX", transform.position.x);
        PlayerPrefs.SetFloat(names + "Enemy2PositionY", transform.position.y);
        PlayerPrefs.SetFloat(names + "Enemy2PositionZ", transform.position.z);
        PlayerPrefs.Save();
    }

    public void LoadPosition()
    {
        Vector3 enemy2Position = new Vector3(
            PlayerPrefs.GetFloat(names + "Enemy2PositionX"),
            PlayerPrefs.GetFloat(names + "Enemy2PositionY"),
            PlayerPrefs.GetFloat(names + "Enemy2PositionZ")
        );
        transform.position = enemy2Position;
    }
}