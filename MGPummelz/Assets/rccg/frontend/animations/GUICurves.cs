using System.Collections.Generic;
using UnityEngine;

namespace rccg.frontend.common
{
    public static class GUICurves
    {
        public const string Linear = "Linear";

        public const string SineEaseIn = "SineEaseIn";
        public const string QuadEaseIn = "QuadEaseIn";
        public const string CubicEaseIn = "CubicEaseIn";
        public const string QuartEaseIn = "QuartEaseIn";
        public const string QuintEaseIn = "QuintEaseIn";
        public const string ExpoEaseIn = "ExpoEaseIn";
        public const string CircEaseIn = "CircEaseIn";
        public const string BackEaseIn = "BackEaseIn";
        public const string ElasticEaseIn = "ElasticEaseIn";
        public const string BounceEaseIn = "BounceEaseIn";

        public const string SineEaseOut = "SineEaseOut";
        public const string QuadEaseOut = "QuadEaseOut";
        public const string CubicEaseOut = "CubicEaseOut";
        public const string QuartEaseOut = "QuartEaseOut";
        public const string QuintEaseOut = "QuintEaseOut";
        public const string ExpoEaseOut = "ExpoEaseOut";
        public const string CircEaseOut = "CircEaseOut";
        public const string BackEaseOut = "BackEaseOut";
        public const string ElasticEaseOut = "ElasticEaseOut";
        public const string BounceEaseOut = "BounceEaseOut";

        public const string SineEaseInOut = "SineEaseInOut";
        public const string QuadEaseInOut = "QuadEaseInOut";
        public const string CubicEaseInOut = "CubicEaseInOut";
        public const string QuartEaseInOut = "QuartEaseInOut";
        public const string QuintEaseInOut = "QuintEaseInOut";
        public const string ExpoEaseInOut = "ExpoEaseInOut";
        public const string CircEaseInOut = "CircEaseInOut";
        public const string BackEaseInOut = "BackEaseInOut";
        public const string ElasticEaseInOut = "ElasticEaseInOut";
        public const string BounceEaseInOut = "BounceEaseInOut";

        public const string SineEaseOutIn = "SineEaseOutIn";
        public const string QuadEaseOutIn = "QuadEaseOutIn";
        public const string CubicEaseOutIn = "CubicEaseOutIn";
        public const string QuartEaseOutIn = "QuartEaseOutIn";
        public const string QuintEaseOutIn = "QuintEaseOutIn";
        public const string ExpoEaseOutIn = "ExpoEaseOutIn";
        public const string CircEaseOutIn = "CircEaseOutIn";
        public const string BackEaseOutIn = "BackEaseOutIn";
        public const string ElasticEaseOutIn = "ElasticEaseOutIn";
        public const string BounceEaseOutIn = "BounceEaseOutIn";

        private static Dictionary<string, AnimationCurve> library;

        public static AnimationCurve linear()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0, 0));
            curve.AddKey(new Keyframe(1, 1));
            curve.postWrapMode = WrapMode.Clamp;
            return curve;
        }

        public static AnimationCurve linear(float from, float to)
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0, from));
            curve.AddKey(new Keyframe(1, to));
            curve.postWrapMode = WrapMode.Clamp;
            return curve;
        }

        public static AnimationCurve easedSimple()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0, 0, 0, 0));
            curve.AddKey(new Keyframe(1, 1, 0, 0));
            return curve;
        }

        public static AnimationCurve get(string id)
        {
            if(library == null)
            {
                library = CurveGenerator.generateCurveLibrary();
            }

            if(library.ContainsKey(id))
            {
                return library[id];
            }
            else
            {
                Debug.LogError("Cannot find animation curve " + id);
                return linear();
            }
        }

        
    }
}
