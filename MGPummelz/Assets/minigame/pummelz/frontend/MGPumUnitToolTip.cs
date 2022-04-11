using rccg.frontend;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace mg.pummelz
{
    public class MGPumUnitToolTip : MonoBehaviour, IPointerClickHandler
    {
         
        public Image illustration;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI abilityText;

        public TextMeshProUGUI healthText;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI powerText;
        public TextMeshProUGUI rangeText;

        public Transform boundsTransform;

        internal MGPumUnitRender unitRender;
        internal GameObject positionObject;



        public void OnPointerClick(PointerEventData eventData)
        {
            hide();
        }

        public void show(MGPumUnitRender unitRender, GameObject positionObject)
        {
            this.positionObject = positionObject;
            this.unitRender = unitRender;
            updateToolTip();
            gameObject.SetActive(true);
        }

        public void hide()
        {
            gameObject.SetActive(false);
        }

        public void updateToolTip()
        {
            MGPumUnit unit = unitRender.unit;

            //fallback to small illu if large is still missing

            Sprite[] sprites = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/large/" + MGPummelz.getColorForPlayer(unit.ownerID).ToLower() + "/" + unit.artID);
            if (sprites == null || sprites.Length == 0)
            {
                illustration.sprite = unitRender.illustration.sprite;
            }
            else
            {
                this.illustration.sprite = sprites[0];
            }

            nameText.text = unit.name;

            if(unit.abilityCurrent != null)
            {
                abilityText.text = unit.abilityCurrent.toI18nedString();
            }
            else
            {
                abilityText.text = "";
            }

            healthText.text = unit.currentHealth.ToString();
            speedText.text = unit.currentSpeed.ToString();
            powerText.text = unit.currentPower.ToString();
            rangeText.text = unit.currentRange.ToString();
        }

        public void Update()
        {
            if(positionObject != null && !positionObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }

   

    
}