using System.Collections;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject[] prefabs;
    private GameObject currentPrefab;
    private GameObject nextPrefab;

    public GameObject activeBox2;
    public GameObject activeBox4;

    public int trajectorySteps = 50; // Số điểm trên quỹ đạo
    public float timeStep = 0.02f;   // Khoảng cách giữa các điểm
    public float fixedLineSpeed = 10f; // Tốc độ cố định khi vẽ quỹ đạo

    public WallStep wall;

    private Vector3 startTouch;
    private Vector3 endTouch;
    private Vector3 directionTouch;
    private float shootForce;
    private bool canShoot = true;
    public bool isMenu = false;

    public GameObject menu;
    public LineRenderer lineRenderer;

    void Start()
    {
        menu.SetActive(false);
        wall = FindFirstObjectByType<WallStep>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        currentPrefab = prefabs[Random.Range(0, prefabs.Length)];
        nextPrefab = prefabs[Random.Range(0, prefabs.Length)];

        UpdateActiveBox();
    }

    void Update()
    {
        if (!canShoot || isMenu) return;

        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                startTouch = touchPos;
                lineRenderer.enabled = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lineRenderer.enabled = true;

                endTouch = touchPos;
                UpdateTrajectory();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lineRenderer.enabled = false;
                Shoot();
            }
        }
    }

    void UpdateTrajectory()
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(startTouch.x, startTouch.y, Camera.main.transform.position.y));
        Vector3 worldEnd = Camera.main.ScreenToWorldPoint(new Vector3(endTouch.x, endTouch.y, Camera.main.transform.position.y));

        directionTouch = new Vector3(worldStart.x - worldEnd.x, 5f, worldStart.z - worldEnd.z).normalized;
        shootForce = Vector3.Distance(worldStart, worldEnd) * 3f;

        DrawTrajectory(transform.position, directionTouch * shootForce);
    }

    void Shoot()
    {
        BoxScript box = Instantiate(currentPrefab, transform.position, Quaternion.identity).GetComponent<BoxScript>();
        box.ShootBox(directionTouch, shootForce);

        currentPrefab = nextPrefab;
        nextPrefab = prefabs[Random.Range(0, prefabs.Length)];

        activeBox2.SetActive(false);
        activeBox4.SetActive(false);

        wall.MoveWall(false);
        StartCoroutine(ShootCooldown());
        GameManager.Instance.PlaySound(0);
    }

    void DrawTrajectory(Vector3 startPos, Vector3 initialVelocity)
    {
        Vector3[] points = new Vector3[trajectorySteps];
        points[0] = startPos;
        Vector3 velocity = initialVelocity;
        Vector3 currentPosition = startPos;

        for (int i = 1; i < trajectorySteps; i++)
        {
            velocity += Physics.gravity * timeStep;
            currentPosition += velocity * timeStep;
            points[i] = currentPosition;
        }

        lineRenderer.positionCount = trajectorySteps;
        lineRenderer.SetPositions(points);
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1);
        UpdateActiveBox();
        canShoot = true;
    }

    void UpdateActiveBox()
    {
        activeBox2.SetActive(nextPrefab == prefabs[0]);
        activeBox4.SetActive(nextPrefab == prefabs[1]);
    }
}
