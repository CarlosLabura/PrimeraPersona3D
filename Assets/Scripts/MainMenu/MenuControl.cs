using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [Header("Menu Data")]
    [SerializeField] private TextAsset LevelInfo;
    public float TransitionSpeed = 5f;
    public Vector3 ItemStartPosition;
    public Vector3 ItemStartOffset;

    [Header("Objects Needed")]
    [SerializeField] private RawImage Background;
    [SerializeField] private GameObject MenuPrefab;
    [SerializeField] private Transform MenuContainer;

    // Internal Variables
    public static string LevelPath = "Scenes/";
    private string[] Levels;
    private string LevelSelected;

    // Components Needed
    private void Start()
    {
        Levels = LevelInfo.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < Levels.Length; i++)
        {
            GameObject Object = Instantiate(MenuPrefab, MenuContainer, false);
            Object.GetComponent<RectTransform>().anchoredPosition3D = ItemStartPosition + ItemStartOffset * i;

            MainMenuItem MainMenuItem = Object.GetComponent<MainMenuItem>();
            MainMenuItem.LevelName = Levels[i];

            Levels[i] = LevelPath + Levels[i].Trim() + "/";
            MainMenuItem.LevelPath = Levels[i];

            MainMenuItem.Load();
        }
    }
    private void Update()
    {
        if (StartAnimation.Started)
        {

        }
    }
}
