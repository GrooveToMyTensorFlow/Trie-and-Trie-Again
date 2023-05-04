# Trie, Trie Again

This project focuses on implementing and extending trie data structures, specifically r-way trie and ternary search tree (TST) trie, to support common operations related to text processing such as partial matching, autocompletion, and autocorrection. The project uses the 1000 most common words in the English language to populate each trie.

## Data Structures

### R-Way Trie

An r-way trie is a tree-like data structure that is used to store a collection of strings. Each node in the trie represents a character of a string and has r child nodes, where r is the size of the alphabet. In this project, we assume that the input strings consist of lowercase English letters, so r = 26.

### Ternary Search Tree (TST) Trie

A TST trie is another tree-like data structure used for storing strings. It differs from r-way trie in its structure: each node has only 3 child nodes – one for characters less than the current character, one for characters equal to the current character, and one for characters greater than the current character.

## Methods

1. `List<string> PartialMatch(string pattern)`: Return all keys that match the given pattern. A pattern is defined as a string with letters and stars where each star represents a wildcard character.

2. `List<string> Autocomplete(string prefix)`: Return the list of keys that begin with the given prefix.

3. `List<string> Autocorrect(string key)`: Return the list of keys that differ from the given key by one letter. Try to be as efficient as possible.

## Requirements

1. Populate each Trie with the 1000 most common keys (words) in the English language.
   - Using the link below, cut and paste the 1000 keys into a file: [Top 1000 Words](https://www.ef.com/ca/english-resources/english-vocabulary/top-1000-words/)
   - Input each key from the file, convert it to lower case, and insert it into each Trie. Assume the value associated with a key is a random integer.

2. Implement the three methods above for each implementation of the Trie class.

3. Thoroughly test the three methods.

