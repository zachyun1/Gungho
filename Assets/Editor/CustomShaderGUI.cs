using UnityEngine;
using UnityEditor;
using System;

public class CustomShaderGUI : ShaderGUI
{
    Material targetMat;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);


        targetMat = materialEditor.target as Material;
        //foreach (MaterialProperty property in properties)
        //    materialEditor.ShaderProperty(property, property.displayName);

        MaterialProperty _ToggleOutlinePass = ShaderGUI.FindProperty("_ToggleOutlinePass", properties);
        //MaterialProperty _BasePass = ShaderGUI.FindProperty("_BasePass", properties);

        if(_ToggleOutlinePass.floatValue == 1)
            Toggle_ShaderPass("Always", false);
        else
            Toggle_ShaderPass("Always", true);

        //if (_BasePass.floatValue == 1)
        //    Toggle_ShaderPass("ForwardBase", false);
        //else
        //    Toggle_ShaderPass("ForwardBase", true);


    }

    void Toggle_ShaderPass(string name, bool state)
    {
        bool IsPassEnabled = targetMat.GetShaderPassEnabled(name);
        if (IsPassEnabled != state)
            targetMat.SetShaderPassEnabled(name, state);
    }
}
