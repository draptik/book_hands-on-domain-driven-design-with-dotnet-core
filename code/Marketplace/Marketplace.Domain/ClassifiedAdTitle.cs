using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new ClassifiedAdTitle(title);
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");
            
            var value = new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty));
            
            CheckValidity(value);
            return new ClassifiedAdTitle(value);
        }
        
        private readonly string _value;

        // Satisfy the serialization requirements (i.e. RavenDb)
        protected ClassifiedAdTitle() { }

        internal ClassifiedAdTitle(string value) => _value = value;

        public static implicit operator string(ClassifiedAdTitle self) => self._value;
        
        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Title cannot be longer that 100 characters");
        }
    }
}