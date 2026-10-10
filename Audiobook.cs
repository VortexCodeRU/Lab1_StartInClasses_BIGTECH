using System;

namespace lab1
{
    public class Audiobook : MediaItem
    {
        private Duration duration;
        private int book_mark;
        public Audiobook(int key, string title, string artist, int count_listen, int durationSec, int book_mark) : base(key, title, artist, count_listen)
        {
            this.duration = new Duration(durationSec);
            if (book_mark < 1 || book_mark > duration.scam_duration)
            {
                throw new ArgumentException("Некорректная закладка");
            }
            this.book_mark = book_mark;
        }

        public override int GetDuration() => duration.Seconds;
    }
}
