﻿using UnityEngine;
using UnityEditor;

public class CustomShaderGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);
        //foreach (MaterialProperty property in properties)
        //    materialEditor.ShaderProperty(property, property.displayName);
    }
}
