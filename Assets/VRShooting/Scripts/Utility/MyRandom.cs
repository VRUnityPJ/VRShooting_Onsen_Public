namespace VRShooting.Scripts.Utility
{
    public class MyRandom
    {
        /// <summary>
        /// ハッシュ関数
        /// 参考：https://www.slideshare.net/UnityTechnologiesJapan/ss-82219762
        /// </summary>
        /// <returns></returns>
        public static int xxHash(int data, int seed)
        {
            uint v = (uint)seed + 374761393U + 4U;
            v += (uint)data + 3266489917U;
            v = ((v << 17) | (v >> 15)) * 668265263U;
            v ^= v >> 15;
            v *= 2246822519U;
            v ^= v >> 13;
            v *= 3266489917U;
            v ^= v >> 16;
            return (int)v;
        }
    }
}