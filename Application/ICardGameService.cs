using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ICardGameService
    {
        ICardService CardService { get; }
        IDataImportExportService DataImportExportService { get; }
    }
    public class CardGameService : ICardGameService
    {
        public ICardService CardService { get; }

        public IDataImportExportService DataImportExportService { get; }

        public CardGameService(ICardService cardService, IDataImportExportService dataImportExportService)
        {
            CardService = cardService;
            DataImportExportService = dataImportExportService;
        }
    }
}
