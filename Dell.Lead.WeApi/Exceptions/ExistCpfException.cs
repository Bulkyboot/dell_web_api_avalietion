using System;

namespace Dell.Lead.WeApi.Exceptions
{
    //existCpf
    public class ExistCpfException : Exception
    {
        public ExistCpfException(string messeger) : base(messeger) { }
    }
}