using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tøída reprezentující hráèe.
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
    public float walkSpeed = 5f; // Rychlost chùze
    public float runSpeed = 10f; // Rychlost bìhu
    public float mouseSensitivity = 100f; // Citlivost myši
    public Camera cam; // Kamera hráèe
    public float jumpForce = 3f; // Síla skoku
    private float xRotation = 0f; // Úhel natoèení kamery


    [Header("Stamina")]
    public int maxStamina = 100; // Maximální poèet staminy
    public int staminaRegenRate = 5; // Rychlost obnovy staminy
    private bool isRegenerating = false; // Promìnná pro kontrolu regenerace staminy
    public int currentStamina;

    [Header("Životy")]
    public int maxHealth = 100; // Maximální poèet životù

    [Header("Debug")]
    public GameObject debugScreen;
    public GameObject pointer;


    [Header("RigidBody")]
    private Rigidbody rb; // Rigidbody hráèe

    [Header("Kontrola")]
    private bool isJumping = false; // Promìnná pro kontrolu skákání
    private bool isRunning = false; // Promìnná pro kontrolu bìhu
    public bool isCrouching = false; // Promìnná pro kontrolu plížení
    public bool isInGame = false;

    [Header("Plížení")]
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
        Cursor.lockState = CursorLockMode.Locked; // Skrytí kurzoru myši
        soundSourceMusic.Play();
        debugScreen.SetActive(false);
        Time.timeScale = 1f; // obnovení èasu spustí hru zpìt
        pointer.SetActive(false);
        Storage.SetActive(false);
        standingCamHeight = cam.transform.localPosition.y;
        startRotation = transform.eulerAngles;
        menuPanel.SetActive(false);
        string gameType = PlayerPrefs.GetString("gameType");
        Debug.Log("Typ hry: " + gameType);  // zobrazení typu hry v textovém prvku
        if (gameType == "OldGame")
        {
            Debug.Log("Naètení pozice");
            LoadGame();
        }
        else
        {
            Debug.Log("Nová hra");
        }


    }

    void Update()
    {

        livesText.text = "Lives: " + maxHealth;
        staminaText.text = "Stamina: " + currentStamina;

        // Pøepnutí mezi chùzí a bìhem
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



        // Únava bìhem bìhu
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


        // Získání vstupu hráèe
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement;


        if (isCrouching)
        {
            movement = transform.forward * moveVertical * 0.5f; // Polovièní rychlost pohybu pøi plížení
        }
        else
        {
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        }

        // Výpoèet rychlosti pohybu hráèe
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        // Natoèení kamery
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



        // Regenerace staminy mimo bìh a únavy bìhem bìhu
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

        // Náklon kamery doprava na klávese E
        if (Input.GetKey(KeyCode.E))
        {
            cam.transform.Rotate(Vector3.up, 3f);

        }

        // Náklon kamery doleva na klávese Q
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
    /// Metoda pro skákání
    /// </summary>
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    /// <summary>
    /// Metoda pro resetování pozice hráèe.
    /// </summary>
private void ResetPosition()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = newPosition;
        transform.eulerAngles = startRotation;
        isJumping = false;
    }



    /// <summary>
    /// Metoda pro ubírání životù.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Debug.Log("Hráè zemøel");
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
    /// metoda pro kontrolu zda hráè narazil do stìny.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // Pokud hráè narazil na stìnu
        if (collision.gameObject.tag == "Wall")
        {
            // Zastavíme pohyb hráèe
            rb.velocity = Vector3.zero;

            // Vypíšeme zprávu do konzole pro úèely debugování
            Debug.Log("Hráè narazil do zdi.");
        }else if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            Debug.Log("Neskáèe");
        }
    }


    /// <summary>
    /// Metoda pro zastavení hry.
    /// </summary>
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // pozastavení èasu zastaví hru
    }

    /// <summary>
    /// Metoda pro znovu zapnutí hry
    /// </summary>
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // obnovení èasu spustí hru zpìt
    }

    /// <summary>
    /// Metoda pro achievementy.
    /// </summary>
    public void AchievementMenu()
    {
        isInGame = true;
        SceneManager.LoadScene("Achievement"); // naète scénu s názvem "Options"
    }

    /// <summary>
    /// Metoda pro ukládání pozice hráèe.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", transform.position.z);

        // Uložení rotace hráèe
        PlayerPrefs.SetFloat("PlayerRotationX", cam.transform.rotation.x);
        PlayerPrefs.SetFloat("PlayerRotationY", cam.transform.rotation.y);
        PlayerPrefs.SetFloat("PlayerRotationZ", cam.transform.rotation.z);

        // Uložení aktuálního poètu životù a staminy
        PlayerPrefs.SetInt("CurrentHealth", maxHealth);
        PlayerPrefs.SetInt("CurrentStamina", currentStamina);

        PlayerPrefs.Save();
        Debug.Log("Hra byla uložena");
    }

    /// <summary>
    /// Metoda pro naètení pozice hráèe.
    /// </summary>
    public void LoadGame()
    {
        // Naètení pozice hráèe
        float x = PlayerPrefs.GetFloat("PlayerPositionX");
        float y = PlayerPrefs.GetFloat("PlayerPositionY");
        float z = PlayerPrefs.GetFloat("PlayerPositionZ");
        transform.position = new Vector3(x, y, z);

        // Naètení rotace hráèe
        float rx = PlayerPrefs.GetFloat("PlayerRotationX");
        float ry = PlayerPrefs.GetFloat("PlayerRotationY");
        float rz = PlayerPrefs.GetFloat("PlayerRotationZ");
        transform.rotation = Quaternion.Euler(rx, ry, rz);

        // Naètení aktuálního poètu životù a staminy
        maxHealth = PlayerPrefs.GetInt("CurrentHealth");
        currentStamina = PlayerPrefs.GetInt("CurrentStamina");
    }


    /// <summary>
    /// Metoda pro opustìní scény.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Quitting game..."); // vypíše zprávu v konzoli
        SceneManager.LoadScene("MainMenu"); // naète scénu s názvem "Options"
    }

    /// <summary>
    /// Kontrola zda je nìjaký objekt nad hráèem.
    /// </summary>
    void CheckAbovePlayer()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 2.0f))
        {
            Debug.Log("Nìco je nad hráèem");

        }


    }


}