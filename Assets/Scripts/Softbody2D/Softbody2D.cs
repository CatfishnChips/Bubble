using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Softbody2D : MonoBehaviour
{
    public Mesh mesh;
    public Vector3[] vertices;
    public int CenterPoint;
    public int verticesCount;
    public List<GameObject> points;
    public GameObject toBeInstantiated;

    [SerializeField] private LayerMask _ignoreLayerMask;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        verticesCount = vertices.Length;

        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject childObject = Instantiate(toBeInstantiated, gameObject.transform.position + vertices[i], Quaternion.identity) as GameObject;
            childObject.transform.parent = gameObject.transform;
            points.Add(childObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = points[i].transform.localPosition;
        }
        mesh.vertices = vertices;
    }

    private void Start()
    {
        DistanceJoint2D distanceJoint2D = GetComponent<DistanceJoint2D>();
        distanceJoint2D.connectedBody = points[0].GetComponent<Rigidbody2D>();

        for (int i = 0; i < points.Count; i++)
        {
            points[i].GetComponent<VertexPoint>().Softbody2D = this;

            if (i != CenterPoint)
            {
                if (i == points.Count - 1)
                {
                    points[i].GetComponent<HingeJoint2D>().connectedBody = points[1].GetComponent<Rigidbody2D>();
                }
                else
                {
                    points[i].GetComponent<HingeJoint2D>().connectedBody = points[i + 1].GetComponent<Rigidbody2D>();
                }
            }
            else
            {
                points[i].GetComponent<HingeJoint2D>().enabled = false;
            }
        }
    }

    public void HandleCollision(Collision2D col)
    {
        if ((_ignoreLayerMask & (1 << col.collider.gameObject.layer)) != 0)
        {
            return;
        }
        else 
        {
            PopBubble();
        }
    }

    public void PopBubble()
    {
        Debug.Log("Bubble Popped!");
        for (int i = 0; i < points.Count; i++)
        {
            GameObject point = points[i];
            Destroy(point);
        }
        points.Clear();
        Destroy(gameObject);
    }
}