using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: Destroy old maze
//TODO: Other usability: starting coordinates etc.

public class MazeEditor : EditorWindow
{
    private Maze m_maze;
    private IntegerField m_width = null;
    private IntegerField m_height = null;
    private IntegerField m_seed = null;
    private IntegerField m_iterations = null;

    [MenuItem("GameBadges/Maze Editor")]
    public static void CreateMazeEditor()
    {
        MazeEditor wnd = GetWindow<MazeEditor>();
        wnd.titleContent = new GUIContent("Maze Editor");
    }

    [MenuItem("GameBadges/Create Cell Data")]

    public static void createCellData()
    {
        CellData cd = ScriptableObject.CreateInstance<CellData>();
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
        {
            AssetDatabase.CreateFolder("Assets", "Data");
        }
        AssetDatabase.CreateAsset(cd, "Assets/Data/celldata.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = cd;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement; //root of your tab
        Label label = new Label("Maze Editor 0.0.1.");
        root.Add(label);

        GroupBox groupBox = new GroupBox("Maze settings");
        groupBox.style.backgroundColor = Color.gray2;
        this.m_width = new IntegerField("Width");
        this.m_width.value = 20;
        this.m_height = new IntegerField("Height");
        this.m_height.value = 20;
        this.m_seed = new IntegerField("Seed");
        this.m_seed.value = 2;
        this.m_iterations = new IntegerField("Iterations");
        this.m_iterations.value = 1000;
        groupBox.Add(this.m_width);
        groupBox.Add(this.m_height);
        groupBox.Add(this.m_seed);
        groupBox.Add(this.m_iterations);

        Button b = new Button();
        b.text = "Create Maze";
        b.clicked += B_clicked; //same as creating a delegate

        root.Add(groupBox);
        root.Add(b);
    }

    private void B_clicked() //Create maze button clicked
    {
        Debug.Log("Clicked");
        GameObject o = new GameObject("Maze");
        this.m_maze = o.AddComponent<Maze>();
        Object ob = AssetDatabase.LoadAssetAtPath("Assets/Data/celldata.asset", typeof(CellData));
        this.m_maze.m_cellData = (CellData)ob;
        this.m_maze.GenerateMaze(
            this.m_width.value,
            this.m_height.value,
            this.m_seed.value,
            this.m_iterations.value);
    }
}
