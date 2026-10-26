using System;
using System.Collections.Generic;

namespace lab1
{
    public interface IMediaItemFactory
    {
        MediaItem Create(string type, int key, string title, string artist);
    }
    public class MediaItemFactory : IMediaItemFactory
    {
        private readonly Dictionary<string, Func<int, string, string, MediaItem>> registry;
        public MediaItemFactory()
        {
            registry = new Dictionary<string, Func<int, string, string, MediaItem>>(StringComparer.OrdinalIgnoreCase)
            {
                ["track"] = (k, t, a) => new Track(k, t, a, 100, 0, new[] { "Pop" }, new FreeAccess(), new NiceRating()),
                ["podcast"] = (k, t, a) => new Podcast(k, t, a, 0, episodeCount: 5),
                ["audiobook"] = (k, t, a) => new Audiobook(k, t, a, 0, 1000, 10)
                //["radio"] = (k, t, a) => new RadioStream(k, t, a)
            };
        }

        public MediaItem Create(string type, int key, string title, string artist)
        {
            if (!registry.TryGetValue(type, out var creator))
                throw new ArgumentException($"Неизвестный тип: {type}");
            return creator(key, title, artist); // Делегатор, отвечающий за создание объекта типа метода
        }
    }
}
