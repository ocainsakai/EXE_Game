// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("cff0nCF4ThXlYONTQjRq4vqnxBMtrqCvny2upa0trq6vA8gDzPsiBSVopE+e0vFBi1WQTxUkhQNc/yuXVbenfg5+Ddn2d2z+jpDmbzy2+VgkaKgBPvSmtmp51A1CQujQyK3pYZ8tro2foqmmhSnnKViirq6uqq+sDbKjyfEvo71iqwxGtpuGLpUHWWtTwzu6uSPCo0FvZ7YRHBTNA+0cUngPhQ5zHQ6oiUzyxSyw5P44XkaDm4AOLkOmj7u0HqrbM0LHr+PBfkLOs+gDYUA8KM+2/GKe2v3CsqyXr5zVAKUeefLVSEC71WGpAJMzuaWyG3DhaVL6PHSJQfllCx2jh59eB9sE0pS2GGjy84busFhBzJM9qh84ndLhv1DvRy1bZq2srq+u");
        private static int[] order = new int[] { 7,1,9,13,8,7,7,8,11,11,10,13,12,13,14 };
        private static int key = 175;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
