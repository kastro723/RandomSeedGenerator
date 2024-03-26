using UnityEditor;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class RandomSeedGenerator : EditorWindow
{
    private long seed = 0; // 0 ~ 9223372036854775807 (long Ÿ���� ��� ����)
    private int minNumber = 0;
    private int maxNumber = 0;
    private int iterations = 0;
    private List<int> results = new List<int>();

    // LCG Parameters
    private const long a = 1103515245;
    private const long c = 12345;
    private const long m = 2147483647;
    

    private bool showResults = true;
    private Vector2 scrollPosition;


    private int currentPage = 1;
    private int itemsPerPage = 10; // ������ �� �׸� ��
    private int totalPage = 1;

    private GUIStyle centeredStyle;

    [MenuItem("Tools/Random Seed Generator")]
    public static void ShowWindow()
    {
        var window = GetWindow<RandomSeedGenerator>("Random Seed Generator");
        window.maxSize = new Vector2(window.maxSize.x, window.maxSize.y);
        window.minSize = new Vector2(300, 500);
        window.LoadNumberValues();
    }

 

    void OnGUI()
    {
        if (centeredStyle == null)
        {
            centeredStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        }

        GUILayout.Label("Ver. 1.1.0", EditorStyles.boldLabel);
        DrawLine();

        seed = EditorGUILayout.LongField("Seed", seed);
        minNumber = EditorGUILayout.IntField("Min Number", minNumber);
        maxNumber = EditorGUILayout.IntField("Max Number", maxNumber);
        iterations = EditorGUILayout.IntField("Iterations", iterations);

        // ������ �� �׸� ���� ����
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("10")) UpdateItemsPerPage(10);
        if (GUILayout.Button("30")) UpdateItemsPerPage(30);
        if (GUILayout.Button("50")) UpdateItemsPerPage(50);
        if (GUILayout.Button("100")) UpdateItemsPerPage(100);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Generate") && iterations != 0)
        {
            GenerateRandomNumbers();
            CalculateTotalPage();
            currentPage = 1; // ������� ù �������� ����
            showResults = true;
            SaveNumberValues();
        }

        if (GUILayout.Button("Clear"))
        {
            showResults = false;
            results.Clear();
            currentPage = 1;
            totalPage = 1;
        }

        // ������ ����
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<") && currentPage > 1)
        {
            currentPage--;
        }
        GUILayout.Label($"Page {currentPage} of {totalPage}", centeredStyle);
        if (GUILayout.Button(">") && currentPage < totalPage)
        {
            currentPage++;
        }
        GUILayout.EndHorizontal();

        DrawLine();

        if (showResults)
        {
            DisplayPageResults();
        }
    }

    void GenerateRandomNumbers()
    {
        long originalSeed = seed; // �ʱ� seed ���� �ӽ� ������ ����
        results.Clear();
        for (int i = 0; i < iterations; i++)
        {
            seed = (a * seed + c) % m; // ���� seed�� ����Ͽ� ���� ����
            int randomNumber = minNumber + (int)(seed % (maxNumber - minNumber + 1));
            results.Add(randomNumber);
        }
        seed = originalSeed; // ���� ���� �� ���� seed ������ ����
    }


    void CalculateTotalPage()
    {
        totalPage = (iterations + itemsPerPage - 1) / itemsPerPage; // �� ������ �� ���
    }

    void DisplayPageResults()
    {
        int start = (currentPage - 1) * itemsPerPage;
        int end = Mathf.Min(start + itemsPerPage, results.Count);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = start; i < end; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{i + 1}", centeredStyle, GUILayout.Width(50));
            GUILayout.Space(20);
            GUILayout.Label($"{results[i]}", centeredStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    // Number�� EditorPrefs�� ����
    void SaveNumberValues()
    {
        EditorPrefs.SetInt("MinNumber", minNumber);
        EditorPrefs.SetInt("MaxNumber", maxNumber);
        EditorPrefs.SetInt("Iterations", iterations);
        EditorPrefs.SetString("Seed", seed.ToString());
    }

    // Number�� EditorPrefs���� �ҷ�����
    void LoadNumberValues()
    {
        minNumber = EditorPrefs.GetInt("MinNumber", 0);
        maxNumber = EditorPrefs.GetInt("MaxNumber", 0);
        iterations = EditorPrefs.GetInt("Iterations", 0);
        long.TryParse(EditorPrefs.GetString("Seed", "0"), out seed);
    }

    void UpdateItemsPerPage(int newItemsPerPage) // ������ �� �׸� ���� ���� page ���
    {
        if (newItemsPerPage != itemsPerPage && results.Count != 0 && iterations != 0)
        {
            int firstItemIndex = (currentPage - 1) * itemsPerPage;
            itemsPerPage = newItemsPerPage;
            currentPage = (firstItemIndex / itemsPerPage) + 1;
            CalculateTotalPage(); 
        }
    }


    private void DrawLine()
    {
        var rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, Color.gray);
    }

}
