using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordFinder.Models.DTO;

namespace WordFinder.Services.Interfaces
{
    public interface IWordFinder
    {
        public IEnumerable<string> Find(IEnumerable<string> wordstream);
    }
}
