using rccg.frontend;
using UnityEngine;
using UnityEngine.UI;

namespace mg.pummelz
{
    public class MGPumEndTurnButton : MonoBehaviour
    {
        public MGPumInputManager inputManager;
        public Image buttonImage;
        public Button button;

        internal int playerID;

        public void switchTurn(int playerID, bool interactable)
        {
            this.playerID = playerID;
            this.button.interactable = interactable;
            updateButtonImage();
        }

        public void updateButtonImage()
        {
            this.buttonImage.sprite = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/turnbutton")[this.playerID * 2 + (this.button.interactable ? 0 : 1)];
        }

   

        public void buttonclicked()
        {
            inputManager.endTurnButtonclicked();
        }
    }

   

    
}