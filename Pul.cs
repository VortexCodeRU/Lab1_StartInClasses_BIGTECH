using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lab1
{
    public class PlaylistItemView
    {
        public string Title { get; private set; }
        public string Artist { get; private set; }
        public int DurationSeconds { get; private set; }
        public string Kind { get; private set; }

        public void Fill(string title, string artist, int durationSeconds, string kind)
        {
            Title = title;
            Artist = artist;
            DurationSeconds = durationSeconds;
            Kind = kind;
        }

        public void Reset()
        {
            Title = null;
            Artist = null;
            DurationSeconds = 0;
            Kind = null;
        }
    }

    public class PlaylistItemPool
    {
        private readonly Stack<PlaylistItemView> free = new Stack<PlaylistItemView>();

        public int FreeCount => free.Count;

        public PlaylistItemView Rent()
        {
            return free.Count > 0 ? free.Pop() : new PlaylistItemView();
        }

        public void Return(PlaylistItemView item)
        {
            if (item == null) return;
            item.Reset();
            free.Push(item);
        }
    }
}