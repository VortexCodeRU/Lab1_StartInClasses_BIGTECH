using lab1;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lab1
{
    public class Program
    {
        static void Main()
        {
            try
            {
                IPlayerSettings settings = new PlayerSettings(50, 1);
                IMediaItemFactory factory = new MediaItemFactory();
                SelectionRule byKey = new KeySelectionRule();
                SelectionRule byPopular = new PopularSelectionRule();
                SelectionRule byRandom = new RandomSelectionRule(new SystemRandomSource());

                Console.WriteLine("* Настройки плеера *");
                Console.WriteLine(settings);
                Console.WriteLine("-------------------------------------------------------");

                // Подборка через строителя
                var spec = new PlaylistSpecBuilder()
                    .SetName("Вечерний микс")
                    .SetGenre("HipHop")
                    .SetMaxDuration(7200)
                    .SetOrder("popular")
                    .SetAuthor("User")
                    .Build();

                Console.WriteLine("* Подборка (Builder) *");
                Console.WriteLine($"Название: {spec.Name}");
                Console.WriteLine($"Жанр: {spec.GenreFilter}");
                Console.WriteLine($"Лимит: {spec.MaxDurationSec} сек");
                Console.WriteLine($"Порядок: {spec.Order}");
                Console.WriteLine($"Автор: {spec.Author}");
                Console.WriteLine("-------------------------------------------------------");

                // Элементы через фабрику
                PlayList playList = new PlayList(byKey);

                TryAdd(playList, factory.Create("track", 1, "Camry 3.5", "uncleFlexxx"));
                TryAdd(playList, factory.Create("track", 2, "Чайная медитация", "Лю Цзыци"));
                TryAdd(playList, factory.Create("track", 5, "ADD", "bbno$"));
                TryAdd(playList, factory.Create("track", 4, "PEPSI", "Kamz0ner"));
                TryAdd(playList, factory.Create("podcast", 11, "Лекция по выш мату", "Кожевников"));
                TryAdd(playList, factory.Create("audiobook", 50, "Книга", "Автор"));
                TryAdd(playList, factory.Create("track", 51, "Unknown", "NoName"));

                Console.WriteLine($"Все доступные жанры: {string.Join(" | ", playList.GetAllGenres())}");
                Console.WriteLine("-------------------------------------------------------");

                playList.remove(11);

                Console.WriteLine("Список элементов в плейлисте:");
                Print(playList);
                Console.WriteLine($"Всего элементов: {playList.len()}");
                Console.WriteLine("-------------------------------------------------------");

                Console.WriteLine("* Сортировка по ключам *");
                playList.SetRule(byKey);
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");

                Console.WriteLine("* Сортировка по прослушиваниям *");
                playList.SetRule(byPopular);
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");

                Console.WriteLine("* Случайный порядок *");
                playList.SetRule(byRandom);
                playList.ApplySort();
                Print(playList);
                Console.WriteLine("-------------------------------------------------------");

                Console.WriteLine(playList.Popular_track());
                Console.WriteLine(
                    $"Суммарная длительность: {playList.sum_duration_inSeconds()} сек " +
                    $"или {playList.sum_duration_inSeconds() / 60} мин");
                Console.WriteLine($"Создано треков (счётчик): {Track.GetTrackCount()}");
                Console.WriteLine("-------------------------------------------------------");

                // Проверяем найстроки
                Console.WriteLine("* Настройки: громкость и повтор *");
                Console.WriteLine($"Volume = {settings.Volume}, Replay = {settings.Replay}");

                Console.WriteLine("-------------------------------------------------------");
                Track demoPremium = new Track(101, "Premium Hit", "Star", 180, 100, new[] { "Rap", "Dn'B" }, new PremiumAccess(), new NiceRating());
                Track demoFree = new Track(102, "Free Song", "Artist", 200, 50, new[] { "Pop" }, new FreeAccess(), new NiceRating());

                Console.WriteLine("* Пользователь без подписки *");
                Console.WriteLine("Трек {0}: слушать {1}", demoPremium.Title, demoPremium.CanPlay(false) ? "можно" : "нельзя");
                Console.WriteLine("Трек {0}: слушать {1}", demoFree.Title, demoFree.CanPlay(false) ? "можно" : "нельзя");
                Console.WriteLine("* Пользователь с подпиской *");
                Console.WriteLine("Трек {0}: слушать {1}", demoPremium.Title, demoPremium.CanPlay(true) ? "можно" : "нельзя");
                Console.WriteLine("Трек {0}: слушать {1}", demoFree.Title, demoFree.CanPlay(true) ? "можно" : "нельзя");
                Console.WriteLine("-------------------------------------------------------");

                // Хранилище
                Console.WriteLine("Проверка работы хранилища:");
                ITimeSource time = new FixedTimeSource(new DateTime(2026, 10, 24, 21, 30, 0));
                InMemoryTrackRepository repository = new InMemoryTrackRepository();
                PlaylistService service = new PlaylistService(time, repository);

                repository.Save(demoFree);
                repository.Save(demoPremium);
                service.Listen(demoPremium);

                Track[] savedTracks = repository.GetAll();
                Console.WriteLine("Количество треков в хранилище: " + savedTracks.Length);
                foreach (Track track in savedTracks)
                {
                    Console.WriteLine(
                        $"Ключ: {track.Key}\n" +
                        $"Название: {track.Title}\n" +
                        $"Исполнитель: {track.Artist}\n" +
                        $"Прослушивания: {track.Count_listen}\n" +
                        $"Последнее прослушивание: {track.LastListenedAt?.ToString("dd.MM.yyyy HH:mm:ss") ?? "Ещё не прослушено"}\n");
                }
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("* Прототип еженедельной подборки *");

                var template = new WeeklyPlaylistTemplate(
                    "Хиты недели",
                    new DateTime(2026, 10, 20),
                    new List<MediaItem> { factory.Create("track", 1, "Song A", "Artist"), factory.Create("track", 2, "Song B", "Artist") });

                var week2 = template.Copy();
                week2.SetWeekStart(new DateTime(2026, 10, 27));
                week2.ReplaceTracks(new List<MediaItem> { factory.Create("track", 3, "Song C", "Artist") });

                Console.WriteLine($"Образец: {template.Title}, дата {template.WeekStart:d}, треков {template.Tracks.Count}");
                Console.WriteLine($"Копия:   {week2.Title}, дата {week2.WeekStart:d}, треков {week2.Tracks.Count}");
                // образец не должен измениться
                Console.WriteLine("-------------------------------------------------------");
                const int N = 200_000;
                Console.WriteLine($"* Пул объектов ({N} элементов) *");


                // без пула
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < N; i++)
                {
                    var view = new PlaylistItemView();
                    view.Fill("Title" + i, "Artist", 100, "track");
                    // view просто выбрасываем
                }
                sw.Stop();
                long withoutPool = sw.ElapsedMilliseconds;
                Console.WriteLine($"Без пула: {withoutPool} мс");

                // с пулом
                var pool = new PlaylistItemPool();
                sw.Restart();
                for (int i = 0; i < N; i++)
                {
                    var view = pool.Rent();
                    view.Fill("Title" + i, "Artist", 100, "track");
                    pool.Return(view); // вернули и очистили
                }
                sw.Stop();
                long withPool = sw.ElapsedMilliseconds;
                Console.WriteLine($"С пулом:  {withPool} мс");
                Console.WriteLine($"Разница:  {withoutPool - withPool} мс");
                Console.WriteLine($"В пуле свободно: {pool.FreeCount}");
                Console.WriteLine("-------------------------------------------------------");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void TryAdd(PlayList pl, MediaItem item)
        {
            try
            {
                pl.Add(item);
                Console.WriteLine($"Добавлено №{item.Key}: {item.Title} — '{item.Artist}'\n");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }

        static void Print(PlayList playList)
        {
            foreach (MediaItem music in playList.GetItems())
            {
                Console.WriteLine(music);
                Console.WriteLine();
            }
        }
    }
}