using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData")]
public class RelicData : ScriptableObject
{
    public string relicKind;
    public Sprite sprite;
    public Sprite relicIcon;
}
