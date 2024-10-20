using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiverAuthenticityChecker.Exceptions
{
    public class ComputerMismatchException:Exception
    {
        public ComputerMismatchException() : base("Компьютер не соответвует исходному компьютеру!")
        {

        }
    }
}
