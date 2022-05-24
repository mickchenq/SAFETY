using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.ViewModel.BasicSet
{
    public class QueryLayer
    {
        public string LayerCode { get; set; }
        public int? ShelfId { get; set; }
        public byte? CurrentLayer { get; set; }
        public string IsStop { get; set; }

    }
}
