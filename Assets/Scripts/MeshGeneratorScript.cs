using UnityEngine;


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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = baseMesh;
        CalculateVertexNumber();
        createShape();
        meshupdate();
       // OnDrawGizmos();
    }

    void createShape()
    {
        Vertacies = new Vector3[VertexCount];

        for (int z = 0; z <= zSize; z++)// per the tutorial 'i' could be imbeded in the for loop to make it unique to only this loop. for (int i = 0, int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vertacies[i] = new Vector3(x,0,z);
                i++;
            }
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

    private void OnDrawGizmos()
    {
       if (Vertacies == null)
        {
            return;
        }
        
        
        for (i = 0; i < VertexCount; i++)
        
        {
            Gizmos.DrawSphere(Vertacies[i], .1f);

        }
  
        
    }




    //private void Update()
    //{

    //baseMesh.Clear();
    //baseMesh.vertices = Vertacies;   
    //baseMesh.triangles = triangles;

    //baseMesh.RecalculateNormals();

    //}


}
