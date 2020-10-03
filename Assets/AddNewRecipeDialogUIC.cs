using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AddNewRecipeDialogUIC : MonoBehaviour
{
    public TMP_InputField recipeNameInput;

    public void OnDialogConfirmedButtonClicked()
    {
        string recipeName = recipeNameInput.text;

        if (recipeName == "") return;

        Recipe recipe = new Recipe(recipeName, new DateTime(2000,1,1));

        recipeNameInput.text = "";

        AppController.instance.OnNewRecipeConfirmed(recipe);
    }

    public void OnDialogCanceledButtonClicked()
    {
        AppController.instance.OnNewRecipeCanceled();
    }
}
