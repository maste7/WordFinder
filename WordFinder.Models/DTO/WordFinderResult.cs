using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFinder.Models.DTO
{
    public class WordFinderResult
    {
        /// <summary>
        /// Search word
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Occurrencies on matrix
        /// </summary>
        public int Occurrencies { get; set; }
    }
}
