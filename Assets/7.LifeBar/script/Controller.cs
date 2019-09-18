using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private LifeBar _bar;
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if(canvas == null)
        {
            Debug.Log("场景中没有Canvas组件");
            return;
        }

        SpawnLifeBar(canvas);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Move(Vector3.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector3.down);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _bar.ChangeLife(-50);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _bar.ChangeLife(50);
        }
    }

    private void Move(Vector3 move)
    {
        transform.Translate(move * Time.deltaTime * 5);
    }

    private void SpawnLifeBar(Canvas canvas)
    {
        GameObject prefab = Resources.Load<GameObject>("LifeBar");
        _bar = Instantiate(prefab, canvas.transform).AddComponent<LifeBar>();
        List<LifeBarData> data = new List<LifeBarData>()
        {
            new LifeBarData(null, Color.red),
            new LifeBarData(null, Color.yellow),
            new LifeBarData(null, Color.blue)
        };
        _bar.init(transform, 350, data);
    }
}
