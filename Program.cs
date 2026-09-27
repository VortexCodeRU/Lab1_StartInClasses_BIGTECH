using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Xml.Schema;
using static Track;

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
    }
    // Геттеры
    public int Key => key;
    public string Title => title;
    public string Artist => artist;
    public int Duration => duration;
    public int Count_listen => count_tracks;
    public string[] Genre => genre;

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




    // Класс-контейнер
    public class PlayList
    {
        private Track[] playLists; // Хранилище
        private int count;

        public PlayList(int capacity = 15)
        {
            playLists = new Track[capacity];
            count = 0;
        }
        public Track[] GetTracks()
        {
            Track[] copy = new Track[count];
            Array.Copy(playLists, copy, count);
            return copy;
        }

        public void Add(Track track)
        {
            int[] digits = new int[count]; // Проверка на занятые ключи
            for (int i = 0; i < count; i++)
            {
                digits[i] = playLists[i].key;
            }

            for (int i = 0; i < count; i++) 
            {
                if (playLists[i].key == track.key)
                    throw new ArgumentException($"Трек с ключом {track.key} существует.\nЗанятые ключи: {string.Join(", ", digits)}\n");
            }
            // Проверка памяти
            if (count >= playLists.Length)
            {
                Array.Resize(ref playLists, playLists.Length * 2);
            }
            playLists[count] = track;
            count++;
        }
        public void remove(int num)
        {
            int index = -1;
            for (int i = 0; i < count; i++) {
                if (playLists[i].key == num)
                {
                    index = i; break;
                }
            }
            if (index == -1)
                throw new ArgumentException($"Трек с ключом {num} не найден\n");

            for (int j = index; j < count - 1; j++)
            {
                playLists[j] = playLists[j + 1];
            }
            playLists[count - 1] = null;
            count--;
        }

        public int sum_duration_inSeconds()
        {
            int result_sum = 0;
            for (int i = 0; i < count; i++)
            {
                result_sum += playLists[i].duration;
            }
            return result_sum;
        }

        public String Popular_track()
        {
            if (count == 0) {
                throw new ArgumentException("Плейлист пуст\n");
            }
            int best = 0;
            Track best_track = null;
            for (int i = 0; i < count; i++)
            {
                if (best < playLists[i].count_listen)
                {
                    best = playLists[i].count_listen;
                    best_track = playLists[i];
                }
            }
            return $"Самый популярный трек - {best_track.title} от '{best_track.artist}'";
        }

        public void sort()
        {
            // Пузырьковая сортировка
            for (int j = 0; j < count - 1; j++) {
                for (int i = 0; i < count - 1; i++)
                {
                    if (playLists[i + 1].count_listen < playLists[i].count_listen)
                    {
                        Track a = playLists[i + 1];
                        playLists[i + 1] = playLists[i];
                        playLists[i] = a;
                    }
                }
            }
        }
        public string[] GetAllGenres() // Вывод всех добавленных уникальных жанров
        {
            int total = 0;
            for (int i = 0; i < count; i++)
            {
                total += playLists[i].getGenres().Length;
            }
            string[] result = new string[total];
            int unique_index = 0;
            for (int i = 0;i < count; i++)
            {
                string[] genres = playLists[i].getGenres();
                for (int j = 0; j < genres.Length; j++)
                {
                    bool flag = true;
                    for (int k = 0; k < unique_index; k++)
                    {
                        if (result[k] == genres[j]){
                            flag = false;
                            break;
                        }
                    }
                    if (flag){
                        result[unique_index] = genres[j];
                        unique_index++;
                    }
                }
            }
            string[] temp = new string[unique_index];
            Array.Copy(result, temp, unique_index);
            return temp;
        }

        public int len()
        {
            return count;
        }
    }

}



public class Program
{
    static void Main()
    {
        try
        {
            Track t1 = new Track(1, "Camry 3.5", "uncleFlexxx", 192, 5_000_000, new[] { "HipHop", "Pop" });
            Track t2 = new Track(2, "Чайная медитация", "Лю Цзыци", 687, 700, new[] { "Chill", "Symphony", "Relax" });
            Track t3 = new Track(5, "ADD", "bbno$", 198, 30_231_110, new[] { "Rap", "HipHop", "Pop" });
            Track t4 = new Track(2, "PEPSI", "Kamz0ner", 102, 1_212_000, new[] { "Rap", "ClubHouse", "Relax" });
            Track t5 = new Track(11, "Лекция по выш мату", "Кожевников", 1200, 0, new[] { "CrazyHistory", "UnknownInfo", "lazy subject" });
            Track t6 = new Track(50);
            Track t7 = new Track(51);


            PlayList playList = new PlayList();
            TryAdd(playList, t1);
            TryAdd(playList, t2);
            TryAdd(playList, t3);// ошибка
            TryAdd(playList, t4);
            TryAdd(playList, t5);
            TryAdd(playList, t6);
            TryAdd(playList, t7);

            Console.WriteLine($"Все доступные жанры: {string.Join(" | ", playList.GetAllGenres())}");
            Console.WriteLine("-------------------------------------------------------");
            playList.remove(11);
            //Console.WriteLine(t1);

            foreach (Track music in playList.GetTracks())
            {
                Console.WriteLine("Список треков в плейлисте:");
                Console.WriteLine($"{music}\n");

            }
            Console.WriteLine($"Всего треков в плейлисте: {playList.len()}");
            Console.WriteLine("-------------------------------------------------------");
            playList.sort();
            Console.WriteLine("* Плейлист отсортирован по количеству прослушиваний *");
            foreach (Track music in playList.GetTracks())
            {
                Console.WriteLine("Список треков в плейлисте:");
                Console.WriteLine($"{music}\n");

            }
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(playList.Popular_track());
            Console.WriteLine($"Суммарная длительность прослушиваний: {playList.sum_duration_inSeconds()} сек или {playList.sum_duration_inSeconds() / 60} мин");
            Console.WriteLine($"Количество созданных объектов: {GetTrackCount()}");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Плохая практика работы геттера. Внешний код способен изменять содержимое ;(\nРезультат:");
            Track t = new Track(1, "Song", "Artist", 200, 0, new[] { "Rock", "Pop" });
            string[] bad = t.getGenre();
            bad[0] = "Hacked";
            bad[1] = "System";
            Console.WriteLine(string.Join(", ", t.getGenre()));

            Console.WriteLine("Защитное копирование в геттере, позволяющее запрететь изменение в хранилище\nРезультат:");
            Track test = new Track(404, "Song", "Artist", 200, 0, new[] { "Rock", "Pop" });
            string[] good = test.getGenres();
            good[0] = "Hacked";
            good[1] = "System";
            Console.WriteLine(string.Join(", ", test.getGenres()));

        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }

    }

    static void TryAdd(PlayList pl, Track t)
    {
        try
        {
            pl.Add(t);
            Console.WriteLine($"Новое добавление. №{t.Key}: {t.Title} - '{t.Artist}'\n");
        }
        catch(ArgumentException e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }

    }

}