using UnityEngine;

namespace mg.pummelz
{
    public class MGPumYSorter : MonoBehaviour
    {

        public float offset;

        //draw lower units in front
        void LateUpdate()
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.001f * this.transform.localPosition.y + offset);
        }


    }
}
