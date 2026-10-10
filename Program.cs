using lab1;
using System;
using System.Net.NetworkInformation;

namespace lab1{
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

                SelectionRule byKey = new KeySelectionRule();
                SelectionRule byPopular = new PopularSelectionRule();
                SelectionRule byRandom = new RandomSelectionRule();

                PlayList playList = new PlayList(byKey);
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

                Console.WriteLine("Список треков в плейлисте:");
                foreach (Track music in playList.GetItems())
                {
                    Console.WriteLine("Трек:");
                    Console.WriteLine($"{music}\n");

                }
                Console.WriteLine($"Всего треков в плейлисте: {playList.len()}");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("* Плейлист отсортирован по ключам *");
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("* Плейлист отсортирован по количеству прослушиваний *");
                playList.SetRule(byPopular);
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("* Плейлист отсортирован в произвольном порядке *");
                playList.SetRule(byRandom);
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine(playList.Popular_track());
                Console.WriteLine($"Суммарная длительность прослушиваний: {playList.sum_duration_inSeconds()} сек или {playList.sum_duration_inSeconds() / 60} мин");
                Console.WriteLine($"Количество созданных объектов: {Track.GetTrackCount()}");

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

        static void Print(PlayList playList)
        {
            foreach (Track music in playList.GetItems())
            {
                Console.WriteLine("Трек:");
                Console.WriteLine($"{music}\n");

            }
        }

    }
}