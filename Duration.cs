using System;

namespace lab1
{
    public class Duration
    {
        private readonly int seconds;
        private static readonly int MAX_DURATION = 3600;
        public Duration(int duration)
        {
            if (duration < 1 || duration > MAX_DURATION)
                throw new ArgumentException($"Некорретная длительность. Только от 1 до {MAX_DURATION}\n");
            this.seconds = duration;
        }

        public int Seconds => seconds;
        public int scam_duration => MAX_DURATION;
    }
}