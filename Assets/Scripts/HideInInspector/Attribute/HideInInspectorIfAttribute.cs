using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public class HideInInspectorIf : BaseHideInInspectorIf
{
    public HideInInspectorIf(string conditionName) : base(conditionName, false) {}
}