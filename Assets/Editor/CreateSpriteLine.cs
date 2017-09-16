using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateSpriteLine : ScriptableWizard {

    public Sprite spriteToDraw;
    public GameObject startPoint;

	[MenuItem("Tools/Create Sprite Line")]
    static void CreateSpriteLineWizard()
    {
        ScriptableWizard.DisplayWizard<CreateSpriteLine>("Create Sprite Line", "Create");
    }

    void OnWizardCreate()
    {
        
    }

}
