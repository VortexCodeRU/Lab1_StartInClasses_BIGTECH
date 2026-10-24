using System;
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
    }
}