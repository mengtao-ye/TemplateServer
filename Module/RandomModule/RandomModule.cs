using System;

namespace YSF
{
    public class RandomModule : Singleton<RandomModule>
    {
        private Random mRandom;
        public RandomModule()
        {
            mRandom = new Random();
        }
        /// <summary>
        /// 获取百分比概率
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetRandom(byte value)
        {
            if (value < 0)
            {
                value = 1;
            }
            if (value > 100)
            {
                value = 100;
            }
            int data =  mRandom.Next(0, 100);
            if (data < value)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
