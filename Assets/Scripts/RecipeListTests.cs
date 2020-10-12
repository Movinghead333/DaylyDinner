using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeListTests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Recipe> recip = new List<Recipe>();
        recip.Add(new Recipe("dank", DateTime.Today));
        Recipe r = recip[0];
        r.name = "changed";
        Debug.Log("Listitem is: " + recip[0]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

    }
}
