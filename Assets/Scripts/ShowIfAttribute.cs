using UnityEngine;
using System;
public class ShowIfAttribute : PropertyAttribute
{
    public string variableName;
    public Type componentType;
    public bool inverse;
    public ShowIfAttribute(string variableName, bool inverse = false)
    {
        this.variableName = variableName;
        this.inverse = inverse;
    }
    public ShowIfAttribute(Type componentType, string variableName, bool inverse = false)
    {
        this.componentType = componentType;
        this.variableName = variableName;
        this.inverse = inverse;
    }
    // i forgor where i took this from
}