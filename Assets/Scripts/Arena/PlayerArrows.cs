using UnityEngine;

public class PlayerArrows : MonoBehaviour
{
    [HideInInspector] public float speedBoost;
    [HideInInspector] public float boostLimitTime;

    float boostTimer;
    bool isBoosting = false;

    float dirX = 0, dirY = 0;
    public float moveSpeed;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isBoosting)
        {
            rb.linearVelocity = new Vector2(dirX * (moveSpeed + speedBoost), dirY * (moveSpeed + speedBoost));

            if (boostTimer < boostLimitTime)
            {
                boostTimer += Time.deltaTime;
            }
            else
            {
                isBoosting = false;
                boostTimer = 0f;
            }
        }
        else rb.linearVelocity = new Vector2(dirX * moveSpeed, dirY * moveSpeed);

        if (Input.GetKey(KeyCode.UpArrow)) dirY = 1;
        if (Input.GetKey(KeyCode.DownArrow)) dirY = -1;

        if (Input.GetKey(KeyCode.RightArrow)) dirX = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) dirX = -1;

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) dirY = 0;
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) dirX = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Collectable"))
        {
            isBoosting = true;
        }
    }
}
