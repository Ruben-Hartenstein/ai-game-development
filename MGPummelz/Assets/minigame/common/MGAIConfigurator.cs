using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGAIConfigurator : MonoBehaviour
{

    public Dropdown player0Dropdown;
    public Dropdown player1Dropdown;

    public MGMinigame miniGame;

    public MGAIConfigurable aiConfigurable;

    public Button resetButton;

    private List<string> aiTypes;

    void Start()
    {
        aiConfigurable = (MGAIConfigurable)miniGame;

        updateDropdowns();

    }

    private void updateDropdowns()
    {
        player0Dropdown.ClearOptions();
        player1Dropdown.ClearOptions();

        aiTypes = aiConfigurable.getAITypes();
        player0Dropdown.AddOptions(aiTypes);
        player1Dropdown.AddOptions(aiTypes);
    }

    private void updateAI()
    {
        aiConfigurable.setAIType(0, aiTypes[player0Dropdown.value]);
        aiConfigurable.setAIType(1, aiTypes[player1Dropdown.value]);
    }

    public void startPressed()
    {
        updateAI();
        miniGame.startGame();
    }

    public void resetPressed()
    {
        updateAI();
        miniGame.reset();
    }
}
