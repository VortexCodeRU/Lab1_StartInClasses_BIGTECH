using System;
using lab1;

namespace lab1{
    // Класс-контейнер
    public class PlayList
    {
        private Track[] playLists; // Хранилище
        private int count;

        public PlayList(int capacity = 15)
        {
            if (capacity < 1)
                throw new ArgumentException("Размер плейлиста должен бытьь больше 0");
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
                digits[i] = playLists[i].Key;
            }

            for (int i = 0; i < count; i++) 
            {
                if (playLists[i].Key == track.Key)
                    throw new ArgumentException($"Трек с ключом {track.Key} существует.\nЗанятые ключи: {string.Join(", ", digits)}\n");
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
                if (playLists[i].Key == num)
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
                result_sum += playLists[i].Duration;
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
                if (best < playLists[i].Count_listen)
                {
                    best = playLists[i].Count_listen;
                    best_track = playLists[i];
                }
            }
            return $"Самый популярный трек - {best_track.Title} от '{best_track.Artist}'";
        }

        public void sort()
        {
            // Пузырьковая сортировка
            for (int j = 0; j < count - 1; j++) {
                for (int i = 0; i < count - 1; i++)
                {
                    if (playLists[i + 1].Count_listen < playLists[i].Count_listen)
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