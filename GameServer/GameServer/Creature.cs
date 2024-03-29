﻿using System;
using System.Collections.Generic;

namespace GameServer
{
    public class Creature
    {
        //Stats
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Level { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxMana { get; set; }
        public int CurrentMana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public List<Item> Items { get; set; }
        public List<Spell> Spells { get; set; }

        //Equipment
        public Helmet Helmet { get; set; }
        public Chest Chest { get; set; }
        public Legs Legs { get; set; }
        public Boots Boots { get; set; }
        public Weapon Weapon { get; set; }
        //Creature damage is calculated by their Weapon Attack + Strength
        public int Damage { get { return Weapon.Attack + Strength; } }
        //Creature defense is calculated by their Weapon Armor + their total equipment defense
        public int Armor { get { return Weapon.Armor + Helmet.Defense + Chest.Defense + Legs.Defense + Boots.Defense; } }

        public Creature()
        {
            Name = "";
            Gold = 0;
            Level = 0;
            MaxHealth = 0;
            CurrentHealth = 0;
            MaxMana = 0;
            CurrentMana = 0;
            Strength = 0;
            Dexterity = 0;
            Constitution = 0;
            Intelligence = 0;
            Wisdom = 0;
            Charisma = 0;
            Items = new List<Item>();
            Spells = new List<Spell>();

            Helmet = new Helmet();
            Chest = new Chest();
            Legs = new Legs();
            Boots = new Boots();
            Weapon = new Weapon();
        }
        public void ModifyStrength(int amount)
        {
            Strength += amount;
        }
        public void ModifyDexterity(int amount)
        {
            Dexterity += amount;
        }
        public void ModifyConstitution(int amount)
        {
            Constitution += amount;
        }
        public void ModifyIntelligence(int amount)
        {
            Intelligence += amount;
        }
        public void ModifyWisdom(int amount)
        {
            Wisdom += amount;
        }
        public void ModifyCharisma(int amount)
        {
            Charisma += amount;
        }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        public void AddSpell(Spell spell)
        {
            Spells.Add(spell);
        }

        public int Attack(Creature target)
        {
            //Attack damage is a random number in range of the player's ((Strength + Weapon Damage) +- Dexterity) - (Target's Armor + Defense)
            Random ranGen = new Random();
            int attackDamage = ranGen.Next(Damage - Dexterity, Damage + Dexterity);
            int armor = target.Armor;
            int attackTotal = attackDamage - armor;
            //If the armor lowers the attack below 0, the attack does 0, not negative damage
            if (attackTotal < 0)
            {
                attackTotal = 0;
            }

            return attackTotal;
        }
        public void Hurt(int damage)
        {
            //If the creature would be damaged a negative amount, do 0 damage instead of healing them
            if (damage < 0)
            {
                damage = 0;
            }

            CurrentHealth -= damage;
        }
    }
}
