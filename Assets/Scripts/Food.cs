using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public enum FoodCategory{
    Lemon, Mango, Egg, Banana, Milk, Cola, Pepsi, Soda, Salad, Cheese, Sandwich
}

public class Food : MonoBehaviour{
    [Header("食物种类")] public FoodCategory category;
    [Space]
    public SpriteRenderer icon; 
    
    
    private bool isWandering = false;
    private bool controlling = false;
    public float moveUpDistance = 5f;
    public float moveUpDuration = 1.5f;
    public float moveSpeed = 5f;
    public float zMoveSpeed = 5f;
    public float destroyDistance = 5f;

    public float maxWanderSpeed = 2f;
    public float transitionDuration = 0.5f;
    public float directionChangeInterval = 2f;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;

    public float knockbackDistance = 0.5f;
    public float knockbackDuration = 0.5f;
    public int flashCount = 3;
    public float flashInterval = 0.5f;

    public static event Action<Food> OnCollisionEvent;
    private QTEController qte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        icon = GetComponentInChildren<SpriteRenderer>();
        icon.sprite = ResourceManager.Load<Sprite>("Resources/Sprites/FoodIcons" + category.ToString());
        qte = FindFirstObjectByType<QTEController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlling && !qte.getQTEStarted())
        {
            Vector3 move = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                move += Vector3.up;
            if (Input.GetKey(KeyCode.S))
                move += Vector3.down;
            if (Input.GetKey(KeyCode.A))
                move += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                move += Vector3.right;

            move = move.normalized * (moveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
                move += toCamera * (zMoveSpeed * Time.deltaTime);
            }

            transform.position += move;

            // Check distance to camera
            float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
            if (distanceToCamera < destroyDistance)
            {
                OnReachCamera();
            }
        }
        if (isWandering && !qte.getQTEStarted())
        {
            currentVelocity = Vector3.Lerp(currentVelocity, targetDirection * maxWanderSpeed, Time.deltaTime / transitionDuration);
            transform.position += currentVelocity * Time.deltaTime;
        }
    }

    public void OnClick()
    {
        StartCoroutine(MoveUpSmoothly());
    }

    void OnReachCamera()
    {
        Debug.Log("Object reached the camera and is being destroyed.");
        controlling = false;
        Camera.main.GetComponent<Click>().StopControlling();
        
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClickableSprite") || other.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with " + other.tag);
            OnReachObstacle();
            OnCollisionEvent?.Invoke(this);
        }
        if (other.CompareTag("Layer"))
        {
            GetComponent<SpriteRenderer>().sortingOrder = other.GetComponent<Layer>().orderInLayer + 1;
        }
    }

    void OnReachObstacle()
    {
        // You can add more effects here if you want
        controlling = false;
        StartCoroutine(SmoothKnockback());
    }

    IEnumerator MoveUpSmoothly()
    {
        Vector3 targetPos = transform.position + Vector3.up * moveUpDistance;
        Vector3 velocity = Vector3.zero;

        float distanceThreshold = 0.01f;
        while (Vector3.Distance(transform.position, targetPos) > distanceThreshold)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, moveUpDuration);
            yield return null;
        }

        transform.position = targetPos;

        controlling = true;
        isWandering = true;
        
        StartCoroutine(ChangeDirectionRoutine());
    }

    IEnumerator ChangeDirectionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (isWandering)
        {
            // Pick a new random direction on XY plane
            float angle = UnityEngine.Random.Range(0f, 360f);
            targetDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    IEnumerator SmoothKnockback()
    {
        StartCoroutine(FlashRoutine());
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * knockbackDistance;

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / knockbackDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }

    IEnumerator FlashRoutine()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) yield break;

        for (int i = 0; i < flashCount; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(flashInterval / 2f);

            rend.enabled = true;
            yield return new WaitForSeconds(flashInterval / 2f);
        }
        controlling = true;
    }
}
