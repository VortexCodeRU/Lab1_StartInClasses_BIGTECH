using System;
using lab1;

namespace lab1{
    public class Track : MediaItem
    {
        // поля
        private static int count_tracks = 0;
        private readonly Duration duration;

        private string[] genre;

        // Конструктор с параметрами по умолчанию
        public Track(int key, int duration=1, int count_listen = 1) : base(key, "Unknown", "NoName", count_listen)
        {
            this.duration = new Duration(duration);
            this.genre = new string[0];
            count_tracks++;
        }

        // Конструктор с конкретными значениями
        public Track(int key, string title, string artist, int duration, int count_listen, string[] genre) : base(key, title, artist, count_listen)
        {
            this.duration = new Duration(duration);
            count_tracks++;
            this.genre = (string[])genre.Clone();
        }
        // Геттеры
        public int Duration => duration.Seconds;
        public string[] Genre => genre;


        // Работа с жанрами
        public override string[] GetGenres() => (string[])genre.Clone();
        // Прочие методы
        public static int GetTrackCount() => count_tracks;
        public override int GetDuration() => duration.Seconds;
        public override String ToString()
        {
            return base.ToString() + $"Список жанров: {(genre.Length > 0 ? string.Join(", ", genre) : "нет")}";
        }
    }
}