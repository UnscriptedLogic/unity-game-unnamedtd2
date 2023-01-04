using System;
using UnityEngine;

public interface IUsesHealthBar
{
    void InitHealthBar(out float current, out float max, Action<float> update);
}
