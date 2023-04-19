using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// T��da reprezentuj�c� hr��e.
/// </summary>
public class PlayerMovement : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip soundClipRun;
    public AudioSource soundSourceRun;
    public AudioClip soundClipJump;
    public AudioSource soundSourceJump;
    public AudioClip soundClipMusic;
    public AudioSource soundSourceMusic;


    [Header("Pohyb")]
    public float walkSpeed = 5f; // Rychlost ch�ze
    public float runSpeed = 10f; // Rychlost b�hu
    public float mouseSensitivity = 100f; // Citlivost my�i
    public Camera cam; // Kamera hr��e
    public float jumpForce = 3f; // S�la skoku
    private float xRotation = 0f; // �hel nato�en� kamery


    [Header("Stamina")]
    public int maxStamina = 100; // Maxim�ln� po�et staminy
    public int staminaRegenRate = 5; // Rychlost obnovy staminy
    private bool isRegenerating = false; // Prom�nn� pro kontrolu regenerace staminy
    public int currentStamina;

    [Header("�ivoty")]
    public int maxHealth = 100; // Maxim�ln� po�et �ivot�

    [Header("Debug")]
    public GameObject debugScreen;
    public GameObject pointer;


    [Header("RigidBody")]
    private Rigidbody rb; // Rigidbody hr��e

    [Header("Kontrola")]
    private bool isJumping = false; // Prom�nn� pro kontrolu sk�k�n�
    private bool isRunning = false; // Prom�nn� pro kontrolu b�hu
    public bool isCrouching = false; // Prom�nn� pro kontrolu pl�en�
    public bool isInGame = false;

    [Header("Pl�en�")]
    private float standingCamHeight;
    private float crouchingCamHeight = 3f;


    [Header("Reset")]
    public KeyCode resetKey = KeyCode.P;
    public Vector3 startRotation;


    [Header("Storage")]
    public GameObject Storage;


    [Header("menu")]
    public GameObject menuPanel;

    [Header("UI")]
    public Text livesText;
    public Text staminaText;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Skryt� kurzoru my�i
        soundSourceMusic.Play();
        debugScreen.SetActive(false);
        Time.timeScale = 1f; // obnoven� �asu spust� hru zp�t
        pointer.SetActive(false);
        Storage.SetActive(false);
        standingCamHeight = cam.transform.localPosition.y;
        startRotation = transform.eulerAngles;
        menuPanel.SetActive(false);
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazen� typu hry v textov�m prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Na�ten� pozice");
            LoadGame();
        }
        else
        {
            Debug.Log("Nov� hra");
        }


    }

    void Update()
    {

        livesText.text = "Lives: " + maxHealth;
        staminaText.text = "Stamina: " + currentStamina;

        // P�epnut� mezi ch�z� a b�hem
        if (Input.GetKeyDown(KeyCode.LeftShift) && isJumping == false && currentStamina > 0)
        {
            isRunning = true;
            soundSourceRun.Play();
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            soundSourceRun.Stop();
        }


        if (Input.GetKeyDown(resetKey))
        {
            ResetPosition();
        }



        // �nava b�hem b�hu
        if (isRunning && currentStamina > 0)
        {
            currentStamina -= 1;
            if (currentStamina == 0)
            {
                isRunning = false;
                soundSourceRun.Stop();
            }
        }



        if (transform.position.y < -15)
        {
            transform.position = new Vector3(0, 2, 0); // Replace (0, 2, 0) with the starting position of your map
        }


        // Z�sk�n� vstupu hr��e
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement;


        if (isCrouching)
        {
            movement = transform.forward * moveVertical * 0.5f; // Polovi�n� rychlost pohybu p�i pl�en�
        }
        else
        {
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        }

        // V�po�et rychlosti pohybu hr��e
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        // Nato�en� kamery
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (Physics.Raycast(transform.position, -Vector3.up, 1.1f))
        {
            isJumping = false;
        }

        //skok
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            soundSourceJump.Play();
            Jump();
        }
    

        if (Input.GetKeyDown(KeyCode.F3))
        {
            debugScreen.SetActive(!debugScreen.activeSelf);
            pointer.SetActive(!pointer.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Storage.SetActive(!Storage.activeSelf);

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }



        // Regenerace staminy mimo b�h a �navy b�hem b�hu
        if (!isRunning && !isRegenerating && currentStamina < maxStamina)
        {
            isRegenerating = true;
            StartCoroutine(RegenerateStamina());
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isJumping && currentStamina > 0 && !isRunning)
        {
            isCrouching = true;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, crouchingCamHeight, cam.transform.localPosition.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, standingCamHeight, cam.transform.localPosition.z);
        }

        // N�klon kamery doprava na kl�vese E
        if (Input.GetKey(KeyCode.E))
        {
            cam.transform.Rotate(Vector3.up, 3f);

        }

        // N�klon kamery doleva na kl�vese Q
        if (Input.GetKey(KeyCode.Q))
        {
            cam.transform.Rotate(Vector3.up, -1f);
        }

    }

    /// <summary>
    /// Metoda pro Regeneraci staminy
    /// </summary>
    /// <returns>regenerace</returns>
    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(1f);
        currentStamina += staminaRegenRate;
        if (currentStamina < maxStamina && !isRunning)
        {
            StartCoroutine(RegenerateStamina());
        }
        else
        {
            isRegenerating = false;
        }
    }

    /// <summary>
    /// Metoda pro sk�k�n�
    /// </summary>
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    /// <summary>
    /// Metoda pro resetov�n� pozice hr��e.
    /// </summary>
private void ResetPosition()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = newPosition;
        transform.eulerAngles = startRotation;
        isJumping = false;
    }



    /// <summary>
    /// Metoda pro ub�r�n� �ivot�.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Debug.Log("Hr�� zem�el");
        }
    }

    public void RestoreHealth(int health)
    {
        maxHealth += health;
        if (maxHealth > 100)
        {
            maxHealth = 100;
        }
    }


    /// <summary>
    /// Metoda pro restore staminy.
    /// </summary>
    /// <param name="stamina"></param>
    public void RestoreStamina(int stamina)
    {
        currentStamina += stamina;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }


    /// <summary>
    /// metoda pro kontrolu zda hr�� narazil do st�ny.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // Pokud hr�� narazil na st�nu
        if (collision.gameObject.tag == "Wall")
        {
            // Zastav�me pohyb hr��e
            rb.velocity = Vector3.zero;

            // Vyp�eme zpr�vu do konzole pro ��ely debugov�n�
            Debug.Log("Hr�� narazil do zdi.");
        }else if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            Debug.Log("Nesk��e");
        }
    }


    /// <summary>
    /// Metoda pro zastaven� hry.
    /// </summary>
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // pozastaven� �asu zastav� hru
    }

    /// <summary>
    /// Metoda pro znovu zapnut� hry
    /// </summary>
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // obnoven� �asu spust� hru zp�t
    }

    /// <summary>
    /// Metoda pro achievementy.
    /// </summary>
    public void AchievementMenu()
    {
        isInGame = true;
        SceneManager.LoadScene("Achievement"); // na�te sc�nu s n�zvem "Options"
    }

    /// <summary>
    /// Metoda pro ukl�d�n� pozice hr��e.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", transform.position.z);

        // Ulo�en� rotace hr��e
        PlayerPrefs.SetFloat("PlayerRotationX", cam.transform.rotation.x);
        PlayerPrefs.SetFloat("PlayerRotationY", cam.transform.rotation.y);
        PlayerPrefs.SetFloat("PlayerRotationZ", cam.transform.rotation.z);

        // Ulo�en� aktu�ln�ho po�tu �ivot� a staminy
        PlayerPrefs.SetInt("CurrentHealth", maxHealth);
        PlayerPrefs.SetInt("CurrentStamina", currentStamina);

        PlayerPrefs.Save();
        Debug.Log("Hra byla ulo�ena");
    }

    /// <summary>
    /// Metoda pro na�ten� pozice hr��e.
    /// </summary>
    public void LoadGame()
    {
        // Na�ten� pozice hr��e
        float x = PlayerPrefs.GetFloat("PlayerPositionX");
        float y = PlayerPrefs.GetFloat("PlayerPositionY");
        float z = PlayerPrefs.GetFloat("PlayerPositionZ");
        transform.position = new Vector3(x, y, z);

        // Na�ten� rotace hr��e
        float rx = PlayerPrefs.GetFloat("PlayerRotationX");
        float ry = PlayerPrefs.GetFloat("PlayerRotationY");
        float rz = PlayerPrefs.GetFloat("PlayerRotationZ");
        transform.rotation = Quaternion.Euler(rx, ry, rz);

        // Na�ten� aktu�ln�ho po�tu �ivot� a staminy
        maxHealth = PlayerPrefs.GetInt("CurrentHealth");
        currentStamina = PlayerPrefs.GetInt("CurrentStamina");
    }


    /// <summary>
    /// Metoda pro opust�n� sc�ny.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Quitting game..."); // vyp�e zpr�vu v konzoli
        SceneManager.LoadScene("MainMenu"); // na�te sc�nu s n�zvem "Options"
    }

    /// <summary>
    /// Kontrola zda je n�jak� objekt nad hr��em.
    /// </summary>
    void CheckAbovePlayer()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 2.0f))
        {
            Debug.Log("N�co je nad hr��em");

        }


    }


}