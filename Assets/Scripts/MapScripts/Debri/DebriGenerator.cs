using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public struct DebriGenSettings
{
    public int amount;
    public float verticalOffset;
    public DebriThemeSO debriTheme;
    public List<Vector3> pathlist;
    public List<Vector3> nodes;
    public Transform parent;

    public DebriGenSettings(int amount, float verticalOffset, DebriThemeSO debriTheme, List<Vector3> pathlist, List<Vector3> nodes, Transform parent)
    {
        this.amount = amount;
        this.verticalOffset = verticalOffset;
        this.debriTheme = debriTheme;
        this.pathlist = pathlist;
        this.nodes = nodes;
        this.parent = parent;
    }
}

public static class DebriGenerator
{
    public static void GenerateDebrisRandomly(DebriGenSettings settings)
    {
        for (int i = 0; i < settings.amount; i++)
        {
            Debri debri = RandomLogic.FromList(settings.debriTheme.Debris);
            Vector3 position = RandomLogic.FromList(settings.nodes);

            if (IsClearOfPath(settings.pathlist, position, debri.size))
            {
                GameObject debriObject = Object.Instantiate(debri.prefab, position, Quaternion.identity, settings.parent);
                debriObject.transform.forward = RandomLogic.VectorDirAroundY();
            }
        }
    }

    public static bool IsClearOfPath(List<Vector3> pathlist, Vector3 position, float radius)
    {
        for (int i = 0; i < pathlist.Count; i++)
        {
            if (Vector3.Distance(position, pathlist[i]) <= radius)
            {
                return false;
            }
        }

        return true;
    }
}