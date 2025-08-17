using UnityEngine;

public class BubleSpawn : MonoBehaviour
{
    [SerializeField] Transform buble;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 200; i++)
        {
            var position  = Random.insideUnitCircle * 20f; ;
            Instantiate(buble,position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
