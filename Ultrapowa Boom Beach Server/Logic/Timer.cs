using System;

namespace UCS.Logic
{
    internal class Timer
    {
        int m_vSeconds;
        internal DateTime StartTime;

        public Timer()
        {
            StartTime = new DateTime(1970, 1, 1);
            m_vSeconds   = 0;
        }

        public void FastForward(int seconds)
        {
            m_vSeconds -= seconds;
        }

        public int GetRemainingSeconds(DateTime time, bool boost = false, DateTime boostEndTime = default(DateTime), float multiplier = 0f)
        {
            int result = int.MaxValue;
            if (!boost)
            {
                result = m_vSeconds - (int)time.Subtract(StartTime).TotalSeconds;
            }
            else
            {
                if (boostEndTime >= time)
                    result = m_vSeconds - (int)(time.Subtract(StartTime).TotalSeconds * multiplier);
                else
                {
                    Single boostedTime = (float)time.Subtract(StartTime).TotalSeconds - (float)(time - boostEndTime).TotalSeconds;
                    Single notBoostedTime = (float)time.Subtract(StartTime).TotalSeconds - boostedTime;

                    result = m_vSeconds - (int)(boostedTime * multiplier + notBoostedTime);
                }
            }
            if (result <= 0)
                result = 0;
            return result;
        }

        public int GetRemainingSeconds(DateTime time)
        {
            int result = m_vSeconds - (int) time.Subtract(StartTime).TotalSeconds;
            if (result <= 0)
            {
                result = 0;
            }
            return result;
        }

        public void StartTimer(int seconds, DateTime time)
        {
            StartTime = time;
            m_vSeconds = seconds;
        }
    }
}
