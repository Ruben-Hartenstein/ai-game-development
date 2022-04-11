using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGLevelConfigurator : MonoBehaviour
{

    public Dropdown levelDropdown;

    public MGMinigame miniGame;

    internal MGLevelConfigurable levelConfigurable;


    private List<string> levelTypes;

    void Start()
    {
        levelConfigurable = (MGLevelConfigurable)miniGame;

        updateDropdowns();

    }

    private void updateDropdowns()
    {
        levelDropdown.ClearOptions();

        levelTypes = levelConfigurable.getLevels();
        levelDropdown.AddOptions(levelTypes);
    }

    public void resetPressed()
    {
        levelConfigurable.setLevel(levelTypes[levelDropdown.value]);
        miniGame.reset();
    }
}
