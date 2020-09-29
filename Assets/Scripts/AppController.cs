using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public class AppController : MonoBehaviour
{
    #region Singleton
    public static AppController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    #endregion

    #region Public_Members
    public GameObject addRecipeDialog;
    public GameObject cookRecipeDialog;

    public static DateTime neverMade = new DateTime(2000, 1, 1);
    #endregion

    #region Private_Members
    private List<Recipe> recipes = new List<Recipe>();

    private string dataPath;

    private int selectedListItemID = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // setup up dataPath for loading and storing recipes
        dataPath = Application.persistentDataPath + "/data.txt";

        // write dummy recipe data to data.txt
        WriteDummyData();

        
        LoadRecipes();

        Rerender();
    }

    #region UI_Event_Callbacks
    /* gets called when the "+" button is pressed and shows a the dialog to add
       add a new recipe */
    public void OnAddNewRecipe()
    {
        addRecipeDialog.SetActive(true);
    }

    public void OnNewRecipeConfirmed(Recipe newRecipe)
    {
        addRecipeDialog.SetActive(false);
        recipes.Add(newRecipe);
        recipes.Sort();
        Rerender();
        SaveRecipes();
    }

    public void OnNewRecipeCanceled()
    {
        addRecipeDialog.SetActive(false);
    }

    public void OnCookRecipeConfirmed()
    {
        Debug.Log("recipe cooked");
        recipes[selectedListItemID].lastPrepared = DateTime.Today;
        SaveRecipes();
        Rerender();
        Debug.Log(recipes[selectedListItemID].GetDate());
        cookRecipeDialog.SetActive(false);
    }

    public void OnCookRecipeCanceled()
    {
        cookRecipeDialog.SetActive(false);
    }

    public void OnRecipeListItemSelected(int id)
    {
        cookRecipeDialog.SetActive(true);
        selectedListItemID = id;
        string title = $"Wurde heute das Gericht \"{recipes[id].name}\" gekocht?";
        CookRecipeDialogUIC.instance.SetDialogTitle(title);
    }

    public void OnEnterDeletionMode()
    {

    }
    #endregion

    private void Rerender()
    {
        RecipeListUIC.instance.RerenderList(recipes);
    }

    #region Data_Loading_And_Saving
    /* Load recipes from data.txt and sort them */
    private void LoadRecipes()
    {
        try
        {
            string[] lines = File.ReadAllLines(dataPath);

            for (int i = 0; i < lines.Length / 2; i++)
            {
                int index = i * 2;
                string recipeName = lines[index];
                Debug.Log($"Am {lines[index + 1]} gab es {recipeName}");

                DateTime lastPrepared = DateTime.ParseExact(
                    lines[index + 1], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                recipes.Add(new Recipe(recipeName, lastPrepared));
            }

            recipes.Sort();
        }
        catch (Exception e)
        {
            Debug.LogError("Rezepte konnten nicht geladen werden!");
            Debug.LogError(e.Message);
        }
    }

    /* store all current recipes to data.txt in dataPath */
    private void SaveRecipes()
    {
        recipes.Sort();

        string[] lines = new string[recipes.Count * 2];

        for (int i = 0; i < recipes.Count; i++)
        {
            lines[i * 2] = recipes[i].name;
            lines[i * 2 + 1] = recipes[i].GetDate();
        }

        try
        {
            File.WriteAllLines(dataPath, lines);
        }
        catch (Exception e)
        {
            Debug.LogError("Rezepte konnten nicht gespeichert werden.");
            Debug.LogError(e.Message);
        }
    }

    /* Write dummy recipe data to data.txt in dataPath */
    private void WriteDummyData()
    {
        try
        {
            string[] lines = {
                "Zuccini-creme-suppe", "29.09.2020",
                "Paprika Geschnetzeltes", "29.09.2020",
                "Rippchen", "28.09.2020",
                "Ei mit Schinken", "28.09.2020"};

            File.WriteAllLines(dataPath, lines);
        }
        catch (Exception e)
        {
            Debug.LogError("Rezepte konnten nicht gespeichert werden.");
            Debug.LogError(e.Message);
        }
    }
    #endregion
}
