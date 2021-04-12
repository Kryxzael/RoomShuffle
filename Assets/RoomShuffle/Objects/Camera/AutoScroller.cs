using UnityEngine;

public class AutoScroller : MonoBehaviour
{
    public float Speed;

    public float MaxLength;

    public Direction4 Direction;

    private Vector2 _originalPosition;

    private bool finished;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
            return;
        
        switch (Direction)
        {
            case Direction4.Down:
                transform.position += (Vector3)Vector2.down * Time.deltaTime * Speed;
                break;
            case Direction4.Up: 
                transform.position += (Vector3)Vector2.up * Time.deltaTime * Speed;
                break;
            case Direction4.Left: 
                transform.position += (Vector3)Vector2.left * Time.deltaTime * Speed;
                break;
            case Direction4.Right: 
                transform.position += (Vector3)Vector2.right * Time.deltaTime * Speed;
                break;
        }

        if (Vector2.Distance(transform.position, _originalPosition) >= MaxLength)
        {
            finished = true;
        }
    }
}
