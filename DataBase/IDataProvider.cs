using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IDataProvider
    {
        ICardDataProvider CardDataProvider { get; }
    }

    public class DataProvider : IDataProvider
    {
        public ICardDataProvider CardDataProvider { get; }

        public DataProvider(ICardDataProvider cardDataProvider)
        {
            CardDataProvider = cardDataProvider;
        }
    }
}