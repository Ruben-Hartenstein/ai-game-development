using rccg.frontend;
using RelegatiaCCG.rccg.frontend.animations;
using UnityEngine;
using UnityEngine.UI;

namespace mg.pummelz
{
    public class MGPumUnitRender : MonoBehaviour
    {

        public Image illustration;
        private string illustrationName;

        public Image highlightLeft;
        public Image highlightRight;

        public ImageAnimator imageAnimator;
        public DissolveAnimator dissolveAnimator;
        public MGPumBarRender healthBar;

        public Transform origin = null;
        public Transform placeholderParent = null;

        private bool _showHealthbarAnim;
        internal bool showHealthbarAnim { get {return _showHealthbarAnim;} set { _showHealthbarAnim = value; updateShowHealthbar(); } }

        private bool _showHealthbarHighlight;
        internal bool showHealthbarHighlight { get { return _showHealthbarHighlight; } set { _showHealthbarHighlight = value; updateShowHealthbar(); } }

        private bool _showHighlights = true;
        internal bool showHighlights { get { return _showHighlights; } set { _showHighlights = value; updateHighlights(); } }

        private void updateShowHealthbar()
        {
            healthBar.gameObject.SetActive((showHealthbarAnim || showHealthbarHighlight || this.unit.damage > 0)); //&& this.unit.zone == MGPumZoneType.Battlegrounds
        }



        internal MGPummelz controller;

        private MGPumUnit _unit;

        public MGPumUnit unit
        {
            get
            {
                return _unit;
            }

            set
            {
                if (value == null)
                {
                    Debug.LogError("Unit is set to null.");
                }
                _unit = value;
                updateUnitRender();
            }
        }

        public MGPumUnitRender()
        {
            this.illustrationName = "";
        }



        void Awake()
        {
            
        }

        public void Start()
        {


        }

        public void init(MGPummelz controller, MGPumUnit unit, MGPumFieldRender field)
        {
            this.controller = controller;
            this.unit = unit;
            updateUnitRender();

            Sprite[] highlights = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/highlight");

            highlightLeft.sprite = highlights[unit.ownerID * 2];
            highlightRight.sprite = highlights[unit.ownerID * 2 + 1];
        }

        public void updateUnitRender()
        {
            if (unit != null && !this.illustrationName.Equals(unit.artID))
            {
                Sprite[] sprites = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/illu/" + MGPummelz.getColorForPlayer(unit.ownerID).ToLower() + "/" + unit.artID);
                if(sprites == null || sprites.Length == 0)
                {
                    sprites = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/illu/" + MGPummelz.getColorForPlayer(unit.ownerID).ToLower() + "/default");
                }

                this.illustration.sprite = sprites[0];
                //image.sprite = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("checkers/pieces")[value.ownerID * 2 + (value.isKing ? 1 : 0)];
                this.illustrationName = unit.artID;
            }

          



            this.name = _unit.id + "";

            updateShowHealthbar();
            updateHighlights();
        }

        private void updateHighlights()
        {
            if (controller.inputManager.highlightUnit(unit) && showHighlights)
            {
                highlightLeft.gameObject.SetActive(controller.stateManager.shownStateOracle.canMove(unit));
                highlightRight.gameObject.SetActive(controller.stateManager.shownStateOracle.canAttack(unit));
            }
            else
            {
                highlightLeft.gameObject.SetActive(false);
                highlightRight.gameObject.SetActive(false);
            }
        }

    }
}
