using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInformation : SingletonMono<SceneInformation>
{
    public SceneLoadingFlags sceneFlags;
}

[System.Flags]
public enum SceneLoadingFlags
{
    RequireDungeon = 1 << 0,
    RequireNavMesh = 1 << 1,
}

