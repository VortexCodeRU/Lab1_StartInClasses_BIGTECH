using System;

namespace lab1
{
    public interface SelectionRule { void sort(MediaItem[] item, int count); }

    public class KeySelectionRule : SelectionRule
    {
        public void sort(MediaItem[] item, int count)
        {
            for (int j = 0; j < count - 1; j++)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    if (item[i + 1].Key < item[i].Key)
                    {
                        MediaItem a = item[i + 1];
                        item[i + 1] = item[i];
                        item[i] = a;
                    }
                }
            }
        }
    }
    public class PopularSelectionRule : SelectionRule
    {
        public void sort(MediaItem[] item, int count)
        {
            // Пузырьковая сортировка
            for (int j = 0; j < count - 1; j++)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    if (item[i + 1].Count_listen < item[i].Count_listen)
                    {
                        MediaItem a = item[i + 1];
                        item[i + 1] = item[i];
                        item[i] = a;
                    }
                }
            }
        }
    }
    public class RandomSelectionRule : SelectionRule
    {
        private readonly Random random = new Random();

        public void sort(MediaItem[] item, int count)
        {
            for (int i = count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                MediaItem a = item[i];
                item[i] = item[j];
                item[j] = a;
            }
        }
    }
}