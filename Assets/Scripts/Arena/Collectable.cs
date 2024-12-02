using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Vector2[] positions;

    public float minSpawnRate = 2f;
    public float maxSpawnRate = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Invoke(nameof(RandomisePosition), Random.Range(minSpawnRate, maxSpawnRate));
        }
    }

    public void RandomisePosition()
    {
        transform.position = positions[Random.Range(0, positions.Length)];
        gameObject.SetActive(true);
    }
}
