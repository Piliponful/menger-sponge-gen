using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshFromScratch : MonoBehaviour
{
    int prevMeshHolderCount = 0;

    public GameObject[] meshHolder = new GameObject[2];
    public string seed;
    public bool useRandomSeed = false;
    bool isFirst = true;

    public Material material;

    public float randomRange = 1;

    public float RandomRange = 0;
    public float XRand = 1;
    public float ZRand = 1;
    public int iteratorRandX = 3;
    public int iteratorRandZ = 3;

    public int iteration = 1;

    public int xScale = 3;
    public int yScale = 3;
    public int zScale = 3;

    float xPow;
    float yPow;
    float zPow;

    byte[, ,] cube;

    List<Vector3> newVerts = new List<Vector3>();
    List<int> newTris = new List<int>();
    List<Vector2> newUvs = new List<Vector2>();
    int vertsCount = 0;
    int squareCount = 0;
    int meshCount = 0;

    float scale = 27;
    float divisions = 3;

    [Range(0, 1)]
    public float randomValue = .5f;
    [Range(0, 1)]
    public float randomValue2 = .5f;
    [Range(0, 1)]
    public float randomValue3 = .5f;
    [Range(0, 1)]
    public float randomValue4 = .5f;
    [Range(1, 5)]
    public int subDiv = 3;

    float randomValue4_1 = 0;
    float randomValue4_2 = 0;
    float randomValue4_3 = 0;
    float randomValue4_4 = 0;

    void Start()
    {
        if (useRandomSeed == true) seed = Random.value.ToString();
        Random.seed = seed.GetHashCode();
        xPow = Mathf.Pow(xScale, iteration);
        yPow = Mathf.Pow(yScale, iteration);
        zPow = Mathf.Pow(zScale, iteration);
        cube = new byte[(int)xPow * 20, (int)yPow * 20, (int)zPow * 20];
        CubeGen();
        NewMesh();
    }
    void CubeGen()
    {
        for (int x = 0; x < Mathf.Pow(xScale, iteration); x++)
        {
            for (int y = 0; y < Mathf.Pow(yScale, iteration); y++)
            {
                for (int z = 0; z < Mathf.Pow(zScale, iteration); z++)
                {
                    if (isSierpinskiCarpetPixelFilled(x, z) || isSierpinskiCarpetPixelFilled(y, x) || isSierpinskiCarpetPixelFilled(y, z)) continue;

                    //if (y == Mathf.Pow(yScale, iteration) - 1 && Random.value > randomValue) continue;

                    if (Random.value > randomValue2) { CubeSplit(x, y, z, 1); continue; }

                    BuildCube(x, y, z, scale);
                }
            }
        }
    }
    void CubeSplit(float a, float b, float c, int pow)
    {
        float value2 = 0;
        if (pow == 1) value2 = 0;
        if (pow == 2) value2 = 1;
        if (pow == 3) value2 = 6;
        if (pow == 4) value2 = 23;
        if (pow == 5) value2 = 76;

        float _scale = scale / Mathf.Pow(divisions, pow);

        float value = pow + value2;

        a = a * (Mathf.Pow(divisions, pow) / value);
        b = b * (Mathf.Pow(divisions, pow) / value);
        c = c * (Mathf.Pow(divisions, pow) / value);

        for (float x = a; x < a + divisions; x++)
        {
            for (float y = b; y < b + divisions; y++)
            {
                for (float z = c; z < c + divisions; z++)
                {
                    if (y == b + divisions - 1 && Random.value > randomValue3) continue;

                    if (pow < subDiv && Random.value > randomValue4) { CubeSplit(x, y, z, pow + 1); continue; }

                    BuildCube(x, y, z, _scale);
                }
            }
        }
    }
    bool isSierpinskiCarpetPixelFilled(int x, int z)
    {
        while (x > 0 || z > 0) // when either of these reaches zero the pixel is determined to be on the edge 
        // at that square level and must be filled
        {
            if (x % iteratorRandX == XRand && z % iteratorRandZ == ZRand) //checks if the pixel is in the center for the current square level
                return true;
            x /= 3; //x and y are decremented to check the next larger square level
            z /= 3;
        }
        return false; // if all possible square levels are checked and the pixel is not determined 
        // to be open it must be filled
    }
    void CubeTop(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_1) * _scale, (z + 1) * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_2) * _scale, (z + 1) * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_3) * _scale, z * _scale));
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_4) * _scale, z * _scale));
        CubeTU(T, U);
    }
    void CubeBottom(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3(x * _scale, y * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, y * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, y * _scale, (z + 1) * _scale));
        V.Add(new Vector3(x * _scale, y * _scale, (z + 1) * _scale));
        CubeTU(T, U);
    }
    void CubeNorth(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3((x + 1) * _scale, y * _scale, (z + 1) * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_2) * _scale, (z + 1) * _scale));
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_1) * _scale, (z + 1) * _scale));
        V.Add(new Vector3(x * _scale, y * _scale, (z + 1) * _scale));
        CubeTU(T, U);
    }
    void CubeSouth(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3(x * _scale, y * _scale, z * _scale));
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_4) * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_3) * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, y * _scale, z * _scale));
        CubeTU(T, U);
    }
    void CubeWest(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3(x * _scale, y * _scale, (z + 1) * _scale));
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_1) * _scale, (z + 1) * _scale));
        V.Add(new Vector3(x * _scale, (y + 1 + randomValue4_4) * _scale, z * _scale));
        V.Add(new Vector3(x * _scale, y * _scale, z * _scale));
        CubeTU(T, U);
    }
    void CubeEast(List<Vector3> V, List<Vector2> U, List<int> T, float x, float y, float z, float _scale)
    {
        vertsCount += 4;
        V.Add(new Vector3((x + 1) * _scale, y * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_3) * _scale, z * _scale));
        V.Add(new Vector3((x + 1) * _scale, (y + 1 + randomValue4_2) * _scale, (z + 1) * _scale));
        V.Add(new Vector3((x + 1) * _scale, y * _scale, (z + 1) * _scale));
        CubeTU(T, U);
    }
    void CubeTU(List<int> T, List<Vector2> U)
    {
        T.Add(squareCount * 4); T.Add((squareCount * 4) + 1); T.Add((squareCount * 4) + 2); T.Add((squareCount * 4)); T.Add((squareCount * 4) + 2); T.Add((squareCount * 4) + 3);
        U.Add(new Vector2(0, 0)); U.Add(new Vector2(1, 0)); U.Add(new Vector2(1, 1)); U.Add(new Vector2(0, 1));
        squareCount++;
    }
    void CreateGameObject(int n, Mesh mesh)
    {
        int meshHolderCount = 0;
        if (isFirst == true)
        {
            meshHolder[meshHolderCount] = new GameObject("MeshHolder" + meshHolderCount);

            meshHolderCount++;
            prevMeshHolderCount = meshHolderCount - 1;
            isFirst = false;
        }

        mesh.RecalculateBounds();
        ;
        mesh.RecalculateNormals();

        GameObject GO = new GameObject("mesh" + n);
        GO.transform.parent = meshHolder[prevMeshHolderCount].transform;
        meshHolder[prevMeshHolderCount].transform.position = Vector3.zero;
        GO.transform.localPosition = Vector3.zero;
        GO.AddComponent<MeshRenderer>().material = material;
        GO.GetComponent<MeshRenderer>().sharedMaterial = material;
        GO.AddComponent<MeshFilter>().mesh = mesh;
        GO.AddComponent<MeshCollider>();

        squareCount = 0;
    }
    void BuildCube(float x, float y, float z, float scale)
    {
        if (vertsCount >= 64000) NewMesh();
        //if (GetByte(x, y + 1, z) == 1)
        CubeTop(newVerts, newUvs, newTris, x, y, z, scale);
        //if (GetByte(x, y - 1, z) == 1)
        CubeBottom(newVerts, newUvs, newTris, x, y, z, scale);
        //if (GetByte(x, y, z + 1) == 1)
        CubeNorth(newVerts, newUvs, newTris, x, y, z, scale);
        //if (GetByte(x, y, z - 1) == 1)
        CubeSouth(newVerts, newUvs, newTris, x, y, z, scale);
        //if (GetByte(x + 1, y, z) == 1)
        CubeEast(newVerts, newUvs, newTris, x, y, z, scale);
        //if (GetByte(x - 1, y, z) == 1)
        CubeWest(newVerts, newUvs, newTris, x, y, z, scale);
    }
    void NewMesh()
    {
        vertsCount = 0;

        Mesh mesh = new Mesh();
        mesh.name = "mesh" + meshCount;
        mesh.vertices = newVerts.ToArray();
        mesh.uv = newUvs.ToArray();
        mesh.triangles = newTris.ToArray();

        CreateGameObject(meshCount, mesh);

        newTris.Clear();
        newUvs.Clear();
        newVerts.Clear();

        meshCount++;
    }
    byte GetByte(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 || x >= Mathf.Pow(xScale, iteration) || y >= Mathf.Pow(yScale, iteration) || z >= Mathf.Pow(zScale, iteration)) return 1; else return cube[x, y, z];
    }
}

