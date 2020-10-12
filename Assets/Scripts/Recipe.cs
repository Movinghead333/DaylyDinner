using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Recipe : IComparable<Recipe>
{
    static int idCounter = 0;
    public string name;
    public DateTime lastPrepared;
    public int uniqueID;

    public Recipe(string name, DateTime lastPrepared)
    {
        this.name = name;
        this.lastPrepared = lastPrepared;
        this.uniqueID = idCounter++;
        Debug.Log("constructed");
    }

    public int CompareTo(Recipe other)
    {
        return this.lastPrepared.CompareTo(other.lastPrepared);
    }

    public string GetDate()
    {
        return lastPrepared.ToString("dd.MM.yyyy");
    }

    public override string ToString()
    {
        return "Recipename: " + name + " Last prepared: " + GetDate();
    }
}
