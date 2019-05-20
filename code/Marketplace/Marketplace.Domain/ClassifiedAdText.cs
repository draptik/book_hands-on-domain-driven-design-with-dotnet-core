using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        public static ClassifiedAdText FromString(string Text) => new ClassifiedAdText(Text);
        
        private readonly string _value;

        private ClassifiedAdText(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException(
                    "Text cannot be longer than 100 characters", 
                    nameof(value));
            }
            _value = value;
        }
        
        public static implicit operator string(ClassifiedAdText self) => self._value;
    }
}