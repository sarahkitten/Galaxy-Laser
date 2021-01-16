using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // config params
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0,1)] float shootSoundVolume = 0.25f;
    int maxHealth;


    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float quickProjectileFiringPeriod = 0.05f;
    [SerializeField] float projectileFiringPeriod = 0.4f;
    int maxBurstShots;
    [SerializeField] int burstShotsRemaining = 10;
    [SerializeField] bool burstShotOn = false;
    [SerializeField] int burstRegenPerShot = 1;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float dfltFiringPeriod;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        AutoFire();
        maxHealth = health;
        dfltFiringPeriod = projectileFiringPeriod;
        maxBurstShots = burstShotsRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        QuickFire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collision registered with");
        Debug.Log(collision.tag);
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
        if (health > maxHealth)
        {
            Debug.Log("Health Overflow");
            health = maxHealth;
        }
    }

    private void Die() {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth() {
        return health;
    }
    public int GetBurstShotsRemaining() {
        Debug.Log(burstShotsRemaining);
        return burstShotsRemaining;
    }

    private void QuickFire()
    {
        if (Input.GetButtonDown("Fire1")) {
            if (burstShotsRemaining > 0)
            {
                projectileFiringPeriod = quickProjectileFiringPeriod;
                burstShotOn = true;
            }
        }
        if (Input.GetButtonUp("Fire1")) {
            projectileFiringPeriod = dfltFiringPeriod;
            burstShotOn = false;
        }
    }

    private void AutoFire() {
        firingCoroutine = StartCoroutine(FireContinuously());
    }

    IEnumerator FireContinuously() {
        while (true) {
            // validate burst shot
            if (burstShotOn) 
            { 
                burstShotsRemaining -= 1; 
                if (burstShotsRemaining <= 0)
                {
                    projectileFiringPeriod = dfltFiringPeriod;
                    burstShotOn = false;
                }
            } else if (burstShotsRemaining < maxBurstShots)
            {
                burstShotsRemaining += burstRegenPerShot;
            }
            // shoot laser
            GameObject laser = Instantiate(
                laserPrefab, 
                transform.position, 
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move() {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;     // Camera-independent edges of screen
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;     
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
