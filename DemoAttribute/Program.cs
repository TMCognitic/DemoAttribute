using System;
using System.Collections.Generic;
using System.Reflection;

namespace DemoAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            Hero hero = new Hero();
            List<Monstre> monstres = new List<Monstre>(new Monstre[] { new Loup(), new Orque(), new Dragonnet(), new LoupDesBois() });

            foreach (Monstre m in monstres)
                hero.Loot(m);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    abstract class LootAttribute : Attribute
    {
        private Random _random;
        private int _maxValue;

        public string Text { get; private set; }

        public int Value
        {
            get
            {
                return _random.Next(_maxValue) + 1;
            }
        }

        public LootAttribute(int maxValue, string text)
        {
            if (maxValue < 2)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be > 1!!!");

            Text = text;
            _maxValue = maxValue;
            _random = new Random();
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class OrAttribute : LootAttribute
    {       
        public OrAttribute() : base(6, "de l'or")
        {
            
        }

        public OrAttribute(int maxValue) : base(maxValue, "de l'or")
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    class CuirAttribute : LootAttribute
    {
        public CuirAttribute() : base(4, "du cuir")
        {
        }
    }

    class GemmeAttribute : LootAttribute
    {
        public GemmeAttribute() : base(3, "des gemmes")
        {

        }
    }

    class Hero
    {
        public void Loot(Monstre m)
        {
            Type monsterType = m.GetType();

            Console.WriteLine($"Je loot un {monsterType.Name}");

            IEnumerable<LootAttribute> lootAttributes = monsterType.GetCustomAttributes<LootAttribute>();
            foreach(LootAttribute lootAttribute in lootAttributes)
                Console.WriteLine($"Je ramasse {lootAttribute.Text} : {lootAttribute.Value}");

            Console.WriteLine();
        }
    }

    abstract class Monstre
    {

    }

    [Cuir]
    class Loup : Monstre
    {

    }

    [Cuir]
    class LoupDesBois : Loup
    {

    }

    [Or]
    class Orque : Monstre
    {

    }

    [Cuir]
    [Or(100)]
    [Gemme]
    class Dragonnet : Monstre
    {

    }
}
