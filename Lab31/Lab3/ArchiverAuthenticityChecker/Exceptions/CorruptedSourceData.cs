using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiverAuthenticityChecker.Exceptions
{
    public class CorruptedSourceData:Exception
    {
        public CorruptedSourceData():base("Исходные данные повреждены!") 
        {

        }
    }
}
