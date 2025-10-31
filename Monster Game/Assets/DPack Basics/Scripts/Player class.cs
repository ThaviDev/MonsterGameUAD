using UnityEngine;

public class Playerclass : MonoBehaviour
{
    [Header("Ritmo Cardiaco")]
    public float HeartRate = 70f; //Default heart rate
    public float MaxHeartRate = 130f;
    public float MinHeartRate = 50f;
    [Tooltip("Velocidad a la que cambia el ritmo cardiaco")]
    public float HeartRateChangeSpeed = 20f;

    [Header("Detección de Objetos")]
    [Tooltip("Tag del enemigo o los objetos de peligro")]
    public string TargetTag = "Enemy"; // Tag for the enemy or danger objects
    public float DetectionRange = 5f; // Min distance to detect objects

    // --- Dash fields (moved to class scope, can be private) ---
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    [Header("Dash Settings")]
    public float DashCooldown = 1f;
    public float DashDistance = 5f;
    public float DashDuration = 0.2f;
    // ---------------------------------------------------------

    // State for non-coroutine dash
    private Vector3 dashStartPos;
    private Vector3 dashTargetPos;
    private float dashElapsed = 0f;

    void Update()
    {
        ChangeHeartBeat();

        // Update cooldown timer
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Handle dash progression without coroutines
        if (isDashing)
        {
            dashElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(dashElapsed / DashDuration);
            transform.position = Vector3.Lerp(dashStartPos, dashTargetPos, t);

            if (dashElapsed >= DashDuration)
            {
                transform.position = dashTargetPos;
                isDashing = false;
            }
        }
    }

    void ChangeHeartBeat()
    {

        GameObject[] EnemyObject = GameObject.FindGameObjectsWithTag(TargetTag);
        bool isNear = false;

        foreach (GameObject obj in EnemyObject)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < DetectionRange)
            {
                isNear = true;
                break;
            }
        }

        if (isNear)
        {
            // Aumenta el ritmo cardiaco poco a poco
            HeartRate = Mathf.MoveTowards(HeartRate, MaxHeartRate, HeartRateChangeSpeed * Time.deltaTime);
        }
        else
        {
            // Disminuye el ritmo cardiaco poco a poco
            HeartRate = Mathf.MoveTowards(HeartRate, MinHeartRate, HeartRateChangeSpeed * Time.deltaTime);
        }
    }

    void Interact()
    {}

    void Dash()
    {
   
        if (isDashing) return;
        if (dashCooldownTimer > 0f) return;

        // determine dash direction based on input
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 dashDirection;
        if (inputDir.sqrMagnitude > 0.01f)
        {
            // transform from local to world space
            dashDirection = transform.TransformDirection(inputDir.normalized);
            dashDirection.y = 0f;
            dashDirection.Normalize();
        }
        else
        {
            // If theres no input, dash forward as default
            dashDirection = transform.forward;
            dashDirection.y = 0f;
            dashDirection.Normalize();
        }

        isDashing = true;
        dashElapsed = 0f;
        dashStartPos = transform.position;
        dashTargetPos = dashStartPos + dashDirection * DashDistance;
        dashCooldownTimer = DashCooldown;
    }
}
