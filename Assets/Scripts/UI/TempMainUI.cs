using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempMainUI : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private string changesTitle;
    [TextArea(5, 5)] [SerializeField] private string comments;

    [SerializeField] private List<string> newChanges;
    [SerializeField] private List<string> fixedChanges;
    [SerializeField] private List<string> removedChanges;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI commentsTMP;
    [SerializeField] private TextMeshProUGUI changesTitleTMP;
    [SerializeField] private TextMeshProUGUI changesTMP;

    private void Start()
    {
        SetFields();
    }

    private void OnValidate()
    {
        SetFields();
    }

    private void SetFields()
    {
        string versionText = Application.version;

        commentsTMP.text = comments;
        changesTitleTMP.text = $"[{versionText}] {changesTitle}";

        changesTMP.text = "";
        for (int i = 0; i < newChanges.Count; i++)
        {
            if (i == 0)
            {
                changesTMP.text += Divider();
            }

            changesTMP.text += $"\n<color=#FFD700>[New] <color=white>{newChanges[i]}";
        }

        if (fixedChanges.Count > 0)
            changesTMP.text += Divider(before: "\n");

        for (int i = 0; i < fixedChanges.Count; i++)
        {
            changesTMP.text += $"\n<color=#ff5f00>[Fix] <color=white>{fixedChanges[i]}";
        }

        if (removedChanges.Count > 0)
            changesTMP.text += Divider(before: "\n");

        for (int i = 0; i < removedChanges.Count; i++)
        {
            changesTMP.text += $"\n<color=#df2f2f>[Removed] <color=white>{removedChanges[i]}";
        }
    }

    private string Divider(string before = "", string after = "") => $"{before}<color=#4f4f4f>------------------------------------{after}";
}
