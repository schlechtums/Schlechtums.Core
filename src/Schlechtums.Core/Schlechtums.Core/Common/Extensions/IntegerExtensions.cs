using System;

namespace Schlechtums.Core.Common.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns a pluralized string ending based on a number.
        /// </summary>
        /// <param name="pluralForm">The plural form.  Ex. Computer: s (ToPlural("s"), Entry: ies (ToPlural("ies")</param>
        /// <param name="baseForm">The base ending.  Ex. Computer: (empty string) (ToPlural("s", ["" or omit]), Entry: y. (ToPlural("ies", "y").</param>
        /// <returns>The word ending.  Ex. if the word is entry/entries, then the string being pluralized should only be "entr", and it will get an ending of either "y" or "ies".</returns>
        public static string ToPlural(this int num, string pluralForm = "s", string baseForm = "")
        {
            return num == 1 ? baseForm : pluralForm;
        }

        /// <summary>
        /// Returns a pluralized string based on a number.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="word">The word to pluralize.</param>
        /// <param name="pluralForm">The plural form.  Ex. Computer: s (ToPlural("s"), Entry: ies (ToPlural("ies")</param>
        /// <param name="baseForm">The base ending.  Ex. Computer: (empty string) (ToPlural("s", ["" or omit]), Entry: y. (ToPlural("ies", "y").</param>
        /// <returns>The pluralized word.Ex. if the word is entry/entries, then the string being pluralized should only be "entr", and it will get an ending of either "y" or "ies".</returns>
        public static string ToPlural(this int num, string word, string pluralForm = "s", string baseForm = "")
        {
            return word + num.ToPlural(pluralForm, baseForm);
        }
    }
}