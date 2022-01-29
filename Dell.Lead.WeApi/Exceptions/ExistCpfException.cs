using System;

namespace Dell.Lead.WeApi.Exceptions
{
    //for discribe if this cpf exist
    public class ExistCpfException : Exception
    {
        public ExistCpfException(string messeger) : base(messeger) { }
    }
}