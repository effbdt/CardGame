using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IDataImportExportService
    {
        void JsonReader(string path);

        void ExportGameProcess(string playedCards);

        //void SubFolderCheck(Card card);
    }
}
