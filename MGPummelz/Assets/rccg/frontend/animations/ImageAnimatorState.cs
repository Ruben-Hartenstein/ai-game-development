using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class ImageAnimatorState
    {

        public Transform parent;

        public Vector3 pos;


        public Vector3 scale;

        public float alpha;

        public float rotationY;

        public ImageAnimatorState()
        {

        }

        public ImageAnimatorState(Transform transform)
        {
            parent = transform.parent;
            pos = transform.localPosition;
            scale = transform.localScale;
            CanvasGroup cg = transform.gameObject.GetComponent<CanvasGroup>();
            if(cg != null)
            {
                alpha = transform.gameObject.GetComponent<CanvasGroup>().alpha;
            }
            else
            {
                alpha = 1.0f;
            }

            
            rotationY = transform.localEulerAngles.y;
        }

        public void add(ImageAnimatorState diffState)
        {
            this.pos += diffState.pos;
            this.scale += diffState.scale;
            this.alpha += diffState.alpha;
            this.rotationY += diffState.rotationY;
        }

        public ImageAnimatorState(ImageAnimatorState oldState, ImageAnimatorState newState)
        {
            this.parent = oldState.parent;
            this.pos = newState.pos - oldState.pos;
            this.scale = newState.scale - oldState.scale;
            this.alpha = newState.alpha - oldState.alpha;
            this.rotationY = newState.rotationY - oldState.rotationY;
        }

        public ImageAnimatorState deepCopy()
        {
            ImageAnimatorState copy = new ImageAnimatorState();
            copy.parent = parent;
            copy.pos = this.pos;
            copy.scale = this.scale;
            copy.alpha = this.alpha;
            copy.rotationY = this.rotationY;
            return copy;
        }

    }
}