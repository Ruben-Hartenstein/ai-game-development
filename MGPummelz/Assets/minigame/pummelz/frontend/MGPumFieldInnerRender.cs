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

    public class MGPumFieldInnerRender : MonoBehaviour, IPointerEnterHandler
    {
        public MGPumFieldRender fieldRender;

        
        public void OnPointerEnter(PointerEventData eventData)
        {
            fieldRender.controller.inputManager.fieldPointerEnter(fieldRender);
        }

        
    }

}