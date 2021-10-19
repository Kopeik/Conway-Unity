using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class section_select : MonoBehaviour
{

    [SerializeField] celltalog catalog;
    [SerializeField] Camera cam;
    [SerializeField] GameObject cell;
    [SerializeField] Text coords;
    public float dragSpeed = 2;
    private Vector2 dragOrigin;
    bool drag_was_set = false;
    Vector2 mousePosition;
    float cameraSizer;
    bool deleteMode = true;
    // Start is called before the first frame update
    void Start()
    {
        cameraSizer = cam.orthographicSize;


    }


    // Update is called once per frame
    void Update()
    {



        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.Floor(mousePosition.x) + 0.5f, Mathf.Floor(mousePosition.y) + 0.5f, 1);
        coords.text = "X: " + Mathf.Floor(mousePosition.x) + "\nY: " + Mathf.Floor(mousePosition.y);
        coords.transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x + 1, transform.position.y + 1));
        //coords.transform.Translate(new Vector3(-0.5f, -0.5f));

        if (Input.GetMouseButton(0))
        {
            deleteMode = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Delete);
            if (!deleteMode)
            {
                GameObject cell_instance = GameObject.Instantiate(cell);
                cell_instance.transform.position = transform.position;
                if (catalog.addCell(cell_instance, catalog.celldict_new))
                    cell_instance.SetActive(true);
                else Destroy(cell_instance);
            }
            else
            {
                catalog.remove((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y), catalog.celldict_new);
            }
        }
        if (Input.GetMouseButton(1))

        {
            if (drag_was_set == false)
            {
                drag_was_set = true;
                dragOrigin = mousePosition;
            }
            cam.transform.position = new Vector3(cam.transform.position.x + (dragOrigin.x - mousePosition.x), cam.transform.position.y + (dragOrigin.y - mousePosition.y), cam.transform.position.z);
        }
        if (Input.GetMouseButtonUp(1))
        {
            drag_was_set = false;
        }

        cam.orthographicSize += cameraSizer * (1.0f / 5.0f) * (-1) * Input.mouseScrollDelta.y;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1.0f / 5 * cameraSizer, 5 * cameraSizer);


    }
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
