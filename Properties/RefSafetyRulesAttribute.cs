using System;

internal class RefSafetyRulesAttribute : Attribute
{
    private int v;

    public RefSafetyRulesAttribute(int v)
    {
        this.v = v;
    }
}