using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderData;

[RequireComponent(typeof(MeshFilter))]

public class MeshGeneratorScript : MonoBehaviour
{
    Mesh baseMesh;

    Vector3[] Vertacies;
    int[] triangles;
    Vector2[] UVCords;
    Color[] TopoShader;
    public Gradient altitudes;
    public int xSize = 40;
    public int zSize = 40;
    public int i = 0;
    public int VertexCount;
    public int Verts=0;
    public int Tris=0;
    public float yVal;
     public float yPass1;
    public float yPass2;
    public float yPass3;
    private float minTerraineHeight;
    private float maxTerraineHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        
        
        baseMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = baseMesh;
        CalculateVertexNumber();
        StartCoroutine(createShape());
        //meshupdate();
        //OnDrawGizmos();
    }

    //void createShape()
    IEnumerator createShape()
    {
        Vertacies = new Vector3[VertexCount];

        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                yVal = (yPass1 - yPass2 + yPass3) * 3;

                yPass1 = Mathf.PerlinNoise(x * .25f, z * .5f) * 2f;
                yPass2 = Mathf.PerlinNoise(x * .65f, z * .3f) * 3f;
                yPass2 = Mathf.PerlinNoise(x * .6f, z * .1f) * .5f;

                Vertacies[i] = new Vector3(x, yVal, z);

                if (yVal > maxTerraineHeight)
                {
                    maxTerraineHeight = yVal;
                }
                if (yVal < minTerraineHeight)
                {
                    minTerraineHeight = yVal;
                }

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize; z++)
        {

            for (int x = 0; x < xSize; x++)
            {

                triangles[Tris + 0] = Verts + 0;
                triangles[Tris + 1] = Verts + xSize + 1;
                triangles[Tris + 2] = Verts + 1;
                triangles[Tris + 3] = Verts + 1;
                triangles[Tris + 4] = Verts + xSize + 1;
                triangles[Tris + 5] = Verts + xSize + 2;

                Verts++;
                Tris += 6;

                yield return new WaitForSeconds(.025f);
            }
            Verts++;
        }

        TopoShader = new Color[Vertacies.Length];
        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {

                float height = Mathf.InverseLerp(minTerraineHeight, maxTerraineHeight, Vertacies[i].y);
                TopoShader[i] = altitudes.Evaluate(height);

                i++;
            }
        }

        UVCords = new Vector2[Vertacies.Length];
        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                UVCords[i] = new Vector2((float)x / xSize,(float) z / zSize);// (float) ensures a float value conversion
                i++;
            }
        }



    }
    private void Update()
    {
       meshupdate();
    }

    void meshupdate()
    {
        baseMesh.Clear();
        baseMesh.vertices = Vertacies;
        baseMesh.triangles = triangles;
        baseMesh.colors = TopoShader;
        baseMesh.uv = UVCords;


        baseMesh.RecalculateNormals();
    }


    void CalculateVertexNumber()
    {
        VertexCount = (xSize + 1) * (zSize + 1);
    }

   void OnDrawGizmos()
    {
       if (Vertacies == null)
        {
            return;
        }

        //for (i = 0; i < VertexCount; i++)
        for (i = 0; i < Vertacies.Length; i++)
        
        {
            Gizmos.DrawSphere(Vertacies[i], .1f);

        }
  
        
    }

    //void Colour1() //definable colours modifies original source shader on item the code is attahed to 
    //{
    //    {
    //    Renderer myRenderer = GetComponent<Renderer>();
    //    Color colour1 = new Color(0.2f, 0.6f, 1.0f, 1.0f);
    //    myRenderer.material.SetColor("_BaseColor", colour1);
    //}
    //void Colour2()// definable colours modifies original source shader on item the code is attahed to 
    //{
    //    Renderer myRenderer = GetComponent<Renderer>();
    //    Color colour1 = new Color(0.2f, 0.6f, 1.0f, 1.0f);
    //    myRenderer.material.SetColor("_BaseColor", colour1);
    //}


}
