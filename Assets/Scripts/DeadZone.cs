using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    public GameObject menu;
    public test testScript;

    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            testScript.isMenu = true;
            menu.SetActive(true);
            GameManager.Instance.PlaySound(2);
        }

    }
}
