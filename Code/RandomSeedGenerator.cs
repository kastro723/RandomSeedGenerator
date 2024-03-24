using UnityEditor;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class RandomSeedGenerator : EditorWindow
{
    private int minNumber = 0;
    private int maxNumber = 0;
    private int iterations = 0;
    private List<int> results = new List<int>();

    // LCG Parameters
    private const long a = 1103515245;
    private const long c = 12345;
    private const long m = 2147483647;
    private long seed = 123456789; // Initial seed

    private bool showResults = true;

    private Vector2 scrollPosition;



    [MenuItem("Tools/Random Seed Generator")]
    public static void ShowWindow()
    {
        var window = GetWindow<RandomSeedGenerator>("Random Seed Generator");
        window.maxSize = new Vector2(window.maxSize.x, window.maxSize.y);
        window.minSize = new Vector2(300, 400);
        window.LoadNumberValues();
    }

    void OnGUI()
    {
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Ver. 1.0.0", EditorStyles.boldLabel);

        DrawLine();

        minNumber = EditorGUILayout.IntField("Min Number", minNumber);
        maxNumber = EditorGUILayout.IntField("Max Number", maxNumber);
        iterations = EditorGUILayout.IntSlider("Iterations", iterations, 0, 99);

        if (GUILayout.Button("Generate"))
        {
            GenerateRandomNumbers();
            showResults = true;
            SaveNumberValues();
        }

        if (GUILayout.Button("Clear"))
        {
            showResults = false;
        }

        DrawLine();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Iteration", centeredStyle, GUILayout.ExpandWidth(false));

        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUILayout.Space(10); // 레이블과 세로선 사이의 공간

        GUILayout.Label("Seed", centeredStyle, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        DrawVerticalLine(lastRect.xMax + 5, lastRect.y); // 세로 선 

        DrawLine();

        if (showResults)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < results.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{i + 1}", centeredStyle, GUILayout.Width(50));
                GUILayout.Space(20);
                GUILayout.Label($"{results[i]}", centeredStyle, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
    }

    void GenerateRandomNumbers()
    {
        int currentCount = results.Count;
        if (iterations > currentCount)
        {
            // iterations 값이 현재 결과보다 크다면, 추가로 생성
            int additionalIterations = iterations - currentCount;
            for (int i = 0; i < additionalIterations; i++)
            {
                seed = (a * seed + c) % m;
                int randomNumber = minNumber + (int)(seed % (maxNumber - minNumber + 1));
                results.Add(randomNumber);
            }
        }
        else
        {
            // iterations 값이 현재 결과보다 작거나 같다면, 리스트를 잘라서 사용
            results = results.GetRange(0, iterations);
        }
    }

    // Number값 EditorPrefs에 저장
    void SaveNumberValues()
    {
        EditorPrefs.SetInt("MinNumber", minNumber);
        EditorPrefs.SetInt("MaxNumber", maxNumber);
        EditorPrefs.SetInt("iterations", iterations);
    }

    // Number값 EditorPrefs에서 불러오기
    void LoadNumberValues()
    {
        minNumber = EditorPrefs.GetInt("MinNumber", 0);
        maxNumber = EditorPrefs.GetInt("MaxNumber", 0);
        iterations = EditorPrefs.GetInt("iterations", 0);
    }

    private void DrawLine()
    {
        var rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, Color.gray);
    }

    private void DrawVerticalLine(float xPosition, float yPosition)
    {
        EditorGUI.DrawRect(new Rect(xPosition, yPosition, 1, this.position.height), Color.gray);
    }
}