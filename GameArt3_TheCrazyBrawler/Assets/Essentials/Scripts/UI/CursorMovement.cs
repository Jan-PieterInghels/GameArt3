using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    [Range(1,2)] [SerializeField] private int _playerNumber;
    [SerializeField] private float _cursorSpeed;
    [SerializeField] private LayerMask _checkLayer;
    [SerializeField] private Image _image;

    private string _horizontalAxis, _verticalAxis, _interactButton;
    private float _objectHeight, _objectWidth;

    private Vector3 _direction;
    private Vector2 _screenBounds;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _horizontalAxis = "Player" + _playerNumber + "_HorizontalAxis";
        _verticalAxis = "Player" + _playerNumber + "_VerticalAxis";
        _interactButton = "Player" + _playerNumber + "_AButton";

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _objectHeight = _image.sprite.bounds.size.y;
        _objectWidth = _image.sprite.bounds.size.x;
    }

    private void Update()
    {
        _direction = new Vector3(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis), 0);

        if(Input.GetButtonDown(_interactButton))
            RaycastCheck();
    }

    private void RaycastCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + Vector3.forward, Mathf.Infinity, _checkLayer);

        if (hit.collider != null) 
        {
            ButtonBehaviour button = hit.collider.GetComponent<ButtonBehaviour>();
            button?.TaskOnClick(_playerNumber);
        }
    }

    void FixedUpdate()
    {
        transform.position += _direction * _cursorSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        Clamp();
    }

    private void Clamp()
    {
        Vector3 viewPos = transform.position;
        viewPos = new Vector3
        (
            FloatClamp(viewPos.x, _screenBounds.x + _objectWidth, -(_screenBounds.x + _objectWidth)),
            FloatClamp(viewPos.y, _screenBounds.y + _objectHeight, ((699.5f/100) - _objectHeight)),
            viewPos.z
        );


        transform.position = viewPos;
    }

    private float FloatClamp(float value, float min, float max)
    {
        return (value <= min) ? min : (value >= max) ? max : value;
    }
}
