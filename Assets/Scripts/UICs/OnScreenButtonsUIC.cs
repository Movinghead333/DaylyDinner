using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenButtonsUIC : MonoBehaviour
{
    public void OnAddNewRecipeButtonClicked()
    {
        AppController.instance.OnAddNewRecipe();
    }

    public void OnEditRecipesButtonClicked()
    {

    }
}
