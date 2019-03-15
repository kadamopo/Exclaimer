using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTest
{
    /// <summary>
    /// This class contains extension methods for the data type Char.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// This is a property that provides only a getter and allows us to specify, and get, a
        /// set of characters that correspond to the letters of the English alphabet.
        /// 
        /// ASSUMPTIONS:
        /// 
        /// The assumption made for our specific problem is that we are dealing with text of the
        /// English language, thus we only need provision for the English alphabet letters.
        /// For this reason and for simplicity I named this property Letters instad of EnglishLetters
        /// or EnglishAlphabetLetters, or LatinLetters or something similar.
        /// 
        /// It is obvious that if we need to deal with other sets of characters, such as characters
        /// from different alphabets, numericals, symbols, etc., then we can easily replicate this
        /// approach and create similar properties for the required sets of characters. The same logic
        /// applies if we need to split the set of letters into subsets such as lower case letters,
        /// capital letters, vowels, consonants, etc.
        /// 
        /// Hopefully this is an elegant and simple sulution in specifying a set of characters.
        /// </summary>
        public static char[] Letters { get; } = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                                             'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// An extension method for type Char that checks if the character that
        /// calls the method is a letter of the English alphabet or not.
        /// </summary>
        /// <param name="self">The character that calls this method.</param>
        /// <returns>True if the character is a letter of the English alphabet, False if it is not.</returns>
        public static bool IsLetter(this char self) => Letters.Contains(self);
    }
}
