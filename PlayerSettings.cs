using System;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace lab1
{
    public interface IPlayerSettings {
        int Volume { get; }
        int Replay { get; }
    }

    public class PlayerSettings : IPlayerSettings
    { // Инициализируется 1 раз при загрузке класса. Потокобезопасная реализация
        // private static readonly Lazy<PlayerSettings> _lazy = new Lazy<PlayerSettings>(() => new PlayerSettings());
        // public static PlayerSettings Instance => _lazy.Value;

        public int Volume { get; private set; }
        public int Replay { get; private set; }
        // 0 = без повтора, 1 = повтор одного, 2 = повтор списка

        //private PlayerSettings()
        //{
        //    Volume = 50;
        //    Replay = 0;
        //}
        public PlayerSettings(int volume, int replay)
        {
            if (volume < 0 || volume > 100)
                throw new ArgumentException("Громкость не соотвествует действительности");
            if (replay < 0 || replay > 2)
                throw new ArgumentException("Режим повтора не соотвествует действительности");
            Volume = volume;
            Replay = replay;
        }

        public override string ToString()
        {
            return $"Громкость: {Volume}\nРежим прослушивания: {Replay}\n";
        }
    }

    public class Player
    {
        private readonly IPlayerSettings settings;
        public Player(IPlayerSettings settings) => this.settings = settings;
        public bool IsMuted => settings.Volume == 0;
    }
}
