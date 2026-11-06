using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderData;

[RequireComponent(typeof(MeshFilter))]
/////   was able to follow theser  tutorials 
///https://www.youtube.com/watch?v=eJEpeUH1EMg&list=PLMFV6nN6_-eY&index=4
/// https://www.youtube.com/watch?v=64NblGkAabk&list=PLMFV6nN6_-eY
///https://www.youtube.com/watch?v=lNyZ9K71Vhc&list=WL&index=1&t=604s
/// however these were designed usiong  unity 2018.2.10f1  most of the referencing for 
/// the gradient shader tutorial 3 is no longer viable so i researched the new process
/// and will annotate the code changes due to version differences as i fix the tutorial
/// lead archetecure to function withthe new render pipeline methedology



public class MeshGeneratorScript : MonoBehaviour
{
    Mesh baseMesh;

    Vector3[] Vertacies;
    int[] triangles;
   // Vector2[] UVCords;
    Color[] TopoShader;
    public Gradient altitudes;
    public int xSize = 40;
    public int zSize = 40;
    public int VertexCount;
    // public int Verts=0; // commented out to fix the mesh rendering collapse bug due to versioning change from  base tutorials -CF
    // public int Tris=0;  // commented out to fix the mesh rendering collapse bug  due to versioning change from  base tutorials-CF

    public float yVal;
     public float yPass1;
    public float yPass2;
    public float yPass3;
    //private float minTerraineHeight; //commented out due to versioning change from  base tutorials -CF
    //private float maxTerraineHeight; //commented out due to versioning change from  base tutorials -CF
    private float minTerraineHeight = float.MaxValue; 
    private float maxTerraineHeight = float.MinValue; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        
        
        baseMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = baseMesh;
        CalculateVertexNumber();
        //createShape();
        StartCoroutine(createShape());
        //meshupdate();
        //OnDrawGizmos();
    }

    //void createShape()
    IEnumerator createShape()
    {
        Vertacies = new Vector3[VertexCount];

        int i = 0;  //added here to protect against shared ref errors due to versioning change from  base tutorials -CF

        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                yPass1 = Mathf.PerlinNoise(x * .25f, z * .5f) * 2f;
                yPass2 = Mathf.PerlinNoise(x * .65f, z * .3f) * .5f;
                yPass3 = Mathf.PerlinNoise(x * .6f, z * .1f) * 1.3f;

                yVal = (yPass1 + yPass2 - yPass3) * 3;// moved to ensure the noise pass calculations are done before the y value calculation due to versioning change from  base tutorials -CF

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


      
        int Verts = 0;
        int Tris = 0;
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
        i = 0;//added to avoid out of range issues due to moved location of defining 'i' var due to versioning change from  base tutorials -CF

        TopoShader = new Color[Vertacies.Length];
        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {

            
            for (int x = 0; x <= xSize; x++)
            {
                if (i >= Vertacies.Length)//added to avoid accidental index  crashes due to versioning change from  base tutorials -CF
                {
                    break;
                }

                float currentHeight = Vertacies[i].y;// changed to avoid errors due to versioning change from  base tutorials -CF
                float heightNormalized = Mathf.InverseLerp(minTerraineHeight, maxTerraineHeight, currentHeight);// changed to avoid errors due to versioning change from  base tutorials -CF
                TopoShader[i] = altitudes.Evaluate(heightNormalized);// changed to avoid errors due to versioning change from  base tutorials -CF

                //float height = Mathf.InverseLerp(minTerraineHeight, maxTerraineHeight, Vertacies[i].y); // commented out for changes due to versioning change from  base tutorials -CF
                //TopoShader[i] = altitudes.Evaluate(height);// commented out for changes due to versioning change from  base tutorials -CF

                i++;
            }
        }
        //i= 0;   //added to avoid out of range issues due to moved location of defining 'i' var due to versioning change from  base tutorials -CF
        //UVCords = new Vector2[Vertacies.Length];
        //for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        //{
        //    for (int x = 0; x <= xSize; x++)
        //    {
        //        UVCords[i] = new Vector2((float)x / xSize,(float) z / zSize);// (float) ensures a float value conversion
        //        i++;
        //    }
        //}



    }
    private void Update()
    {
       meshupdate();
    }

    void meshupdate()
    {
        if (TopoShader == null || TopoShader.Length == 0) return;// added to prevent timing loop errors due to slower coroutine processing of the terraine due to versioning change from  base tutorials -CF

        baseMesh.Clear();
        baseMesh.vertices = Vertacies;
        baseMesh.triangles = triangles;
        baseMesh.colors = TopoShader;
       // baseMesh.uv = UVCords;


        baseMesh.RecalculateNormals();
    }


    void CalculateVertexNumber()
    {
        VertexCount = (xSize + 1) * (zSize + 1);
    }

   void OnDrawGizmos()
    {
        int i = 0;  //added here to protect against shared ref errors due to versioning change from  base tutorials -CF
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
