using System;
using System.Collections.Generic;

namespace lab1
{
    public class WeeklyPlaylistTemplate
    {
        public string Title { get; }
        public DateTime WeekStart { get; private set; }
        private readonly List<MediaItem> tracks;

        public WeeklyPlaylistTemplate(string title, DateTime weekStart, List<MediaItem> tracks)
        {
            Title = title;
            WeekStart = weekStart;
            this.tracks = tracks;
        }

        public IReadOnlyList<MediaItem> Tracks => tracks;
        // глубокая копия по списку
        public WeeklyPlaylistTemplate Copy()
        {
            return new WeeklyPlaylistTemplate(Title, WeekStart, new List<MediaItem>(tracks));
        }

        public void SetWeekStart(DateTime d) => WeekStart = d;
        public void ReplaceTracks(List<MediaItem> newTracks)
        {
            tracks.Clear();
            tracks.AddRange(newTracks);
        }
    }
}
