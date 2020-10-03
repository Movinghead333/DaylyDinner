using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public enum ApplicationState
{
    RECIPE_LIST_VIEW,
    ADD_NEW_RECIPE_DIALOG,
    SHOW_RECIPE_DIALOG,
}

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
    public GameObject showRecipeDialog;
    public GameObject addRecipeButton;

    public static DateTime neverMade = new DateTime(2000, 1, 1);
    #endregion

    #region Private_Members
    private List<Recipe> recipes = new List<Recipe>();

    private string dataPath;

    private int selectedListItemID = 0;

    private ApplicationState applicationState =
        ApplicationState.RECIPE_LIST_VIEW;
    #endregion

    // Start is called before the first frame update
    public void Start()
    {
        // setup up dataPath for loading and storing recipes
        dataPath = Application.persistentDataPath + "/data.txt";

        // write dummy recipe data to data.txt
        //WriteDummyData();

        // laod existing recipes
        LoadRecipes();

        // rerender loaded recipes
        Rerender();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (applicationState)
            {
                case ApplicationState.RECIPE_LIST_VIEW:
                    // minimize the app when pressing back on main screen
#if UNITY_ANDROID && !UNITY_EDITOR
                    //AndroidJavaObject mainActivity =
                    //    new AndroidJavaClass("com.unity3d.player.UnityPlayer").
                    //        GetStatic<AndroidJavaObject>("currentActivity");
                    //mainActivity.Call<bool>("moveTaskToBack", true);
#endif

                    Application.Quit(); // do not exit the application completely
                    break;
                case ApplicationState.ADD_NEW_RECIPE_DIALOG:
                    ChangeState(ApplicationState.RECIPE_LIST_VIEW);
                    break;
                case ApplicationState.SHOW_RECIPE_DIALOG:
                    ChangeState(ApplicationState.RECIPE_LIST_VIEW);
                    break;
            }

            // if there has been an open Menu it is now closed so we can set
            // the UI state to NO_MENU and show the Main Menu again
            //if (currentUIState != UIState.NO_MENU)
            //{
            //    currentUIState = UIState.NO_MENU;
            //    onScreenMenuButtons.SetActive(true);
            //}
        }
    }

    #region UI_Event_Callbacks
    /* gets called when the "+" button is pressed and shows a the dialog to add
       add a new recipe */
    public void OnAddNewRecipe()
    {
        ChangeState(ApplicationState.ADD_NEW_RECIPE_DIALOG);
    }

    public void OnNewRecipeConfirmed(Recipe newRecipe)
    {
        if (!ChangeState(ApplicationState.RECIPE_LIST_VIEW)) return;
        recipes.Add(newRecipe);
        recipes.Sort();
        Rerender();
        SaveRecipes();
    }

    public void OnNewRecipeCanceled()
    {
        ChangeState(ApplicationState.RECIPE_LIST_VIEW);
    }

    public void OnRecipeListItemSelected(int id)
    {
        if (!ChangeState(ApplicationState.SHOW_RECIPE_DIALOG)) return;
        selectedListItemID = id;
        string recipeName = recipes[selectedListItemID].name;
        ViewRecipeDialogUIC.instance.ChangeRecipeName(recipeName);
    }

    public void OnCookRecipeConfirmed()
    {
        ChangeState(ApplicationState.RECIPE_LIST_VIEW);
        Debug.Log("recipe cooked");
        recipes[selectedListItemID].lastPrepared = DateTime.Today;
        SaveRecipes();
        Rerender();
        Debug.Log(recipes[selectedListItemID].GetDate());
    }

    public void OnDeleteRecipeConfirmed()
    {
        ChangeState(ApplicationState.RECIPE_LIST_VIEW);
        recipes.RemoveAt(selectedListItemID);
        SaveRecipes();
        Rerender();
    }

    public void OnShowRecipeDialogClosed()
    {
        ChangeState(ApplicationState.RECIPE_LIST_VIEW);
    }
    #endregion

    private void Rerender()
    {
        RecipeListUIC.instance.RerenderList(recipes);
    }

    private bool ChangeState(ApplicationState newState)
    {
        bool changeState = false;
        switch (newState)
        {
            case ApplicationState.RECIPE_LIST_VIEW:
                addRecipeDialog.SetActive(false);
                showRecipeDialog.SetActive(false);
                addRecipeButton.SetActive(true);
                changeState = true;
                break;
            case ApplicationState.ADD_NEW_RECIPE_DIALOG:
                if (applicationState == ApplicationState.RECIPE_LIST_VIEW)
                {
                    addRecipeDialog.SetActive(true);
                    addRecipeButton.SetActive(false);
                    changeState = true;
                }
                break;
            case ApplicationState.SHOW_RECIPE_DIALOG:
                if (applicationState == ApplicationState.RECIPE_LIST_VIEW)
                {
                    showRecipeDialog.SetActive(true);
                    addRecipeButton.SetActive(false);
                    changeState = true;
                }
                break;
        }
        if (changeState)
        {
            applicationState = newState;
        }
        return changeState;
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
                "Bratwurst", "29.09.2020",
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
