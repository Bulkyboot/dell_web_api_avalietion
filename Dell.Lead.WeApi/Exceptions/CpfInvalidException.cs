using System;

namespace Dell.Lead.WeApi.Exceptions
{
    //for discribe if this cpf exist
    public class CpfInvalidException : Exception
    {
        public CpfInvalidException(string messeger) : base(messeger) { }
        public CpfInvalidException() : base("invalid CPF") { }
    }
}