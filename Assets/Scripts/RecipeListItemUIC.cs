using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeListItemUIC : MonoBehaviour
{
    public TextMeshProUGUI recipeNameTextMesh;
    public TextMeshProUGUI lastPreparedTextMesh;
    public int id;

    public void SetupListItem(string recipeName, string lastPrepared, int id)
    {
        recipeNameTextMesh.text = recipeName;
        lastPreparedTextMesh.text = lastPrepared;
        this.id = id;
    }

    public void OnListItemClicked()
    {
        AppController.instance.OnRecipeListItemSelected(id);
    }
}
