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
                yVal= (yPass1 - yPass2 + yPass3)*3;

                yPass1 = Mathf.PerlinNoise(x * .25f, z * .5f) * 2f;
                yPass2 = Mathf.PerlinNoise(x * .65f, z * .3f) * 3f;
                yPass2 = Mathf.PerlinNoise(x * .6f, z * .1f) * .5f;

                Vertacies[i] = new Vector3(x, yVal, z);
                i++;
            }
        }

         triangles = new int[xSize * zSize *6];

        for (int z = 0; z < zSize; z++)
        {

            for (int x = 0; x < xSize; x++) 
            {

                triangles[Tris + 0] = Verts+0;
                triangles[Tris + 1] = Verts +xSize + 1;
                triangles[Tris + 2] = Verts + 1;
                triangles[Tris + 3] = Verts + 1;
                triangles[Tris + 4] = Verts + xSize +1;
                triangles[Tris + 5] = Verts +xSize + 2;
        
                Verts++;
                Tris += 6;

                yield return new WaitForSeconds(.025f);
            }
            Verts++;
        }
           
       


        #region Manual setup for mesh gen
        //Vertacies = new Vector3[]
        //{
        //    new Vector3(0,0,0),//0
        //    new Vector3(0,0,1),//1
        //    new Vector3(1,0,0),//2
        //    new Vector3(1,0,1),//3           
        //    new Vector3(1,0,2),//4
        //    new Vector3(2,0,2),//5
        //    new Vector3(2,0,1),//6       
        //    new Vector3(0,0,2),//7
        //    new Vector3(2,0,0),//8          
        //    new Vector3(2,0,3),//9
        //    new Vector3(3,0,3),//10
        //    new Vector3(3,0,2),//11
        //    new Vector3(1,0,3),//12
        //    new Vector3(3,0,1),//13
        //    new Vector3(0,0,3),//14
        //    new Vector3(3,0,0),//15

        //};

        //triangles = new int[]
        //{
        //    0, 1, 2,
        //    1,3,2,
        //    3,4,6,
        //    4,5,6,
        //    1,7,3,
        //    7,4,3,
        //    2,3,8,
        //    3,6,8,
        //    5,9,11,
        //    9,10,11,
        //    6,5,13,
        //    5,11,13,
        //    4,12,5,
        //    12,9,5,
        //    8,6,15,
        //    6,13,15,
        //    7,14,4,
        //    14,12,4

        //};
        #endregion

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
