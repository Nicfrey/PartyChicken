using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PropShowingBehavior : MonoBehaviour
{
    [SerializeField] 
    private Transform propToMove;
    private bool hasProp = false;
    private float startY;
    private float angle;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float amplitude = 0.5f;
    [SerializeField]
    private float rotationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        startY = propToMove.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        if (hasProp)
        {
            angle += Time.deltaTime;
            float newY = startY + Mathf.Sin(angle * speed) * amplitude;
            propToMove.transform.position = new Vector3(propToMove.transform.position.x, newY, propToMove.transform.position.z);
            propToMove.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * rotationSpeed);
        }
    }

    public void HasProp(bool hasProp)
    {
        this.hasProp = hasProp;
    }
}
