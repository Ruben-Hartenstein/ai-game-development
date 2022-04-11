using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace mg.pummelz
{
    public class MGPumChainToolTip : MonoBehaviour
    {
        public Image bubble;

        public TextMeshProUGUI text;

        public MGPumChainToolTip()
        {
        }

        

        public void show(String text, MGPumFieldRender field)
        {
            this.transform.position = field.transform.position;
            updateText(text);
            this.gameObject.SetActive(true);
        }

        public void hide()
        {
            this.gameObject.SetActive(false);
        }

        public void updateLayout()
        {
            LayoutRebuilder.MarkLayoutForRebuild(this.gameObject.GetComponent<RectTransform>());
            foreach (TextMeshProUGUI o in this.GetComponentsInChildren<TextMeshProUGUI>())
            {
                LayoutRebuilder.MarkLayoutForRebuild(o.gameObject.GetComponent<RectTransform>());
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        }



        public void updateText(string text)
        {
            this.text.text = MGPummelz.encodeSymbols(text);

            //match box to text
            updateLayout();
        }
      


    }

}
 

