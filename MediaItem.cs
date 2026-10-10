using System;

namespace lab1
{
    public abstract class MediaItem
    {
        private int _key;
        private string _title;
        private string _artist;
        private int _count_listen;
        protected MediaItem(int key, string title, string artist, int count_listen)
        {
            if (key < 0)
                throw new ArgumentException("Номер отрицательный\n");
            if (count_listen < 0)
                throw new ArgumentException("Число прослушиваний не может быть отрицательным\n");
            if (title is null)
                throw new ArgumentException("Название не должно быть пустым\n");
            this._key = key;
            this._title = title;
            this._artist = artist;
            this._count_listen = count_listen;
        }

        public int Key => _key;
        public string Title => _title;
        public string Artist => _artist;
        public int Count_listen => _count_listen;

        public void Play(int times = 1)
        {
            if (times < 1)
            {
                throw new ArgumentException($"Некорретное число прослушиваний\n");
            }
            _count_listen += times;
        }
        // каждый наследник обязан реализовать по-своему
        public abstract int GetDuration();

        public override String ToString()
        {
            return $"Номер: {_key}\nИмя: {_title}\nИсполнитель: {_artist}\nЧисло прослушиваний: {_count_listen}\n";
        }
    }
}
