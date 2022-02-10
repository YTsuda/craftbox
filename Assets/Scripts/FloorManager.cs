using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    private int force;
    private Mesh mesh;
    private Mesh oldMesh;
    private Vector3[] vertices;

    private Mesh colliderMesh;

    private GameObject hammer;
    private Vector3 hammerCenter;
    private float hammerForce = 0.001f;

    private float floorDepth = -0.05f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("===== Start");

        if (mesh == null) {
            oldMesh = mesh = GetComponent<MeshFilter>().mesh;
        }
        vertices = mesh.vertices;

        hammer = GameObject.Find("HammerContainer/Hammer");

        Vector3 p = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        print("========HIT====!!");
        print("RelatedVelocity:" + collision.relativeVelocity);
        print("Impulse:" + collision.impulse);
        print("ImpulseMagnitude:" + collision.impulse.magnitude);
        print("ContactLength:" + collision.contacts.Length);

        if (collision.impulse.magnitude > 1.0f) {
            print("DEFORM !!!!! ==== ");
            Vector3 hammerFace = new Vector3(-0.335f, -0.075f, 0);
            hammerCenter = transform.InverseTransformPoint(hammer.transform.TransformPoint(hammerFace));
            // Vector3 globalHammerNormal = hammer.transform.TransformPoint(new Vector3(0,1,0));
            // Debug.DrawRay(globalHammerFace, new Vector3(0,1,0), Color.red, 100);
            DeformIngot(collision);
            // Plane hammerPlane = new Plane(globalHammerNormal, globalHammerFace);
        }
    }

    void DeformIngot(Collision collision)
    {

        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Vector3 localPoint = transform.InverseTransformPoint(contact.point);
        //     Debug.DrawRay(contact.point, new Vector3(0,0.3f,0.3f), Color.red, 5);
        // }
        print("local_hammer_center:" + hammerCenter);
        Vector3 verticalCenter = new Vector3(hammerCenter.x, (hammerCenter.y + floorDepth) / 2, hammerCenter.z);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float horizontalDistance = Vector3.Distance(
                new Vector3(hammerCenter.x, 0, hammerCenter.z),
                new Vector3(vertex.x, 0, vertex.z)
            );
            float verticalDistance = Mathf.Abs(hammerCenter.y - vertex.y);
            Vector3 globalVertex = transform.TransformPoint(vertex);

            float hammerDiameter = 0.04f;
            float rippleDiameter = 0.08f;

            float hammerFaceDepth = 0.01f;
            Vector3 movedVertex = vertex;;

            if (horizontalDistance < hammerDiameter && verticalDistance < hammerFaceDepth) {
                // If hit directry

                // Debug.DrawRay(globalVertex, new Vector3(0,1,0), Color.red, 100);
                Vector3 force = new Vector3(0, -1 * hammerForce, 0);
                movedVertex = vertex + force;

                // Vector3 localPoint = transform.InverseTransformPoint(movedPoint);
                // Debug.DrawRay(localPoint, new Vector3(0,0.3f,0.3f), Color.red, 5);

            } else if (horizontalDistance < rippleDiameter && verticalDistance < hammerFaceDepth) {
                // if the hammer does'nt hit, vertices be down by 粘度

                // ハンマーから遠いVertexほど強く下降する
                float decayRate = (rippleDiameter - horizontalDistance) / (rippleDiameter - hammerDiameter);
                Vector3 force = new Vector3(0, -1 * hammerForce * decayRate, 0);
                movedVertex = vertex + force;
            }

            float distanceFromCenter = Vector3.Distance(verticalCenter, movedVertex);

            if (distanceFromCenter < 0.1f) {
                Vector3 moveDirection = movedVertex - verticalCenter;
                Vector3 force = new Vector3(moveDirection.x, 0, moveDirection.z);
                float forceModifier = 1 - distanceFromCenter / 0.1f;

                movedVertex = movedVertex + force * forceModifier * 0.04f;
            }

            vertices[i] = movedVertex;
            // Move vertices to horizontal from center

        }

        Debug.DrawRay(verticalCenter, new Vector3(0,0.3f,0.3f), Color.red, 5);
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Vector3 localPoint = transform.InverseTransformPoint(contact.point);
        //     int c = 0;

        //     float threshold = 0.03f;
        //     float powerOffset = 0.02f;

        //     for (int i = 0; i < vertices.Length; i++)
        //     {
        //         Vector3 vertex = vertices[i];
        //         float distance = Vector3.Distance(vertex, localPoint);
        //         c += 1;

        //         if (distance < threshold)
        //         {
        //             float forceRate = (threshold - distance) / threshold;
        //             Vector3 subnormal = contact.normal * forceRate * powerOffset;
        //             Vector3 movedPoint = vertex + subnormal;
        //             vertices[i] = movedPoint;
        //         }
        //     }

        //     mesh.vertices = vertices;
        //     mesh.RecalculateBounds();
        //     GetComponent<MeshCollider>().sharedMesh = null;
        //     GetComponent<MeshCollider>().sharedMesh = mesh;
        // }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void OnApplicationQuit() {
        GetComponent<MeshFilter>().mesh.vertices = oldMesh.vertices;
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = oldMesh;
    }
}
