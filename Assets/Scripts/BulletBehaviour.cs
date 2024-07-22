using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float Speed { private get; set; } = 10f;
    public float Range { private get; set; } = 10f;
    private float distanceTravelled = 0f;
    public bool StartMove { private get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if(StartMove)
            Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        distanceTravelled += Speed * Time.deltaTime;

        if (distanceTravelled >= Range)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }
}
