using System;

namespace lab1
{
    public class Podcast : MediaItem
    {
        private int episodeCount;
        public Podcast(int key, string title, string artist, int count_listen, int episodeCount) : base(key, title, artist, count_listen)
        {
            if (episodeCount < 1) {
                throw new ArgumentException("Эпизодов должно быть > 1");
            }
            this.episodeCount = episodeCount;
        }
        // пусть 1 эпизод длится 30 минут
        public override int GetDuration() => episodeCount * 1800;
    }
}
