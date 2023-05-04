//Dharam Raj Patel
//Rocky Sharaf
//Amber
//Assignment 2a

using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrieTernaryTree
{
    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ITrie<T> : IContainer<T>
    {
        bool Insert(string key, T value);
        T Value(string key);
        List<string> PartialMatch(string pattern);
        List<string> Autocomplete(string prefix);
        List<string> Autocorrect(string key);
    }

    //-------------------------------------------------------------------------

    class Trie<T> : ITrie<T>
    {
        private Node root;                 // Root node of the Trie
        private int size;                  // Number of values in the Trie

        class Node
        {
            public char ch;                // Character of the key
            public T value;                // Value at Node; otherwise default
            public Node low, middle, high; // Left, middle, and right subtrees

            // Node
            // Creates an empty Node
            // All children are set to null
            // Time complexity:  O(1)

            public Node(char ch)
            {
                this.ch = ch;
                value = default(T);
                low = middle = high = null;
            }
        }

        // Trie
        // Creates an empty Trie
        // Time complexity:  O(1)

        public Trie()
        {
            size = 0;
        }

        // Public Insert
        // Calls the private Insert which carries out the actual insertion
        // Returns true if successful; false otherwise

        public bool Insert(string key, T value)
        {
            return Insert(ref root, key, 0, value);
        }

        // Private Insert
        // Inserts the key/value pair into the Trie
        // Returns true if the insertion was successful; false otherwise
        // Note: Duplicate keys are ignored
        // Time complexity:  O(n+L) where n is the number of nodes and 
        //                                L is the length of the given key

        private bool Insert(ref Node p, string key, int i, T value)
        {
            if (p == null)
                p = new Node(key[i]);  //key[i] is the character in position 'i'

            // Current character of key inserted in left subtree
            if (key[i] < p.ch)
                return Insert(ref p.low, key, i, value);

            // Current character of key inserted in right subtree
            else if (key[i] > p.ch)
                return Insert(ref p.high, key, i, value);

            else if (i + 1 == key.Length)
            // Key found
            {
                // But key/value pair already exists
                if (!p.value.Equals(default(T)))
                    return false;
                else
                {
                    // Place value in node
                    p.value = value;
                    size++;
                    return true;
                }
            }

            else
                // Next character of key inserted in middle subtree
                return Insert(ref p.middle, key, i + 1, value);
        }

        // Value
        // Returns the value associated with a key; otherwise default
        // Time complexity:  O(d) where d is the depth of the trie

        public T Value(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return the value if all characters of the key have been visited 
                    if (++i == key.Length)
                        return p.value;

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return default(T);   // Key too long
        }

        // Contains
        // Returns true if the given key is found in the Trie; false otherwise
        // Time complexity:  O(d) where d is the depth of the trie

        public bool Contains(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return true if the key is associated with a non-default value; false otherwise 
                    if (++i == key.Length)
                        return !p.value.Equals(default(T));

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return false;        // Key too long
        }

        // PartialMatch
        // Returns a list of all keys that match the given pattern, where a pattern is a string with letters and stars 
        // where each star represents a wildcard character
        // Time complexity:  O(n* L) where n is the number of nodes and L is the length of the given pattern
        public List<string> PartialMatch(string pattern)
        {
            List<string> matches = new List<string>();
            PartialMatch(root, pattern, "", matches);
            return matches;
        }

        private void PartialMatch(Node p, string pattern, string word, List<string> matches)
        {
            if (p == null) return;

            if (pattern.Length == 0)
            {
                // If we've reached the end of the pattern, add the key if it's a match and it has the same length as the input pattern
                if (!p.value.Equals(default(T)) && word.Length == pattern.Length)
                {
                    matches.Add(word);
                }
                return;
            }

            char c = pattern[0];
            string rest = pattern.Substring(1);

            if (c == '*')
            {
                // If the current character is a wildcard, explore all possible paths
                // by recursively calling PartialMatch with the rest of the pattern
                PartialMatch(p.low, rest, word, matches);
                PartialMatch(p.middle, rest, word + p.ch, matches);
                PartialMatch(p.high, rest, word, matches);
            }
            else if (c < p.ch)
            {
                // If the current character is less than the node's character, explore the left subtree
                PartialMatch(p.low, pattern, word, matches);
            }
            else if (c > p.ch)
            {
                // If the current character is greater than the node's character, explore the right subtree
                PartialMatch(p.high, pattern, word, matches);
            }
            else // c == p.ch
            {
                // If the current character matches the node's character, explore the middle subtree
                PartialMatch(p.middle, rest, word + p.ch, matches);
            }
        }






        // Autocomplete
        // Returns a list of all keys that begin with the given prefix
        // Time complexity:  O(n* L) where n is the number of nodes and L is the length of the given prefix

        public List<string> Autocomplete(string prefix)
        {
            List<string> matches = new List<string>();
            Autocomplete(root, prefix, "", matches);
            return matches;
        }
        // Private Autocomplete
        // Recursively traverses the Trie to find keys that begin with the given prefix
        // Time complexity:  O(n* L) where n is the number of nodes and L is the length of the given prefix

        private void Autocomplete(Node p, string prefix, string word, List<string> matches)
        {
            if (p == null) return;

            // If there are no more characters in the prefix, add all keys in the subtree
            if (prefix.Length == 0)
            {
                if (!p.value.Equals(default(T))) matches.Add(word + p.ch);
                Autocomplete(p.low, prefix, word, matches);
                Autocomplete(p.middle, prefix, word + p.ch, matches);
                Autocomplete(p.high, prefix, word, matches);
                return;
            }

            char c = prefix[0];

            if (c < p.ch)
                Autocomplete(p.low, prefix, word, matches);
            else if (c > p.ch)
                Autocomplete(p.high, prefix, word, matches);
            else
                Autocomplete(p.middle, prefix.Substring(1), word + p.ch, matches);
        }

        // Autocorrect
        // Returns a list of all keys that differ from the given key by one letter
        // Time complexity:  O(k* L) where k is the size of the alphabet and L is the length of the given key

        //Autocorrect
        public List<string> Autocorrect(string key)
        {
            List<string> suggestions = new List<string>();
            for (int i = 1; i < key.Length; i++)
            {
                string prefix = key.Substring(0, i);
                string suffix = key.Substring(i + 1);
                char c = key[i];
                foreach (string s in Autocomplete(prefix))
                {
                    for (int j = 0; j < s.Length; j++)
                    {
                        if (s[j] != c)
                        {
                            string corrected = prefix + s[j] + suffix;
                            if (Contains(corrected))
                                suggestions.Add(corrected);
                        }
                    }
                }
            }
            return suggestions;
        }


        void IContainer<T>.MakeEmpty()
        {
            throw new NotImplementedException();
        }

        bool IContainer<T>.Empty()
        {
            throw new NotImplementedException();
        }

        int IContainer<T>.Size()
        {
            throw new NotImplementedException();
        }
    }

        class Program
        {
            static void Main(string[] args)
            {

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] words = System.IO.File.ReadAllLines(@"C:\Users\Admin\Documents\most-common-words.txt");
            int wordNum = 0;
            foreach (string aWord in words)
            {
                ++wordNum;
            }
            // Create a Trie and insert the 1000 most common English words
            Trie<int> trie = new Trie<int>();



            foreach (string word in words)
            {
                trie.Insert(word, new Random().Next(100)); // add a random value
            }

            // Test the Autocomplete method
            List<string> autocomplete = trie.Autocomplete("th");
            Console.WriteLine("Autocomplete(th):");
            foreach (string match in autocomplete)
            {
                Console.WriteLine(match);
            }
            Console.WriteLine();



        }
    }
    
}



