using System;
using System.Collections.Generic;

namespace lab1
{
    public interface ITimeSource { DateTime TimeNow { get; } }
    public interface IRandomSource { int Next(int maxExclusive); }// Как в random от 0 до maxExclusive

    // Возвращает введённую дату
    public class FixedTimeSource : ITimeSource
    {
        public DateTime TimeNow { get; }
        public FixedTimeSource(DateTime time)
        {
            TimeNow = time;
        }
    }

    public class SystemRandomSource : IRandomSource
    {
        private readonly Random r = new Random();
        public int Next(int maxExclusive) => r.Next(maxExclusive);
    }
    // 
    public class FixedRandomSource : IRandomSource
    {
        private readonly int[] values;
        private int index;

        public FixedRandomSource(params int[] seq)
        {
            values = seq;
            index = 0;
        }

        public int Next(int maxExclusive)
        {
            int value = values[index];
            index++;
            return value;
        }
    }


    // Хранилище треков в памяти проги
    public class InMemoryTrackRepository
    {
        private readonly List<Track> tracks = new List<Track>();

        public void Save(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            tracks.RemoveAll(t => t.Key == track.Key);
            tracks.Add(track);
        }

        public Track[] GetAll()
        {
            return tracks.ToArray();
        }
    }

    // Присваение последней даты прослушивания трека
    public class PlaylistService
    {
        private readonly ITimeSource timeSource;
        private readonly InMemoryTrackRepository repository;

        public PlaylistService(ITimeSource timeSource, InMemoryTrackRepository repository)
        {
            if (timeSource == null)
                throw new ArgumentNullException(nameof(timeSource));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            this.timeSource = timeSource;
            this.repository = repository;
        }

        public void Listen(Track track, int times = 1)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track)); 

            track.Play(times);
            track.MarkListened(timeSource.TimeNow);
            repository.Save(track);
        }
    }

}