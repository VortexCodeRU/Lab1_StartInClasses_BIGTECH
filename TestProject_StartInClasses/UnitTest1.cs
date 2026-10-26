using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using Xunit;

namespace lab1.Tests
{
    public class TrackTests
    {
        private AccessLevel premium = new PremiumAccess();
        private AgeRating good = new NiceRating();
        // Проверка длительности
        [Theory]
        [InlineData(1)]
        [InlineData(3600)]
        public void Допустимая_длительность_принимается(int sec)
        {
            var t = new Track(1, "A", "B", sec, 0, new[] { "Pop" }, premium, good);
            Assert.Equal(sec, t.GetDuration());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(3601)]
        public void Недопустимая_длительность_отклоняется(int sec)
        {
            Assert.Throws<ArgumentException>(() => new Track(1, "A", "B", sec, 0, new[] { "Pop" }, premium, good));
        }
        // Проверка названия трека
        [Fact]
        public void Имя_непустое_принимается()
        {
            var t = new Track(1, "Song", "B", 1, 0, new[] { "Pop" }, premium, good);
            Assert.Equal("Song", t.Title);
        }

        [Theory]
        [InlineData(null)]
        //[InlineData(" ")]
        public void Недоспутимое_имя_отклоняется(string title)
        {
            Assert.Throws<ArgumentException>(() => new Track(1, title, "B", 1, 0, new[] { "Pop" }, premium, good));
        }

        // Проверка - прослушивания только растут
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Play_с_неположительным_числом_запрещён(int times)
        {
            var t = new Track(1, "A", "B", 100, 5, new[] { "Pop" }, premium, good);
            Assert.Throws<ArgumentException>(() => t.Play(times));
        }

        // Проверка валидности ключа: k > -1
        [Fact]
        public void Недопускает_отрицательный_ключ()
        {
            Assert.Throws<ArgumentException>(() => new Track(-1, "A", "B", 1, 0, new[] { "Pop" }, premium, good));
        }

        // Проверка на иерархию
        // 1. Общее поведение Play()
        [Fact]
        public void Play_увеличивает_прослушивания_у_Track()
        {
            var t = new Track(1, "A", "B", 100, 5, new[] { "Pop" }, premium, good);
            t.Play(2);
            Assert.Equal(7, t.Count_listen);
        }

        [Fact]
        public void Play_увеличивает_прослушивания_у_Podcast()
        {
            var p = new Podcast(2, "Show", "Host", 3, episodeCount: 4);
            p.Play(2);
            Assert.Equal(5, p.Count_listen);
        }

        [Fact]
        public void Play_увеличивает_прослушивания_у_Audiobook()
        {
            var a = new Audiobook(3, "Book", "Author", 1, durationSec: 1000, book_mark: 100);
            a.Play(2);
            Assert.Equal(3, a.Count_listen);
        }

        // 2. Разная реализация GetDuration()
        [Fact]
        public void Track_GetDuration_равен_секундам()
        {
            var t = new Track(1, "A", "B", 192, 0, new[] { "Pop" }, premium, good);
            Assert.Equal(192, t.GetDuration());
        }

        [Fact]
        public void Podcast_GetDuration_считает_эпизоды()
        {
            var p = new Podcast(1, "Show", "Host", 0, episodeCount: 3);
            Assert.Equal(3 * 1800, p.GetDuration());
        }

        [Fact]
        public void Audiobook_GetDuration_равен_длительности_книги()
        {
            var a = new Audiobook(1, "Book", "Author", 0, durationSec: 500, book_mark: 10);
            Assert.Equal(500, a.GetDuration());
        }

        [Fact]
        public void Сумма_длительностей_по_MediaItem_без_проверки_типа()
        {
            MediaItem[] items =
            {
                new Track(1, "A", "B", 100, 0, new[] { "Pop" }, premium, good),
                new Podcast(2, "S", "H", 0, 2),
                new Audiobook(3, "B", "A", 0, 300, 50)
            };

            int sum = 0;
            foreach (MediaItem m in items)
                sum += m.GetDuration();

            Assert.Equal(100 + 2 * 1800 + 300, sum);
        }


        // 
        public class PlaylistServiceTests
        {
            private readonly AccessLevel premium = new PremiumAccess();
            private readonly AgeRating good = new NiceRating();

            private Track CreateTrack() { return new Track(1, "Song", "Artist", 180, 0, new[] { "Pop" }, premium, good); }

            [Fact]
            public void Прослушивание_сохраняет_заданное_время()
            {
                var expected = new DateTime(2026, 10, 24, 21, 30, 0);
                var time = new FixedTimeSource(expected);
                var repository = new InMemoryTrackRepository();
                var service = new PlaylistService(time, repository);
                var track = CreateTrack();

                service.Listen(track);

                Assert.Equal(expected, track.LastListenedAt);
            }

            [Fact]
            public void Перемешивание_с_заглушкой_воспроизводимо()
            {
                var rule = new RandomSelectionRule(
                    new FixedRandomSource(0, 0));

                MediaItem[] items =
                {
                    CreateTrack(), new Track(2, "Song2", "Artist", 180, 0, new[] { "Pop" }, premium, good),
                    new Track(3, "Song3", "Artist", 180, 0, new[] { "Pop" }, premium, good)
                };

                rule.sort(items, items.Length);

                Assert.Equal(new[] { 2, 3, 1 }, Array.ConvertAll(items, item => item.Key));
            }

            [Fact]
            public void Прослушанный_трек_сохраняется_в_библиотеке()
            {
                var time = new lab1.FixedTimeSource(DateTime.MinValue);
                var repository = new InMemoryTrackRepository();
                var service = new PlaylistService(time, repository);
                var track = CreateTrack();

                service.Listen(track);

                var saved = repository.GetAll();

                Assert.Single(saved);
                Assert.Equal(track.Key, saved[0].Key);
            }


            public class RandomSelectionRuleTests
            {
                private readonly AccessLevel premium = new PremiumAccess();
                private readonly AgeRating good = new NiceRating();

                private Track CreateTrack(int key, string title) { return new Track(key, title, "Artist", 180, 0, new[] { "Pop" }, premium, good); }
                
                // Проверка на корректность правила сортировки
                [Fact]
                public void Sort_WithFixedRandomSource_ShufflesPlaylist()
                {
                    MediaItem[] items = { CreateTrack(1, "Track 1"), CreateTrack(2, "Track 2"), CreateTrack(3, "Track 3") };

                    // Заглушка
                    IRandomSource random = new FixedRandomSource(0, 0);
                    SelectionRule rule = new RandomSelectionRule(random);

                    // перемешиваем подборку
                    rule.sort(items, items.Length);

                    // проверка ожидаемого порядка
                    int[] actualKeys = Array.ConvertAll(items, item => item.Key);

                    Assert.Equal(new[] { 2, 3, 1 }, actualKeys);
                }

                // Проверка на кол-во треков
                [Fact]
                public void Sort_DoesNotLoseOrDuplicateTracks()
                {
                    MediaItem[] items = { CreateTrack(1, "Track 1"), CreateTrack(2, "Track 2"), CreateTrack(3, "Track 3") };

                    IRandomSource random = new FixedRandomSource(0, 0);
                    SelectionRule rule = new RandomSelectionRule(random);

                    rule.sort(items, items.Length);

                    Assert.Equal(3, items.Length);
                    Assert.Equal( new[] { 1, 2, 3 }, Array.ConvertAll(items, item => item.Key).OrderBy(key => key).ToArray());
                }
            }
        }

        // Проверка на глобальность синголтона. Тест изолирован - значения тестов не мешают друг другу
        [Fact]
        public void При_нулевой_громкости_плеер_считается_выключенным()
        {
            var player = new Player(new PlayerSettings(0, 0));
            Assert.True(player.IsMuted);
        }
        [Fact]
        public void При_ненулевой_громкости_плеер_включён()
        {
            var player = new Player(new PlayerSettings(50, 0));
            Assert.False(player.IsMuted);
        }
        // Player работает с interface, а не с конкретным PlayerSettings - можно ставить любую реализацию
        public sealed class FakePlayerSettings : IPlayerSettings
        {
            public FakePlayerSettings(int volume, int replay) { Volume = volume; Replay = replay; }
            public int Volume { get; }
            public int Replay { get; }
        }

        [Fact]
        public void Плеер_использует_внедрённую_реализацию()
        {
            var player = new Player(new FakePlayerSettings(volume: 0, replay: 0));
            Assert.True(player.IsMuted);
        } // При синголтоне - это невозможно, там жеёстко PlayerSettings.Instance

        // Проверка на соотвествие типа словаря
        [Fact]
        public void Неизветсный_тип_отклоняется()
        {
            var z = new MediaItemFactory();
            Assert.Throws<ArgumentException>(() => z.Create("video", 1, "A", "B"));
        }

        // Проверка на работу глубокой копии
        [Fact]
        public void Изменение_копии_не_меняет_образец()
        {
            var original = new WeeklyPlaylistTemplate("Week", DateTime.Today, new List<MediaItem> { new Track(1, "A", "B", 100, 0, new[] { "Pop" }, new FreeAccess(), new NiceRating()) });
            var copy = original.Copy();

            copy.SetWeekStart(DateTime.Today.AddDays(7));
            copy.ReplaceTracks(new List<MediaItem>());

            Assert.Single(original.Tracks);
            Assert.NotEqual(original.WeekStart, copy.WeekStart);
        }

        // Проверка работы пула
        [Fact]
        public void После_возврата_в_пул_данные_очищены()
        {
            var pool = new PlaylistItemPool();
            var v = pool.Rent();
            v.Fill("Secret", "Artist", 200, "track");
            pool.Return(v);

            var v2 = pool.Rent(); // тот же объект или другой из стека
            Assert.Null(v2.Title);
            Assert.Null(v2.Artist);
            Assert.Equal(0, v2.DurationSeconds);
        }

        [Fact]
        public void Пул_переиспользует_объект()
        {
            var pool = new PlaylistItemPool();
            var v1 = pool.Rent();
            pool.Return(v1);
            var v2 = pool.Rent();
            Assert.Same(v1, v2); // тот же экземпляр
        }
    }
}