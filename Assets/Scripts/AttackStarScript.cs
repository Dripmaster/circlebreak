using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStarScript : MonoBehaviour
{
    playerMovwe _playerMove;
    public GameObject fallStar;
    SpriteRenderer sr;
    bool isSpawning;
    float eTime = 0;
    float alp = 0.1f;
    private void OnEnable()
    {
        isSpawning = false;
        eTime = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(eTime <= 1f)
        {
            eTime += Time.deltaTime;
            alp = Mathf.Lerp(0.1f, 1f, eTime / 1);
            Color c = sr.color;
            c.a = alp;
            sr.color = c;
            if (eTime >=1f)
            {
                isSpawning = false;
                StartCoroutine(fallen());
            }
        }
    }
    IEnumerator fallen()
    {
        float startTime = 0;
        do
        {
            startTime += Time.deltaTime;
            float x = Mathf.Lerp(10, 0, startTime / 1);
            fallStar.transform.localPosition = new Vector3(0, x, 0);
            if (startTime >= 1)
            {
                break;
            }
            yield return null;
        } while (true);
        Color c;
        c = sr.color;
        c.a = 0;
        sr.color = c;
        startTime = 0;
        sr = fallStar.GetComponent<SpriteRenderer>();
        do
        {
            startTime += Time.deltaTime;
            float x = Mathf.Lerp(1, 0, startTime / 0.5f);
            Color cc = sr.color;
            cc.a = x;
            sr.color =cc;
            if (startTime >= 0.5f)
            {
                break;
            }
            yield return null;
        } while (true);
        Destroy(gameObject);
    }
    public void setDest(float theta, float Range, float _timeOfSpawn, playerMovwe p)
    {
        _playerMove = p;
        isSpawning = true;
        Vector2 dest;
        dest = Vector2.zero;

        dest.x = Range * Mathf.Cos(-theta);
        dest.y = Range * Mathf.Sin(-theta);

        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * -theta);

        Vector2 newDest = dest;
        newDest.x *= _playerMove.GetBigCircleRatio().x;
        newDest.y *= _playerMove.GetBigCircleRatio().y;
        newDest = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newDest;

        newDest += (Vector2)transform.parent.position;
        transform.position = newDest;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (fallStar.transform.localPosition.y <= 0.3f)
        {
            if (!isSpawning && collision.tag == "Player")
            {
                if (!_playerMove.isDashOrFever())
                {
                    _playerMove.starCollisionEnter(this);
                }
                if (_playerMove.isFever())
                {
                    gameObject.SetActive(false);
                }
                if (_playerMove.isBoom())
                {
                    gameObject.SetActive(false);
                }
            }
            if (!isSpawning && collision.tag == "dashCollider")
            {
                if (_playerMove.isDashOrFever())
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
 
