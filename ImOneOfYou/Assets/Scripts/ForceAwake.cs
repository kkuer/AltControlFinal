using UnityEngine;

public class ForceAwake : MonoBehaviour
{
    public GameManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager.enabled = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.enabled == false)
        {
            manager.enabled = true; 
        }
    }
}
