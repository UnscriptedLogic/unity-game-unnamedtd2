using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenUI : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private string changesTitle;
    [TextArea(5, 5)] [SerializeField] private string comments;

    [SerializeField] private List<string> newChanges;
    [SerializeField] private List<string> balanceChanges;
    [SerializeField] private List<string> fixedChanges;
    [SerializeField] private List<string> removedChanges;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI commentsTMP;
    [SerializeField] private TextMeshProUGUI changesTitleTMP;
    [SerializeField] private TextMeshProUGUI changesTMP;

    [Header("Colors")]
    [SerializeField] private Color updatesColor;
    [SerializeField] private Color commentsColor;
    [SerializeField] private Color balancePrefixColor;
    [SerializeField] private Color abilityMentionColor;

    [Header("Others")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        playBtn.onClick.AddListener(() => SceneController.instance.LoadScene(SceneIndexes.LEVEL1, MapIndexes.RUINS));
        quitBtn.onClick.AddListener(() => SceneController.instance.QuitGame());
    }

    private void OnValidate()
    {
        SetFields();
    }

    private void SetFields()
    {
        string versionText = Application.version;

        commentsTMP.text = "";
        changesTMP.text = "";

        commentsTMP.text += $"<color={ConvertHex(commentsColor)}>{comments}";
        commentsTMP.text += ThickDivider("\n");

        changesTitleTMP.text = $"[{versionText}] {changesTitle}";
        for (int i = 0; i < newChanges.Count; i++)
        {
            if (i == 0)
            {
                //changesTMP.text += Divider();
                changesTMP.text += $"<color=#FFD700>[New] <color={ConvertHex(updatesColor)}>{newChanges[i]}";
                continue;
            }

            changesTMP.text += $"\n<color=#FFD700>[New] <color={ConvertHex(updatesColor)}>{newChanges[i]}";
        }

        if (balanceChanges.Count > 0)
            changesTMP.text += Divider(before: "\n");

        for (int i = 0; i < balanceChanges.Count; i++)
        {
            changesTMP.text += $"\n<color={ConvertHex(balancePrefixColor)}>[Balance] <color={ConvertHex(updatesColor)}>{balanceChanges[i]}";
        }

        if (fixedChanges.Count > 0)
            changesTMP.text += Divider(before: "\n");

        for (int i = 0; i < fixedChanges.Count; i++)
        {
            changesTMP.text += $"\n<color=#ff9f00>[Fix] <color={ConvertHex(updatesColor)}>{fixedChanges[i]}";
        }

        if (removedChanges.Count > 0)
            changesTMP.text += Divider(before: "\n");

        for (int i = 0; i < removedChanges.Count; i++)
        {
            changesTMP.text += $"\n<color=#df2f2f>[Removed] <color={ConvertHex(updatesColor)}>{removedChanges[i]}";
        }
    }

    private string Divider(string before = "", string after = "") => $"{before}<color=#4f4f4f>------------------------------------------{after}";
    private string ThickDivider(string before = "", string after = "") => $"{before}<color=#4f4f4f>====================================={after}";

    private string ConvertHex(Color color) => $"#{ColorUtility.ToHtmlStringRGBA(color)}";
}
