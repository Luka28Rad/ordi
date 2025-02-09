using UnityEngine;

public class SpiderWebMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDuration = 1f;
    public float moveDistance = 2f;
    
    [Header("Web Settings")]
    public Color webColor = Color.white;
    public float webWidth = 0.05f;
    public Vector2 webStartPoint; // Point where web starts (top anchor point)
    
    private float timer;
    private int direction = -1;
    private Vector2 startPosition;
    private LineRenderer webLine;
    private const int WEB_POINTS = 25; // Number of points to make web look smooth
    private float startDelay;
    private bool hasStarted = false;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        webStartPoint = new Vector2(startPosition.x, startPosition.y + moveDistance); // Default top point
        
        // Setup Line Renderer
        webLine = gameObject.AddComponent<LineRenderer>();
        webLine.material = new Material(Shader.Find("Sprites/Default"));
        webLine.startColor = webLine.endColor = webColor;
        webLine.startWidth = webLine.endWidth = webWidth;
        webLine.positionCount = WEB_POINTS;

        // Set random start delay
        startDelay = Random.Range(0f, 5f);
    }
        private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= 5f) {
                if(transform.name.Contains("Yellow"))Achievements.UnlockBlackYellowSpiderSpotted();
                if(transform.name.Contains("Green"))Achievements.UnlockRedGreenSpiderSpotted();
                return true;
            }
        }
        return false;
    }
    void Update()
    {
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the enemy!");
        } 
        if (!hasStarted)
        {
            startDelay -= Time.deltaTime;
            if (startDelay <= 0)
            {
                hasStarted = true;
                timer = 0f; // Ensure timer starts fresh
            }
            return;
        }
        timer += Time.deltaTime;
        
        // Update spider position
        float newY = startPosition.y + direction * moveDistance * Mathf.Sin((timer / moveDuration) * Mathf.PI);
        transform.position = new Vector2(transform.position.x, newY);
        
        // Update web line
        UpdateWebLine(transform.position);
        
        if (timer >= moveDuration)
        {
            direction *= -1;
            timer = 0f;
        }
    }
    
    void UpdateWebLine(Vector2 spiderPosition)
    {
        for (int i = 0; i < WEB_POINTS; i++)
        {
            float t = i / (float)(WEB_POINTS - 1);
            
            // Add slight curve to web using sine wave
            float xOffset = Mathf.Sin(t * Mathf.PI * 2 + Time.time * 2) * 0.1f;
            
            // Interpolate between web start point and current spider position
            Vector3 newPoint = Vector3.Lerp(
                webStartPoint,
                spiderPosition,
                t
            );
            
            // Add subtle wave effect
            newPoint.x += xOffset * (1 - t); // Less wave effect near the spider
            
            webLine.SetPosition(i, newPoint);
        }
    }
    
    // Optional: Add this method to set web start point if needed
    public void SetWebStartPoint(Vector2 newStartPoint)
    {
        webStartPoint = newStartPoint;
    }
}