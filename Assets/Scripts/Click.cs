using UnityEngine;

public class Click : MonoBehaviour
{
    private bool isControlling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isControlling) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit has a specific tag or component
                if (hit.collider.gameObject.CompareTag("ClickableSprite"))
                {
                    isControlling = true;
                    Debug.Log("Sprite clicked: " + hit.collider.name);
                    // Do your thing here
                    hit.collider.gameObject.GetComponent<Food>().OnClick();
                }
            }
        }
    }

    public void StopControlling()
    {
        isControlling = false;
    }

    public void StartControlling()
    {
        isControlling = true;
    }
}
