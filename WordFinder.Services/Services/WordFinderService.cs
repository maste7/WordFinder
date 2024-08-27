using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordFinder.Models.DTO;
using WordFinder.Services.Interfaces;

namespace WordFinder.Services.Services
{
    public class WordFinderService : IWordFinder
    {
        private static readonly int MatrixMaxRows = 64;
        private static readonly int MatrixMaxColumns = 64;
        private static readonly int MaxResults = 10;

        private readonly char[,] _matrix;
        private readonly int _matrixRows;
        private readonly int _matrixColumns;

        /// <summary>
        /// WordFinderService Constructor. Receive rows and columns to generate a Matrix. Make validations on 
        /// matrix parameter to avoid create an invalid one.
        /// </summary>
        /// <param name="matrix">List of strings to create matrix</param>
        /// <exception cref="ArgumentNullException">Throw nif matrix parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown if matrix parameter is empty</exception>
        /// <exception cref="ArgumentException">Thrown if the rows of the matrix exceed the maximum allowed</exception>
        /// <exception cref="ArgumentException">Thrown if the rows of the matrix do not have the same length.</exception>
        /// <exception cref="ArgumentException">Thrown if the columns of the matrix exceed the maximum allowed</exception>
        public WordFinderService(IEnumerable<string> matrix)
        {

            //Validate matrix parameter is not null
            matrix = matrix ?? throw new ArgumentNullException();

            //Validate matrix parameter is not empty
            if (!matrix.Any()) throw new ArgumentException("Matrix can't be Empty");

            //Validate matrix rows do not exceed the maximun allowed
            if (matrix.Count() > MatrixMaxRows) throw new ArgumentException($"Matrix Rows can't be more than {MatrixMaxRows}");

            //Validate that all the rows of the matrix have the same length
            if (matrix.Select(x => x.Length).Distinct().Count() > 1) throw new ArgumentException($"All Matrix Rows have not the same length");

            //Validate matrix columns do not exceed the maximun allowed
            if (matrix.First().Trim().Length > MatrixMaxColumns) throw new ArgumentException($"Matrix Columns can't be more than {MatrixMaxColumns}");

            //Save matrix Rows and Columns 
            this._matrixRows = matrix.Count();
            this._matrixColumns = matrix.First().Trim().Length;

            //Create Matrix
            this._matrix = new char[this._matrixRows, this._matrixColumns];

            //Complete Matrix characters
            
            int rowIndex = 0;
            foreach (var row in matrix)
            {
                int colIndex = 0;

                foreach (var colChar in row.Trim())
                {
                    this._matrix[rowIndex, colIndex] = char.ToLower(colChar);
                    colIndex++;
                }

                rowIndex++;
            }
        }

        /// <summary>
        /// Method to find words from wordstrem inside the matrix. 
        /// </summary>
        /// <param name="wordstream">Words to search on matrix</param>
        /// <returns>Returns a top list of words found on matrix. Retreive them order by occurrences</returns>
        /// <exception cref="ArgumentNullException">Thrown argument null exception if wordstream parameter is null</exception>
        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            //Validate wordstream parameter is not null
            wordstream = wordstream ?? throw new ArgumentNullException();

            //Validate wordstream parameter is not empty
            if (!wordstream.Any()) throw new ArgumentException("Wordstream can't be Empty");

            var wordFinderResults = new List<WordFinderResult>();
            
            //Iterate all words in wordstream
            foreach (var word in wordstream)
            {
                //If word is null or empty, we skip it
                if (string.IsNullOrEmpty(word)) 
                    continue;

                var wordLower = word.ToLower();

                //If word was proccess early, we don't search it again
                if (wordFinderResults.Any(x => x.Word == wordLower))
                    continue;

                wordFinderResults.Add(new WordFinderResult()
                {
                    Word = wordLower,
                    Occurrencies = FindVerticalWordOccurrences(wordLower) + FindHorizontalWordOccurrences(wordLower)
                });
            }

            //Return a top of founded words in matrix
            return wordFinderResults
                    .Where(x => x.Occurrencies > 0)
                    .OrderByDescending(x => x.Occurrencies)
                    .Select(x => x.Word)
                    .Take(MaxResults);
        }

        /// <summary>
        /// This method call find word ocurrences to search vertical
        /// </summary>
        /// <param name="word">Search word</param>
        /// <returns>Number of occurences of word on vertical search</returns>
        private int FindVerticalWordOccurrences(string word)
        {
            return FindWordOccurrences(word, false);
        }

        /// <summary>
        /// This method call find word ocurrences to search horizontal
        /// </summary>
        /// <param name="word">Search word</param>
        /// <returns>Number of occurences of word on horizontal search</returns>
        private int FindHorizontalWordOccurrences(string word)
        {
            return FindWordOccurrences(word, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word">Search word</param>
        /// <param name="isHorizontalSearch">Flag to validate if we want to search horizontal or vertical</param>
        /// <returns>Number or occurences of word on search</returns>
        private int FindWordOccurrences(string word, bool isHorizontalSearch)
        {
            //Set rows and cols based on isHorizontalSearch flag
            var rows = isHorizontalSearch ? this._matrixRows : this._matrixColumns;
            var cols = isHorizontalSearch ? this._matrixColumns : this._matrixRows;

            if (word.Length > rows)
                return 0;

            int maxIterations = rows - word.Length;
            int wordOccurrences = 0;

            //Iterate all the columns
            for (var columnIndex = 0; columnIndex < cols; columnIndex++)
            {
                //Iterate all the rows
                for (var rowIndex = 0; rowIndex <= maxIterations; rowIndex++)
                {
                    int charPos = 0;
                    string foundWord = string.Empty;

                    //Iterate word to process all chars 
                    foreach (char charWord in word)
                    {
                        //If char isn't on matrix position, we break the iteration
                        if (!IsInMatrixPosition(charWord, rowIndex + charPos, columnIndex, isHorizontalSearch))
                            break;

                        foundWord += charWord;
                        charPos++;
                    }

                    //If foundWord is equal to search word, add 1 to wordOccurrences
                    if (foundWord == word)
                        wordOccurrences++;
                }
            }

            return wordOccurrences;
        }
        
        /// <summary>
        /// This method compares the searched char with the char of the matrix in the sent position.
        /// </summary>
        /// <param name="charWord">Searched char</param>
        /// <param name="rowPos">Row index on matrix</param>
        /// <param name="columnPos">Column index on matrix</param>
        /// <param name="isHorizontalSearch">Flag to validate if we want to search horizontal or vertical</param>
        /// <returns>True if chars are equal</returns>
        private bool IsInMatrixPosition(char charWord, int rowPos, int columnPos, bool isHorizontalSearch)
        {
            return isHorizontalSearch ? this._matrix[rowPos, columnPos] == charWord : this._matrix[columnPos, rowPos] == charWord;
        }
    }
}
