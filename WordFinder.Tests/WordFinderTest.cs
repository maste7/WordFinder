using WordFinder.Services.Services;

namespace WordFinder.Tests
{

    public class WordFinderTest
    {
        [Fact]
        public void WordFinder_MatrixContructorErrors_ReturnExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new WordFinderService(null));

            List<string> matrix = new List<string>();
            Assert.Equal("Matrix can't be Empty", Assert.Throws<ArgumentException>(() => { _ = new WordFinderService(matrix); }).Message);

            matrix = new List<string>() {
                "abcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdcabcdc",
                "fgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwiofgwio",
                "chillchillchillchillchillchillchillchillchillchillchillchillchillchill",
                "pqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsdpqnsd",
                "uvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxyuvdxy"
            };
            Assert.StartsWith("Matrix Columns can't be more than", Assert.Throws<ArgumentException>(() => { _ = new WordFinderService(matrix); }).Message);

            matrix = new List<string>() {
                "abcdc",
                "fgwios",
                "chill",
                "pqns",
                "uvdxy"
            };
            Assert.Equal("All Matrix Rows have not the same length", Assert.Throws<ArgumentException>(() => { _ = new WordFinderService(matrix); }).Message);

            matrix = new List<string>() {
                "abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc","abcdc",
                "fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios","fgwios",
                "chill","chill","chill","chill","chill","chill","chill","chill","chill","chill","chill","chill","chill",
                "pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns","pqns",
                "uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy","uvdxy"
            };
            Assert.StartsWith("Matrix Rows can't be more than", Assert.Throws<ArgumentException>(() => { _ = new WordFinderService(matrix); }).Message);

        }

        [Fact]
        public void WordFinder_MatrixContructorValid_MatrixCreated()
        {
            List<string> matrixParameter = new List<string>()
            {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy"
            };

            var matrix = new WordFinderService(matrixParameter);
            Assert.NotNull(matrix);
        }

        [Fact]
        public void WordFinder_FindWordstream_ReturnExceptions()
        {
            List<string> matrixParameter = new List<string>() { "abcdc", "fgwio" };
            var matrix = new WordFinderService(matrixParameter);

            Assert.Throws<ArgumentNullException>(() => matrix.Find(null));
            Assert.Equal("Wordstream can't be Empty", Assert.Throws<ArgumentException>(() => { _ = matrix.Find(new List<string>()); }).Message);
        }

        [Fact]
        public void WordFinder_FindWordstream_ReturnValidList()
        {

            List<string> matrixParameter = new List<string>()
            {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy"
            };

            var matrix = new WordFinderService(matrixParameter);
            List<string> wordStream = ["cold", "wind", "snow", "cold", "chill"];
            List<string> expectedWords = ["cold", "wind", "chill"];
            List<string> unexpectedWords = ["snow"];

            var result = matrix.Find(wordStream);

            Assert.True(result.Intersect(expectedWords).Count() == expectedWords.Count);
            Assert.True(result.Intersect(unexpectedWords).Count() == 0);

        }

        [Fact]
        public void WordFinder_FindWordstream_ReturnValidTop10List()
        {

            List<string> matrixParameter = new List<string>()
            {
                "catcatcatcatcatcat",
                "collcollcollcollco",
                "chillchillchillchi",
                "windwindwindwindwi",
                "nownownownownownow",
                "hiooccccclllllllll"
            };

            var matrix = new WordFinderService(matrixParameter);
            List<string> wordStream =
                ["cat", //6 times
                "coll", //4 times
                "wind", //4 times
                "now",  //6 times
                "chill",//3 times 
                "ccc", //2 time
                "hill", //3 times
                "atc", //5 times
                "hio", //2 time
                "lin", //1 time
                "own"];//5times


            List<string> expectedWords = ["cat", "coll", "wind", "now", "chill", "ccc", "hill", "atc", "hio", "own"];
            List<string> unexpectedWords = ["lin"];

            var result = matrix.Find(wordStream);

            Assert.True(result.Intersect(expectedWords).Count() == expectedWords.Count);
            Assert.True(result.Intersect(unexpectedWords).Count() == 0);

        }
    }
}