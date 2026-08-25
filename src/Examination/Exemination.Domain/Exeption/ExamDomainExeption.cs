using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemination.Domain.Exeption
{
    public class ExamDomainExeption : Exception
    {
        public ExamDomainExeption()
        {
        }
        public ExamDomainExeption(string message)
            : base(message)
        {
        }
        public ExamDomainExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}