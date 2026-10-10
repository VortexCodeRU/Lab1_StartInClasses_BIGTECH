using System;
using lab1;

namespace lab1
{
    // Класс-контейнер
    public class PlayList
    {
        //private Track[] playLists; // Хранилище
        private MediaItem[] items;
        private int count;
        private SelectionRule rule;

        public PlayList(SelectionRule rule = null, int capacity = 15)
        {
            if (capacity < 1)
                throw new ArgumentException("Размер плейлиста должен бытьь больше 0");
            if (rule is null)
                throw new ArgumentException("Правило не задано");
            items = new MediaItem[capacity];
            count = 0;
            this.rule = rule;
        }
        public MediaItem[] GetItems()
        {
            MediaItem[] copy = new MediaItem[count];
            Array.Copy(items, copy, count);
            return copy;
        }

        public void Add(MediaItem item)
        {
            int[] digits = new int[count]; // Проверка на занятые ключи
            for (int i = 0; i < count; i++)
            {
                digits[i] = items[i].Key;
            }

            for (int i = 0; i < count; i++) 
            {
                if (items[i].Key == item.Key)
                    throw new ArgumentException($"Трек с ключом {item.Key} существует.\nЗанятые ключи: {string.Join(", ", digits)}\n");
            }
            // Проверка памяти
            if (count >= items.Length)
            {
                Array.Resize(ref items, items.Length * 2);
            }
            items[count] = item;
            count++;
        }
        public void remove(int num)
        {
            int index = -1;
            for (int i = 0; i < count; i++) {
                if (items[i].Key == num)
                {
                    index = i; break;
                }
            }
            if (index == -1)
                throw new ArgumentException($"Трек с ключом {num} не найден\n");

            for (int j = index; j < count - 1; j++)
            {
                items[j] = items[j + 1];
            }
            items[count - 1] = null;
            count--;
        }

        public int sum_duration_inSeconds()
        {
            int result_sum = 0;
            for (int i = 0; i < count; i++)
            {
                result_sum += items[i].GetDuration();
            }
            return result_sum;
        }

        public String Popular_track()
        {
            if (count == 0) {
                throw new ArgumentException("Плейлист пуст\n");
            }
            int best = 0;
            MediaItem best_track = null;
            for (int i = 0; i < count; i++)
            {
                if (best < items[i].Count_listen)
                {
                    best = items[i].Count_listen;
                    best_track = items[i];
                }
            }
            return $"Самый популярный трек - {best_track.Title} от '{best_track.Artist}'";
        }
        public string[] GetAllGenres() // Вывод всех добавленных уникальных жанров
        {
            int total = 0;
            for (int i = 0; i < count; i++)
            {
                total += items[i].GetGenres().Length;
            }
            string[] result = new string[total];
            int unique_index = 0;
            for (int i = 0;i < count; i++)
            {
                string[] genres = items[i].GetGenres();
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

        public void SetRule(SelectionRule rule)
        {
            if (rule is null)
                throw new ArgumentNullException("Правило не задано");
            this.rule = rule;
        }

        public void ApplySort()
        {
            rule.sort(items, count);
        }
    }
}