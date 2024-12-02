using UnityEngine;

public class MousePosition : MonoBehaviour
{
    Vector2 _mousePosition;

    private void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, _mousePosition.y);
    }
}