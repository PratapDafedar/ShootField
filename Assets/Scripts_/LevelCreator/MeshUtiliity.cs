using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshUtility
{
    static List<Vector3> vertices;
    static List<Vector3> normals;
    // [... all other vertex data arrays you need]

    static List<int> indices;
    static Dictionary<uint, int> newVectices;

    static int GetNewVertex(int i1, int i2)
    {
        // We have to test both directions since the edge
        // could be reversed in another triangle
        uint t1 = ((uint)i1 << 16) | (uint)i2;
        uint t2 = ((uint)i2 << 16) | (uint)i1;
        if (newVectices.ContainsKey(t2))
            return newVectices[t2];
        if (newVectices.ContainsKey(t1))
            return newVectices[t1];
        // generate vertex:
        int newIndex = vertices.Count;
        newVectices.Add(t1, newIndex);

        // calculate new vertex
        vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
        normals.Add((normals[i1] + normals[i2]).normalized);
        // [... all other vertex data arrays]

        return newIndex;
    }


    public static Mesh Subdivide(Mesh mesh, int iteration = 1)
    {
        newVectices = new Dictionary<uint, int>();

        vertices = new List<Vector3>(mesh.vertices);
        normals = new List<Vector3>(mesh.normals);
        // [... all other vertex data arrays]
        indices = new List<int>();

        int[] triangles = mesh.triangles;
        SubDeviceIterated(triangles, iteration);

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        // [... all other vertex data arrays]
        mesh.triangles = indices.ToArray();

        // since this is a static function and it uses static variables
        // we should erase the arrays to free them:
        newVectices = null;
        vertices = null;
        normals = null;
        // [... all other vertex data arrays]

        indices = null;

        return mesh;
    }

    private static void SubDeviceIterated(int[] triangles, int iteration)
    {
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i1 = triangles[i + 0];
            int i2 = triangles[i + 1];
            int i3 = triangles[i + 2];

            int a = GetNewVertex(i1, i2);
            int b = GetNewVertex(i2, i3);
            int c = GetNewVertex(i3, i1);
            indices.Add(i1); indices.Add(a); indices.Add(c);
            indices.Add(i2); indices.Add(b); indices.Add(a);
            indices.Add(i3); indices.Add(c); indices.Add(b);
            indices.Add(a); indices.Add(b); indices.Add(c); // center triangle
        }

        if (iteration > 1)
        {
            int[] _subTriangles = indices.ToArray();
            indices.Clear();
            SubDeviceIterated(_subTriangles, iteration - 1);
        }
    }


    public static void CombinetheMeshesInChildren(Transform parentObject)
    {
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 1; // Just to skip the parent, since parent comes at 0th position.
        Material savedMaterial = null;
        if (meshFilters.Length > 1)
        {
            savedMaterial = meshFilters[1].gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                GameObject.Destroy(meshFilters[i].gameObject);
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
        }

        GameObject childReplacement = new GameObject("Wall-Mesh-Combine");
        childReplacement.transform.parent = parentObject;
        childReplacement.AddComponent<MeshFilter>().mesh = new Mesh();

        MeshRenderer mr = childReplacement.AddComponent<MeshRenderer>();
        mr.material = savedMaterial;

        childReplacement.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        MeshCollider mc = childReplacement.AddComponent<MeshCollider>();
        mc.sharedMesh = childReplacement.GetComponent<MeshFilter>().mesh;

        childReplacement.gameObject.isStatic = true;
        childReplacement.gameObject.SetActive(true);
    }
}