using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public static class HighQualityCardValidator
    {
        public static bool ValidateCard(Card card)
        {
            foreach (var property in card.GetType().GetProperties())
            {
                var attribute = property.GetCustomAttribute<PowerValidationAttribute>();
                if (attribute != null)
                {
                    int power = (int)property.GetValue(card);
                    if (card.HighQuality && power < attribute.MinValueForProperty)
                    {
                        Console.WriteLine($"Validation failed: {property.Name} must be at least {attribute.MinValueForProperty} when HighQuality is true.");
                        return false;
                    }
                }
            }
            return true;
        }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PowerValidationAttribute : Attribute
    {
        public int MinValueForProperty { get; }
        public PowerValidationAttribute(int minValueProp)
        {
            MinValueForProperty = minValueProp;
        }
    }

    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public string CardId { get; set; }
        public string CardName { get; set; }


        //have to check the validator while reading cards from json later, this doesnt do anything yet!
        [PowerValidation(7)]

        public int CardPower
        {
            get { return CardPower; }
            set
            {
                if (value > 15)
                {
                    CardPower = 15;
                }
                else if (value < 0)
                {
                    CardPower = 1;
                }
                else
                {
                    CardPower = value;
                }
            }
        }

        //false is low (bronze or smth)
        //true is high (gold or smth, stronger card)
        public bool HighQuality { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public Card(string cardName, int cardPower, bool highQuality, string description)
        {
            CardName = cardName;
            CardPower = cardPower;
            HighQuality = highQuality;
            Description = description;
            CardId = Guid.NewGuid().ToString();
        }
    }


}
