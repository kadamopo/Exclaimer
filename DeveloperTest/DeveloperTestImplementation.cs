#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeveloperTestInterfaces;

namespace DeveloperTest
{
    public sealed class DeveloperTestImplementation : IDeveloperTest
    {
        #region Question One

        /// <summary>
        /// This is the answer to question 1. 
        /// 
        /// ASSUMPTIONS:
        /// ============
        /// 
        /// 1. The text read by the reader only contains words of the English language, so in this
        /// case there is no provision for words of other languages written in different alphabets,
        /// or for numericals, etc. Please also see the assumptions listed in class CharExtensions.cs
        /// for more details.
        /// 
        /// 2. The second assumption, and in accordance to assumption 1 above, is that when it comes
        /// to deciding if a character is part of a word or not, we assume that we are always dealing
        /// with characters of the English alphabet, both lower case and capitals. So, there is no
        /// provision for characters of different alphabets, numericals, etc.
        /// 
        /// 3. Another assumption is that it is preferable to deal with the input character stream
        /// dynamically, on the fly, instead of reading the whole stream and storing it in a local
        /// variable first before processing it. This, for example, could help in situations of extremely
        /// long streams (for example reading a character stream from a file) that would require the use
        /// of extensive amounts of in-memory storage prior to processing.
        /// 
        /// LOGIC:
        /// ======
        /// 
        /// An instance of this class (DeveloperTestImplementation) is created elsewhere in the code
        /// (specifically in the unit test: StandardTest.TestQuestionOne()). At that moment two objects
        /// are passed to this method as dependencies using the method dependency injection pattern,
        /// the first one is a reader object, which is an instance of the SimpleCharacterReader class that
        /// implements the ICharacterReader interface, and the second one is an output object, which is an
        /// instance of the Question1TestOutput class that implements the IOutputResult interface.
        /// 
        /// The purpose of this method is to use these two objects in order to read a character stream,
        /// dynamically process the character stream in order to separate it into English words, then order
        /// those words by frequency and then alphabetically, and finally create an appropriate output in
        /// the required output format that will be tested by the relevant unit test mentioned above for its
        /// correctness (i.e. the words that contains, the frequency that each word appears in the character
        /// stream, and whether these words have been ordered by frequency and alphabetically as required).
        /// 
        /// Reading the character stream: In order to successfully read the character stream this method uses
        /// a simple do-while loop that in each iteration reads a new character, then decides if the character
        /// is a letter of the English alphabet or not (using the extension method IsLetter()) and accordingly
        /// either adds the character to the next word if it is indeed a letter or adds the word to a dictionary
        /// of words if the last read character is a white space, new line character, a comma, etc, i.e. anything
        /// other than a letter. When a word is added to the dictionary the algorithm makes sure that the
        /// string that holds the next word is initialised to an empty string in order to be able to hold the
        /// next word successfully. The algorithm then deals with the EndOfStreamException raised by the reader
        /// by making sure in the finally part of the try-catch-finally block that the very last word read is not
        /// lost, but stored in the dictionary in the same way with all the previous words.
        /// 
        /// The last line of this method makes sure that the dictionary of words and word frequences is sorted
        /// according to the requirements, i.e. first by word frequency and then alphabetically, and then creates
        /// the desired output. These two tasks, sorting the dictionary and creating the required output, have
        /// been implemented in separate private method in order to declutter the main algorithm, improve its
        /// readibity, and demonstrate how to create methods for reoccurring tasks and functions, improving reusability,
        /// and making it more clear where and how we change the state of certain objects (instead of changing the
        /// state of objects all over the place). For example we can see that the method AddStringToDictionary(...)
        /// is called twice, once from inside the main do-while loop and then again from the finally sub-block of the
        /// try-catch-finally block.
        /// </summary>
        /// <param name="reader">An object of a class that implements the ICharacterReader interface, which provides
        /// a method for reading the next character of a character stream.</param>
        /// <param name="output">An object of a class that implements the IOutputResult interface, which allows to
        /// format an output according to specific requirements.</param>
        public void RunQuestionOne(ICharacterReader reader, IOutputResult output)
        {
            // A dictionary collection that holds words as strings and the frequency of their appearence as integers.
            IDictionary<string, int> wordDictionary = new Dictionary<string, int>();

            // A string variable that helps us form the next word from the input character stream.
            string nextWord = string.Empty;

            using (reader)
            {
                try
                {
                    // This is the main loop that reads a stream of characters, one by one,
                    // splits the stream into English words, according to the assumptions
                    // made above, and then stores the words into a dictionary collection
                    // keeping also track of how often each word appears in the input stream
                    // (word frequency).
                    do
                    {
                        // Read the next character from the stream of characters.
                        char nextChar = reader.GetNextChar();

                        // As long as the next character is a letter keep adding it to the next word,
                        // as soon as you have encountered a word's end (indicated by a whitespace
                        // character, a symbol such as a comma or a full stop, a new line character or
                        // something similar) add the word to the dictionary and reset the variable
                        // in order to be used to form the next word from scratch.
                        if (nextChar.IsLetter())
                        {
                            nextWord += nextChar.ToString().ToLower();
                        }
                        else
                        {
                            if (nextWord != string.Empty)
                            {
                                AddStringToDictionary(wordDictionary, nextWord);
                                nextWord = string.Empty;
                            }
                        }

                    } while (true);
                }
                catch (EndOfStreamException e)
                {
                    // Normally an error message, like the one below, would be logged in a log file or
                    // log database, by being passed to an appropriate method of a dedicated logger object.
                    // As this is out of the scope of this exercise, for now I am just imitating the
                    // error message logging by just displaying the error message to the console.
                    Console.WriteLine($"Error reading stream: {e.GetType().Name}.");
                }
                finally
                {
                    // Here we make sure that we do not miss the very last word of the input stream
                    // because of the EndOfStreamException thrown by the GetNextChar() method of the reader.
                    if (nextWord != string.Empty)
                    {
                        AddStringToDictionary(wordDictionary, nextWord);
                    }
                }

                // Sort the dictionary by word frequency and then alphabetically and then
                // create the required output.
                CreateOutput(SortDictionary(wordDictionary), output);
            }
        }

        /// <summary>
        /// This method adds a string to a dictionary that has string keys and integer values.
        /// 
        /// The idea here is that the first time we add a string as a key into the dictionary we set
        /// the corresponding value for that key to one. Everytime that the same string key comes back
        /// for addition to the dictionary, instead of adding it again (that is not possible anyway
        /// because the keys are unique) we increase its corresponding value by one. 
        /// 
        /// In relation to the problem in hand this seem to be a good representation of the collections
        /// of words that we have in our input stream of characters, together with their frequency.
        /// Everytime we come across a new word we add it to the dictionary. On the other hand if the
        /// word has appeared before, meaning it has already been added to the dictionary, we just
        /// increase its value by one, i.e. we use the value to represent the frequency of the word
        /// in the input stream.
        /// </summary>
        /// <param name="dictionary">A dictionary collection with string keys and integer values</param>
        /// <param name="str">A given string to be added to the dictionary.</param>
        private void AddStringToDictionary(IDictionary<string, int> dictionary, string str)
        {
            if (dictionary != null)
            {
                if (!dictionary.TryGetValue(str, out int value))
                {
                    dictionary.Add(str, 1);
                }
                else
                {
                    dictionary[str]++;
                }
            }
        }

        /// <summary>
        /// Sorts a dictionay first by its integer values in descending order and then by its string keys
        /// alphabetically.
        /// </summary>
        /// <param name="dictionary">Dicrionary to sort.</param>
        /// <returns>An ordered and enumerable collection of key/value pairs, in this case string/int pairs.</returns>
        private IOrderedEnumerable<KeyValuePair<string, int>> SortDictionary(IDictionary<string, int> dictionary)
        {
            return dictionary.OrderByDescending(p => p.Value)
                            .ThenBy(p => p.Key);
        }

        /// <summary>
        /// Creates an output in the desirable format.
        /// </summary>
        /// <param name="pairs">An ordered and enumerable collection of key/value pairs, in this case string/int pairs.</param>
        /// <param name="output">An object of a class that implements the IOutputResult interface.</param>
        private void CreateOutput(IOrderedEnumerable<KeyValuePair<string, int>> pairs, IOutputResult output)
        {
            foreach (var pair in pairs)
            {
                output.AddResult($"{pair.Key} - {pair.Value.ToString()}");
            }
        }

        #endregion

        #region Question Two

        public void RunQuestionTwo(ICharacterReader[] readers, IOutputResult output)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}