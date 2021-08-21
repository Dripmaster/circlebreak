using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerSpawner : MonoBehaviour
{
    private float spawnTimer = 0f;
    public GameObject robotPrefab;
    public GameObject lazerPrefab;
    public playerMovwe _playerMove;
    // Start is called before the first frame update
    void Start()
    {
        spawnLazerRobot();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer > 10) {
            spawnTimer = 0f;
            spawnLazerRobot();
        }
    }

    public void spawnLazerRobot() {
        Vector2 source;
        Vector2 destination;
    
        var g = Instantiate(robotPrefab);

        Vector3 screen = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
        Vector2 robotSize = g.GetComponent<SpriteRenderer>().bounds.size;

        int direction = Random.Range(0, 2);
        float spawnPositionY = Random.Range(-screen.y + 1, screen.y - robotSize.y - 1);
        if(direction == 0) {
            source = new Vector2(screen.x - robotSize.x / 2 - 1, spawnPositionY);
            destination = new Vector2(-7, spawnPositionY);
        }
        else {
            source = new Vector2(-screen.x + robotSize.x / 2 + 1, spawnPositionY);
            destination = new Vector2(7, spawnPositionY);

            g.GetComponent<LazerRobot>().transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        g.GetComponent<LazerRobot>().setPositions(source, destination);
        g.GetComponent<LazerRobot>().lazerPrefab = lazerPrefab;
        g.GetComponent<LazerRobot>()._playerMove = _playerMove;
    }
}
