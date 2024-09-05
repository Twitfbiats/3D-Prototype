using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using Leguar.TotalJSON;
using UnityEngine;

public class GlobalObject : Singleton<GlobalObject>
{
    public Vector2 screenResolution;
    public Vector2 rawMouse;
    public Vector2 mouse;
    public Vector2 prevMouse = Vector2.zero;
    public Vector2 mouseMoved;
    public float mouseSpeed = 5f;
    public PlayerInputSystem playerInputSystem;
    public string entityDataPath;
    public List<EntityData> entityDatas;
    public string playerSkillDataPath;
    public BinarySearchTree<CustomMonoBehavior> customMonoBehaviorBinarySearchTree = new();
    CustomMonoBehaviorComparer customMonoBehaviorComparer = new();
    UtilObject utilObject = new UtilObject();
    public Coroutine nullCoroutine;
    private TransitionRule[,] lowerBodyTransitionRules;
    [SerializeField] private string lowerBodyTransitionRulePath;
    private void Awake() 
    {
        playerInputSystem = new PlayerInputSystem();
        entityDataPath = Application.dataPath + "/JsonData/EntityData.json";
        entityDatas = new List<EntityData>();
        string json = File.ReadAllText(GlobalObject.Instance.entityDataPath);
        entityDatas = FromJson<EntityData>(json);
        playerSkillDataPath = Application.dataPath + "/JsonData/PlayerSkillData.json";
        nullCoroutine = StartCoroutine(Null());
        lowerBodyTransitionRulePath = Application.dataPath + "/ExcelData/LowerBodyTransitionRule.xlsx";
        LoadTransitionRule();
    }

    public void UpdateCustomonoBehaviorHealth(float value, GameObject gameObject)
    {
        customMonoBehaviorBinarySearchTree.Search((CustomMonoBehavior customMonoBehavior) => {return customMonoBehavior.gameObject.GetInstanceID();}, gameObject.GetInstanceID())?.UpdateHealth(value);
        
        // var searched = utilObject.CombatEntityInfoBinarySearch(combatEntityInfos, gameObject.GetInstanceID()).CombatEntity;
        // Debug.Log(searched.CurentHealth + "-" + searched.gameObject.name);
        // searched.UpdateHealth(10);
    }

    public CustomMonoBehavior GetCustomMonoBehavior(GameObject gameObject)
    {
        return customMonoBehaviorBinarySearchTree.Search((CustomMonoBehavior customMonoBehavior) => {return customMonoBehavior.gameObject.GetInstanceID();}, gameObject.GetInstanceID());
    }

    // Start is called before the first frame update
    void Start()
    {
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        
        #region search all custom monobehavior

        FindObjectsByType<CustomMonoBehavior>(FindObjectsSortMode.InstanceID).ToList().ForEach((customMonoBehavior) => 
        {
            customMonoBehaviorBinarySearchTree.Insert(customMonoBehavior);
        });

        // sort the list by object instance id
        // combatEntityInfos.Sort(combatEntityInfoComparer);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        rawMouse = playerInputSystem.Control.View.ReadValue<Vector2>();
        mouse = new Vector2(rawMouse.x/screenResolution.x, rawMouse.y/screenResolution.y);
        mouseMoved = mouse - prevMouse; mouseMoved *= mouseSpeed;    
    }

    public List<T> FromJson<T> (string json)
    {
        JSON JsonObject = JSON.ParseString(json);
        Wrapper<T> wrapper = JsonObject.Deserialize<Wrapper<T>>();
        JsonObject.Deserialize<Wrapper<T>>();
        return wrapper.items;
    }

    public List<T> FromJson<T> (string path, bool use_path)
    {
        string json = File.ReadAllText(path);
        JSON JsonObject = JSON.ParseString(json);
        Wrapper<T> wrapper = JsonObject.Deserialize<Wrapper<T>>();
        JsonObject.Deserialize<Wrapper<T>>();
        return wrapper.items;
    }


    public IEnumerator Null()
    {
        yield return new WaitForSeconds(0);
    }

    public void LoadTransitionRule()
    {
        if (!File.Exists(lowerBodyTransitionRulePath))
        {
            print("LowerBodyTransitionRule file not found!");
            return;
        }

        try
        {
            using (var stream = File.Open(lowerBodyTransitionRulePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[0];

                // Prepare a 2D array to hold the TransitionRule objects
                lowerBodyTransitionRules = new TransitionRule[dataTable.Rows.Count - 1, dataTable.Columns.Count - 1];

                // Extract column and row names
                var columnHeaders = dataTable.Rows[0].ItemArray;
                var rows = dataTable.AsEnumerable().Skip(1).ToArray();

                int rowIndex = 0;
                foreach (var row in rows)
                {
                    var rowValues = row.ItemArray;
                    int columnIndex = 0;
                    foreach (var cell in rowValues.Skip(1)) // Skip the first cell (row header)
                    {
                        var cellValue = cell?.ToString(); // Handle null values
                        if (string.IsNullOrEmpty(cellValue))
                        {
                            print($"Warning: Empty cell found at row {rowIndex + 1}, column {columnIndex + 1}");
                            continue;
                        }

                        var values = cellValue.Split(','); // Split the cell value by comma
                        if (values.Length != 2)
                        {
                            print($"Warning: Unexpected cell format at row {rowIndex + 1}, column {columnIndex + 1}");
                            continue;
                        }

                        var onAirOutcome = values[0].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                        var onGroundOutcome = values[1].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);

                        lowerBodyTransitionRules[rowIndex, columnIndex] = new TransitionRule(onAirOutcome, onGroundOutcome);
                        columnIndex++;
                    }
                    rowIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            print($"An error occurred: {ex.Message}");
        }
        
    }

    void OnEnable()
    {
        playerInputSystem.Control.Enable();
    }

    void OnDisable()
    {
        playerInputSystem.Control.Disable();
    }
}

public class Wrapper<T>
{
    public List<T> items;
}

public class CustomMonoBehaviorComparer : IComparer<CustomMonoBehavior>
{
    public int Compare(CustomMonoBehavior x, CustomMonoBehavior y)
    {
        if (x == null || y == null) 
        {
            return 0;
        }
        return x.gameObject.GetInstanceID().CompareTo(y.gameObject.GetInstanceID());
    }
}



