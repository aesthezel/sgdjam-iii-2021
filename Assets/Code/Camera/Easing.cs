namespace Code.Camera
{
    public enum Ease
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart
    }

    public static class Easing
    {
        public static float MakeEase(Ease type, float t, float b, float c, float d)
        {
            switch (type)
            {
                case Ease.Linear:
                    return LinearEasing(t, b, c, d);
                
                case Ease.InQuad:
                    return EaseInQuad(t, b, c, d);
                
                case Ease.OutQuad:
                    return EaseOutQuad(t, b, c, d);
                
                case Ease.InCubic:
                    return EaseOutCubic(t, b, c, d);
                
                case Ease.InQuart:
                    return EaseInQuart(t, b, c, d);
                
                case Ease.OutCubic:
                    return EaseOutCubic(t, b, c, d);
                
                case Ease.OutQuart:
                    return EaseOutQuart(t, b, c, d);
                
                case Ease.InOutQuad:
                    return EaseInOutQuad(t, b, c, d);
                
                case Ease.InOutCubic:
                    return EaseInOutCubic(t, b, c, d);
                
                case Ease.InOutQuart:
                    return EaseInOutQuart(t, b, c, d);
                
                default:
                   return LinearEasing(t, b, c, d);
            }
        }
        
        // t = Current time
        // b = start value
        // c = change in value
        // d = duration
        private static float LinearEasing(float t, float b, float c, float d) =>  c*t/d + b ;
        
        private static float EaseInQuad(float t, float b, float c, float d)
        {
            t /= d;
            return c*t*t + b;
        }
        
        private static float EaseOutQuad(float t, float b, float c, float d)
        {
            t /= d;
            return -c * t *  (t-2f) + b;
        }
        
        private static float EaseInOutQuad(float t, float b, float c, float d) {
            t /= d/2f;
            if (t < 1) return c/2*t*t + b;
            t--;
            return -c/2 * (t * (t-2) - 1) + b;
        }
        
        private static float EaseInCubic(float t, float b, float c, float d) {
            t /= d;
            return c*t*t*t + b;
        }
        
        private static float EaseOutCubic(float t, float b, float c, float d) {
            t /= d;
            t--;
            return c*(t*t*t + 1) + b;
        }

        private static float EaseInOutCubic(float t, float b, float c, float d) {
            t /= d/2;
            if (t < 1) return c/2*t*t*t + b;
            t -= 2;
            return c/2*(t*t*t + 2) + b;
        }

        private static float EaseInQuart(float t, float b, float c, float d) {
            t /= d;
            return c*t*t*t*t + b;
        }
        
        private static float EaseOutQuart(float t, float b, float c, float d) {
            t /= d;
            t--;
            return -c * (t*t*t*t - 1) + b;
        }
        
        private static float EaseInOutQuart(float t, float b, float c, float d) {
            t /= d/2;
            if (t < 1) return c/2*t*t*t*t + b;
            t -= 2;
            return -c/2 * (t*t*t*t - 2) + b;
        }
    }
}