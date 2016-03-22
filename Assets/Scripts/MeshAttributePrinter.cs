using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 Unity没有建模工具，大多是在其他3D软件中建模后导入Unity资源中使用。但通过脚本可以修改网格的顶点属性，所以理论上可以实现在Unity中从无到有地建模。


建模，就是建网格；建网格，就是画些三角形；画个三角形呢，也就是定位三个点。


不过首先了解下Unity中网格的特性。Unity中的对象就是GameObject了，每个GameObject都可以有一个MeshFilter组件（也可以没有），该组件又有mesh属性（这个一定有），而该属性又有个vertives，也就是一个Vector3数组，储存着顶点信息。


下面就是写个脚本来看看mesh里的东东都是些什么了。


代码功能即：点击Tab键轮询场景中所有GameObject，以获取其MeshFilter.mesh，并在GUI中显示mesh的主要属性内容顶点坐标，法线，三角形的绘制序列等等。（代码写的很仓促，只为了显示内容。）

代码直接拖到mainCamera中即可。可在场景中建几个Cube、Plane什么的看看。
  
 */



public class MeshAttributePrinter : MonoBehaviour
{

    private string text_name;
    private string text_vertices;
    private string text_normals;
    private string text_triangles;
    private string text_uv;
    private string text_tangents;

    public MeshFilter mCurrentFilter;

    public List<MeshFilter> targets;

    void Start()
    {

        targets = new List<MeshFilter>();
        AddAllTargets();

        mCurrentFilter = null;

    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            TargetMesh();
            FillText();
        }
    }

    void OnGUI()
    {


        float _x = 10;
        Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(text_name));
        GUI.Label(new Rect(_x, 10, textSize.x, textSize.y), text_name);

        textSize = GUI.skin.label.CalcSize(new GUIContent(text_triangles));
        GUI.Label(new Rect(_x, 30, textSize.x, textSize.y), text_triangles);

        _x += textSize.x + 20;
        textSize = GUI.skin.label.CalcSize(new GUIContent(text_vertices));
        GUI.Label(new Rect(_x, 30, textSize.x, textSize.y), text_vertices);

        _x += textSize.x + 20;
        textSize = GUI.skin.label.CalcSize(new GUIContent(text_normals));
        GUI.Label(new Rect(_x, 30, textSize.x, textSize.y), text_normals);

        _x += textSize.x + 20;
        textSize = GUI.skin.label.CalcSize(new GUIContent(text_tangents));
        GUI.Label(new Rect(_x, 30, textSize.x, textSize.y), text_tangents);

        _x += textSize.x + 20;
        textSize = GUI.skin.label.CalcSize(new GUIContent(text_uv));
        GUI.Label(new Rect(_x, 30, textSize.x, textSize.y), text_uv);

    }

    public void AddAllTargets()
    {

        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
            if (go.GetComponent<MeshFilter>() != null)
                AddTarget(go.GetComponent<MeshFilter>());
    }

    public void AddTarget(MeshFilter target)
    {

        targets.Add(target);
    }

    private void TargetMesh()
    {

        if (mCurrentFilter == null)
        {
            mCurrentFilter = targets[0];
        }
        else
        {
            int index = targets.IndexOf(mCurrentFilter);
            if (index < targets.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            mCurrentFilter = targets[index];
        }
    }

    private void FillText()
    {

        text_name = "Name: " + mCurrentFilter.gameObject.name;

        Mesh mesh = mCurrentFilter.mesh;

        int size = mesh.vertexCount;
        text_vertices = "vertices: " + size + "\n";
        for (int i = 0; i < size; i++)
        {
            text_vertices += i + ": " + mesh.vertices[i][0] + "," + mesh.vertices[i][1] + "," + mesh.vertices[i][2] + ";\n";
        }

        size = mesh.normals.Length;
        text_normals = "normals: " + size + "\n";
        for (int i = 0; i < size; i++)
        {
            text_normals += mesh.normals[i].x + "," + mesh.normals[i].y + "," + mesh.normals[i].z + ";\n";
        }

        size = mesh.triangles.Length;
        text_triangles = "triangles: " + size + "\n";
        for (int i = 0; i < size / 3; i++)
        {
            text_triangles += mesh.triangles[3 * i] + "," + mesh.triangles[3 * i + 1] + "," + mesh.triangles[3 * i + 2] + ";\n";
        }

        size = mesh.uv.Length;
        text_uv = "uv: " + size + "\n";
        for (int i = 0; i < size; i++)
        {
            text_uv += mesh.uv[i][0] + "," + mesh.uv[i][1] + ";\n";

        }
        size = mesh.tangents.Length; text_tangents = "tangents: " + size + "\n";
        for (int i = 0; i < size; i++)
        {
            text_tangents += mesh.tangents[i][0] + ", " + mesh.tangents[i][1] + ", " + mesh.tangents[i][2] + ", " + mesh.tangents[i][3] + ";\n";
        }
    }
}
