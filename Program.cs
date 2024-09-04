using System;
using System.Collections.Generic;

namespace OOP_AS1_Francis_Cabalo
{
    public enum Kind { Dog, Cat, Lizard, Bird }
    public enum Gender { Male, Female }

    public abstract class Pet
    {
        public Gender Gender { get; private set; }
        public string Name { get; private set; }
        public string Owner { get; private set; }

        protected Pet(Gender gender, string name, string owner)
        {
            Gender = gender;
            Name = name;
            Owner = owner;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Name = {Name}, Gender = {Gender}, Owner = {Owner}";
        }
    }

    public class Dog : Pet
    {
        public string Breed { get; private set; }

        public Dog(Gender gender, string name, string breed, string owner)
            : base(gender, name, owner)
        {
            Breed = breed;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Breed = {Breed}";
        }
    }

    public class Cat : Pet
    {
        public bool IsLonghaired { get; private set; }

        public Cat(Gender gender, string name, bool isLonghaired, string owner)
            : base(gender, name, owner)
        {
            IsLonghaired = isLonghaired;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Longhaired = {IsLonghaired}";
        }
    }

    public class Lizard : Pet
    {
        public bool CanFly { get; private set; }

        public Lizard(Gender gender, string name, bool canFly, string owner)
            : base(gender, name, owner)
        {
            CanFly = canFly;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Can Fly = {CanFly}";
        }
    }

    public class Bird : Pet
    {
        public bool CanFly { get; private set; }

        public Bird(Gender gender, string name, bool canFly, string owner)
            : base(gender, name, owner)
        {
            CanFly = canFly;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Can Fly = {CanFly}";
        }
    }

    public class PetFactory
    {
        public static Pet CreatePet(Kind kind, Gender gender, string name, string owner)
        {
            return kind switch
            {
                Kind.Dog => CreateDog(gender, name, owner),
                Kind.Cat => CreateCat(gender, name, owner),
                Kind.Lizard => CreateLizard(gender, name, owner),
                Kind.Bird => CreateBird(gender, name, owner),
                _ => throw new ArgumentException("Invalid pet kind.")
            };
        }

        private static Dog CreateDog(Gender gender, string name, string owner)
        {
            Console.Write("Enter breed of the dog: ");
            var breed = Console.ReadLine()?.Trim();
            ValidateNotEmpty(breed, "Breed cannot be empty.");
            return new Dog(gender, name, breed, owner);
        }

        private static Cat CreateCat(Gender gender, string name, string owner)
        {
            Console.Write("Is the cat longhaired (y/n)? ");
            var isLonghaired = ParseYesNo(Console.ReadLine()?.Trim());
            return new Cat(gender, name, isLonghaired, owner);
        }

        private static Lizard CreateLizard(Gender gender, string name, string owner)
        {
            Console.Write("Can the lizard fly (y/n)? ");
            var canFly = ParseYesNo(Console.ReadLine()?.Trim());
            return new Lizard(gender, name, canFly, owner);
        }

        private static Bird CreateBird(Gender gender, string name, string owner)
        {
            Console.Write("Can the bird fly (y/n)? ");
            var canFly = ParseYesNo(Console.ReadLine()?.Trim());
            return new Bird(gender, name, canFly, owner);
        }

        private static bool ParseYesNo(string input)
        {
            if (input?.ToLower() == "y")
                return true;
            if (input?.ToLower() == "n")
                return false;
            throw new ArgumentException("Invalid input!");
        }

        private static void ValidateNotEmpty(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(message);
        }
    }

    public class Program
    {
        private static readonly List<Pet> pets = new List<Pet>();

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("1. Add Pet");
                Console.WriteLine("2. List All Pets");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddPet();
                        break;
                    case "2":
                        ListAllPets();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private static void AddPet()
        {
            try
            {
                var kind = GetPetKind();
                var gender = GetGender();
                var name = GetName();
                var owner = GetOwner();

                var pet = PetFactory.CreatePet(kind, gender, name, owner);

                Console.WriteLine($"You are about to add the following pet:\n{pet}");
                if (ConfirmAddition())
                {
                    pets.Add(pet);
                    Console.WriteLine("Pet added successfully.");
                }
                else
                {
                    Console.WriteLine("Pet addition cancelled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void ListAllPets()
        {
            if (pets.Count == 0)
            {
                Console.WriteLine("No pets in the inventory.");
                return;
            }

            foreach (var pet in pets)
            {
                Console.WriteLine(pet);
            }
        }

        private static Kind GetPetKind()
        {
            Console.Write("Enter what kind of pet: ");
            return ParseEnum<Kind>(Console.ReadLine()?.Trim(), "Invalid pet kind!.");
        }

        private static Gender GetGender()
        {
            Console.Write("Enter the gender: ");
            return ParseEnum<Gender>(Console.ReadLine()?.Trim(), "Invalid gender!");
        }

        private static string GetName()
        {
            Console.Write("Enter name of the pet: ");
            var name = Console.ReadLine()?.Trim();
            ValidateNotEmpty(name, "Pet name cannot be empty.");
            return name;
        }

        private static string GetOwner()
        {
            Console.Write("Enter the owner of the pet: ");
            var owner = Console.ReadLine()?.Trim();
            ValidateNotEmpty(owner, "Owner cannot be empty.");
            return owner;
        }

        private static T ParseEnum<T>(string input, string errorMessage) where T : struct, Enum
        {
            if (Enum.TryParse<T>(input, true, out var result))
                return result;
            throw new ArgumentException(errorMessage);
        }

        private static void ValidateNotEmpty(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(message);
        }

        private static bool ConfirmAddition()
        {
            Console.Write("Do you want to proceed? (y/n): ");
            return Console.ReadLine()?.Trim().ToLower() == "y";
        }
    }
}
