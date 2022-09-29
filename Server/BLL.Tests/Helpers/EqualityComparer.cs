using System.Diagnostics.CodeAnalysis;
using BLL.Models;

namespace BLL.Tests.Helpers
{
    internal class GameEqualityComparer : IEqualityComparer<GameModel>
    {
        public bool Equals([AllowNull] GameModel x, [AllowNull] GameModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.Id == y.Id && x.Title == y.Title
                && x.Description == y.Description && x.Price == y.Price;
        }

        public int GetHashCode([DisallowNull] GameModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class GameListEqualityComparer : IEqualityComparer<List<GameModel>>
    {
        public bool Equals([AllowNull] List<GameModel> x, [AllowNull] List<GameModel> y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null || x.Count != y.Count)
                return false;
            bool check = false;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].Id == y[i].Id && x[i].Title == y[i].Title
                    && x[i].Description == y[i].Description && x[i].Price == y[i].Price
                    && x.GetType() == y.GetType() && x.Count == y.Count)
                {
                    check = true;
                }
                else
                {
                    return false;
                }
            }

            return check;
        }

        public int GetHashCode([DisallowNull] List<GameModel> obj)
        {
            return obj.GetHashCode();
        }
    }

}
