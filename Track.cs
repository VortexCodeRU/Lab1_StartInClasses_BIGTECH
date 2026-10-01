using System;
using lab1;

namespace lab1{
    public class Track
    {
        // поля
        private int key;
        private string title;
        private string artist;
        private int duration;
        private int count_listen;
        private static int count_tracks = 0;
        private static readonly int MAX_DURATION = 3600;
        private PlayList pl; // Ссылка на внешний объект

        private string[] genre;

        // Конструктор с параметрами по умолчанию
        public Track(int key, int duration=1, int count_listen = 1)
        {
            if (duration < 1 || duration > MAX_DURATION)
                throw new ArgumentException($"Некорретная длительность. Только от 1 до {MAX_DURATION}\n");
            if (count_listen < 0)
                throw new ArgumentException("Число прослушиваний не может быть отрицательным\n");
            this.key = key;
            this.title = "Unknown";
            this.artist = "NoName";
            this.duration = duration;
            this.count_listen = count_listen;
            this.genre = new string[0];
            count_tracks++;

            this.pl = new PlayList();
        }

        // Конструктор с конкретными значениями
        public Track(int key, string title, string artist, int duration, int count_listen, string[] genre)
        {
            if (key < 0)
                throw new ArgumentException("Номер отрицательный\n");
            if (duration < 1 || duration > MAX_DURATION)
                throw new ArgumentException($"Некорретная длительность. Только от 1 до {MAX_DURATION}\n");
            if (count_listen < 0)
                throw new ArgumentException("Число прослушиваний не может быть отрицательным\n");
            if (title is null)
                throw new ArgumentException("Название не должно быть пустым\n");

            this.key = key;
            this.title = title;
            this.artist = artist;
            this.duration = duration;
            this.count_listen = count_listen;
            count_tracks++;
            this.genre = (string[])genre.Clone();

            this.pl = new PlayList();
        }
        // Геттеры
        public int Key => key;
        public string Title => title;
        public string Artist => artist;
        public int Duration => duration;
        public int Count_listen => count_listen;
        public string[] Genre => genre;
        public PlayList playList => pl;

        public void play() { play(1); } // Иначе дублирование логики

        public void play(int times)
        {
            if (times < 1)
            {
                throw new ArgumentException($"Некорретное число прослушиваний\n");
            }
            this.count_listen += times;
        }

        public override String ToString()
        {
            return $"Номер: {key}\nИмя: {title}\nИсполнитель: {artist}\nДлительность трека: {duration}\nЧисло прослушиваний: {count_listen}\nСписок жанров: {(genre.Length > 0 ? string.Join(", ", genre) : "нет")}";
        }

        // Работа с жанрами 
        public string[] getGenre()
        {
            return genre; // Плохая практика
        }
        public string[] getGenres()
        {
            return (string[])genre.Clone();
        }
        
        public static int GetTrackCount()
        {
            return count_tracks;
        }
    }
}