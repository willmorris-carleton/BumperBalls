using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Player player = (Player) target;
        if (DrawDefaultInspector ()) {
            //player.InitializeBallColour();
        }
    }
}
