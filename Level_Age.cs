using System;

namespace lab1
{
    public interface AccessLevel
    {
        bool CanPlay(bool userIsPremium);
        string Name { get; }
    }
    public class FreeAccess : AccessLevel
    {
        public string Name => "Free";
        public bool CanPlay(bool userIsPremium) => true; // Всегда можно юзать трек
    }
    public class PremiumAccess : AccessLevel {
        public string Name => "Premium";
        public bool CanPlay(bool userIsPremium) => userIsPremium; // трек только для премиум юзеров
    }


    public interface AgeRating
    {
        bool nsfw { get; }
        string label { get; }
    }
    public class NiceRating : AgeRating
    {
        public bool nsfw => false;
        public string label => "Good";
    }
    public class BadRating : AgeRating
    {
        public bool nsfw => true;
        public string label => "Warning";
    }
}
