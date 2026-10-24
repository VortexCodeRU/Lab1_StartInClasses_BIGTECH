using System;
using lab1;

namespace lab1{
    public class Track : MediaItem
    {
        // поля
        private static int count_tracks = 0;
        private readonly Duration duration;
        private string[] genre;
        private readonly AccessLevel access;
        private readonly AgeRating rating;

        // Конструктор с параметрами по умолчанию
        public Track(int key, int duration=1, int count_listen = 1) : base(key, "Unknown", "NoName", count_listen)
        {
            if (access is null)
                this.access = new FreeAccess();
            if (rating is null)
                this.rating = new NiceRating();
            this.duration = new Duration(duration);
            this.genre = new string[0];
            count_tracks++;
            this.access = new FreeAccess();
            this.rating = new NiceRating();
        }

        // Конструктор с конкретными значениями
        public Track(int key, string title, string artist, int duration, int count_listen, string[] genre, AccessLevel access, AgeRating rating) : base(key, title, artist, count_listen)
        {
            this.duration = new Duration(duration);
            count_tracks++;
            this.genre = (string[])genre.Clone();
            this.access = access;
            this.rating = rating;
        }
        // Геттеры
        public int Duration => duration.Seconds;
        public string[] Genre => genre;

        public bool IsNsfw { get; internal set; }


        // Работа с жанрами
        public override string[] GetGenres() => (string[])genre.Clone();
        // Прочие методы
        public static int GetTrackCount() => count_tracks;
        public override int GetDuration() => duration.Seconds;
        public override String ToString()
        {
            return base.ToString() + $"Доступ: {access.Name}, рейтинг: {rating.label}\nСписок жанров: {(genre.Length > 0 ? string.Join(", ", genre) : "нет")}";
        }

        public bool CanPlay(bool userIsPremium)
        {
            return access.CanPlay(userIsPremium);
        }

        public DateTime? LastListenedAt { get; private set; } // У трека может ещё не быть даты прослушивания
        public void MarkListened(DateTime time)
        {
            if (time == null)
                throw new ArgumentNullException("Ещё не прослушено");
            LastListenedAt = time;
        }
    }
}