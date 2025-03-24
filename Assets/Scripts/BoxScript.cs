using UnityEngine;
using TMPro;
using System.Collections;

public class BoxScript : MonoBehaviour
{
    public int boxValue = 2;
    public GameObject boxPrefab;
    public Material materials;
    public TextMeshPro[] textBox;
    public WallStep wall;

    public GameManager gameManager;
    

    private Rigidbody rb;
    [SerializeField]
    public int colorIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wall = FindFirstObjectByType<WallStep>();
        gameManager = FindFirstObjectByType<GameManager>();

        UpdateBox();
    }

    private void OnCollisionEnter(Collision collision)
    {
        BoxScript boxScript = collision.gameObject.GetComponent<BoxScript>();

        if (boxScript != null && boxValue == boxScript.boxValue)
        {
            // Đảm bảo chỉ hộp có giá trị hashcode thấp hơn xử lý va chạm
            if (GetInstanceID() > boxScript.GetInstanceID()) return;

            gameManager.Score((boxValue + boxScript.boxValue));

            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            if (boxPrefab != null)
            {
                GameObject spawnBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
                BoxScript newBoxScript = spawnBox.GetComponent<BoxScript>();

                newBoxScript.rb = spawnBox.GetComponent <Rigidbody>();
                newBoxScript.rb.AddForce(Vector3.up * 1f, ForceMode.Impulse);
                newBoxScript.rb.AddTorque(new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)), ForceMode.Impulse);

                if (newBoxScript != null)
                {
                    newBoxScript.boxValue = boxValue * 2;
                    newBoxScript.colorIndex = colorIndex + 1;
                    newBoxScript.UpdateBox();
                    
                }
            }
            GameManager.Instance.PlaySound(1);

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            Destroy(gameObject,0.5f);
            Destroy(collision.gameObject,0.5f);

            wall.MoveWall(true);

        }
    }

    
    private void UpdateBox()
    {
        foreach (TextMeshPro text in textBox)
        {
            if (text != null)
            {
                text.text = boxValue.ToString();
            }
        }
        ChangeColor();
    }

    
    private void ChangeColor()
    {
        if (GetComponent<Renderer>() != null)
        {
            Color[] colors = {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta
             };


            colorIndex = (colorIndex) % colors.Length;

            GetComponent<Renderer>().material.color = colors[colorIndex];
        }
    }

    public void ShootBox(Vector3 shootDirection, float shootForce)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-2,2), Random.Range(-2, 2), Random.Range(-2,2)), ForceMode.Impulse); 
    }

    

}
