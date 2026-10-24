using lab1;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace lab1{
    public class Program
    {
        static void Main()
        {
            try
            {
                AccessLevel free = new FreeAccess();
                AccessLevel premium = new PremiumAccess();
                AgeRating good = new NiceRating();
                AgeRating bad = new BadRating();
                Track t1 = new Track(1, "Camry 3.5", "uncleFlexxx", -30, 5_000_000, new[] { "HipHop", "Pop" }, premium, good);
                Track t2 = new Track(2, "Чайная медитация", "Лю Цзыци", 687, 700, new[] { "Chill", "Symphony", "Relax" }, free, good);
                Track t3 = new Track(5, "ADD", "bbno$", 198, 30_231_110, new[] { "Rap", "HipHop", "Pop" }, premium, bad);
                Track t4 = new Track(2, "PEPSI", "Kamz0ner", 102, 1_212_000, new[] { "Rap", "ClubHouse", "Relax" }, free, bad);
                Track t5 = new Track(11, "Лекция по выш мату", "Кожевников", 1200, 0, new[] { "CrazyHistory", "UnknownInfo", "lazy subject" }, free, good);
                Track t6 = new Track(50);
                Track t7 = new Track(51);

                SelectionRule byKey = new KeySelectionRule();
                SelectionRule byPopular = new PopularSelectionRule();
                SelectionRule byRandom = new RandomSelectionRule(new SystemRandomSource());
                
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
                Print(playList);
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
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("* Пользователь без подписки *");
                Console.WriteLine("Трек {0}: слушать {1}", t1.Title, t1.CanPlay(false) ? "можно" : "нельзя");
                Console.WriteLine("Трек {0}: слушать {1}", t2.Title, t2.CanPlay(false) ? "можно" : "нельзя");
                Console.WriteLine("* Пользователь с подпиской *");
                Console.WriteLine("Трек {0}: слушать {1}", t1.Title, t1.CanPlay(true) ? "можно" : "нельзя");
                Console.WriteLine("Трек {0}: слушать {1}", t2.Title, t2.CanPlay(true) ? "можно" : "нельзя");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Проверка работы хранилища:");
                ITimeSource time = new FixedTimeSource(new DateTime(2026, 10, 24, 21, 30, 0));
                InMemoryTrackRepository repository = new InMemoryTrackRepository();
                PlaylistService service = new PlaylistService(time, repository);

                repository.Save(t2);
                repository.Save(t1);
                repository.Save(t4);
                service.Listen(t3);
                Track[] savedTracks = repository.GetAll();

                Console.WriteLine("Количество треков в хранилище: " + savedTracks.Length);

                foreach (Track track in savedTracks)
                {
                    Console.WriteLine(
                        $"Ключ: {track.Key}, \n" +
                        $"Название: {track.Title}, \n" +
                        $"Исполнитель: {track.Artist}, \n" +
                        $"Прослушивания: {track.Count_listen}, \n" +
                        $"Последнее прослушивание: {track.LastListenedAt?.ToString("dd.MM.yyyy HH:mm:ss") ?? "Ещё не прослушено"}");
                }

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

        static void TryAdd(PlayList pl, MediaItem item)
        {
            try
            {
                pl.Add(item);
                Console.WriteLine($"Новое добавление. №{item.Key}: {item.Title} - '{item.Artist}'\n");
            }
            catch(ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }

        }

        static void Print(PlayList playList)
        {
            foreach (MediaItem music in playList.GetItems())
            {
                Console.WriteLine("Трек:");
                Console.WriteLine($"{music}\n");

            }
        }

    }
}