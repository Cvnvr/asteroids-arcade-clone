using UnityEngine;

public class ShipMovementController : MonoBehaviour, IMoveable
{
    #region Variables
    private ScreenBoundsHandler screenBoundsHandler;

    private Rigidbody2D rigidbody;

    [Header("Movement Parameters")]
    [SerializeField] private float forwardThrust;

    [SerializeField] private float rotationalThrust;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotationalSpeed;

    private float forwardInput;
    private float rotationalInput;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        ValidateRigidbody();

        #region Local Functions
        void ValidateRigidbody()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent(typeof(Rigidbody2D));
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }

            rigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion Local Functions
    }

    private void Start()
    {
        screenBoundsHandler = ScreenBoundsHandler.Instance;
    }
    #endregion Initialisation

    private void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        rotationalInput = Input.GetAxis("Horizontal");

        KeepObjectWithinScreenBounds();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
    }

    private void KeepObjectWithinScreenBounds()
    {
        if (screenBoundsHandler == null)
        {
            Debug.LogWarning($"[WARNING] The object {this.gameObject.name} cannot access the Screen Bounds Handler.");
            return;
        }

        Vector2 newPosition = transform.position;

        // beyond LEFT of screen
        if (transform.position.x > screenBoundsHandler.LeftSide)
        {
            newPosition.x = screenBoundsHandler.RightSide;
        }
        // beyond RIGHT of screen
        if (transform.position.x < screenBoundsHandler.RightSide)
        {
            newPosition.x = screenBoundsHandler.LeftSide;
        }
        // beyond TOP of screen
        if (transform.position.y > screenBoundsHandler.TopSide)
        {
            newPosition.y = screenBoundsHandler.BottomSide;
        }
        // beyond BOTTOM of screen
        if (transform.position.y < screenBoundsHandler.BottomSide)
        {
            newPosition.y = screenBoundsHandler.TopSide;
        }

        // Set new inverted position
        transform.position = newPosition;
    }
}