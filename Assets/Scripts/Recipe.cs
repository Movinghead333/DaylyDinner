using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Recipe : IComparable<Recipe>
{
    public string name;
    public DateTime lastPrepared;

    public Recipe(string name, DateTime lastPrepared)
    {
        this.name = name;
        this.lastPrepared = lastPrepared;
    }

    public int CompareTo(Recipe other)
    {
        return this.lastPrepared.CompareTo(other.lastPrepared);
    }

    public string GetDate()
    {
        return lastPrepared.ToString("dd.MM.yyyy");
    }
}
