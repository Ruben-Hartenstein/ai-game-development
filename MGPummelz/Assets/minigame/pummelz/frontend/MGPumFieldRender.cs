using rccg.frontend;
using RelegatiaCCG.rccg.engine;
using RelegatiaCCG.rccg.frontend;
using RelegatiaCCG.rccg.frontend.animations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace mg.pummelz
{

    public class MGPumFieldRender : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
    {
        public Image tile;

        internal MGPummelz controller;

        private MGPumField _field;

        internal MGPumField field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
            }

        }

        public void init(MGPummelz controller, MGPumField field)
        {
            this.controller = controller;
            this.field = field;
        }

        internal Vector2Int coords { get { return field != null ? field.coords : new Vector2Int(-1, -1); } }
        internal int x { get { return field != null ? field.coords.x : -1; } }
        internal int y { get { return field != null ? field.coords.y : -1; } }

        //private MGCheckers controller;

        internal MGPumUnitRender unitRender;


        void Awake()
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            controller.inputManager.fieldPointerUp(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            controller.inputManager.fieldPointerDown(this, eventData.button);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            controller.inputManager.fieldPointerExit(this);
        }
    }

}