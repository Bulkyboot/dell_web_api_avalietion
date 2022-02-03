using Dell.Lead.WeApi.Data.Converter.Converter;
using Dell.Lead.WeApi.Data.VO;
using Dell.Lead.WeApi.Exceptions;
using Dell.Lead.WeApi.Repositories;
using Dell.Lead.WeApi.Util;
using System;
using System.Collections.Generic;

namespace Dell.Lead.WeApi.Business.Implementation
{
    //'
    public class ContributorsBusinessImplementation : IContributorsBusiness
    {
        private readonly IContributorsRepository _Repository;
        private readonly ContributorsConverter _converter;

        public ContributorsBusinessImplementation(IContributorsRepository Repository)
        {
            _Repository = Repository;
            _converter = new ContributorsConverter();
        }
        public ContributorsVO Create(ContributorsVO contributors)
        {
            var contributorsEntity = _converter.Parse(contributors);
            contributorsEntity = _Repository.Create(contributorsEntity);
            return _converter.Parse(contributorsEntity);
            if (contributorsEntity == null)
            {
                Console.WriteLine("This item is null");
            }
            else
            {
                Console.WriteLine("This item has already been registered");
            }
        }

        public void Delete(long cpf)
        {
            _Repository.Delete(cpf);
        }

        public List<ContributorsVO> FindAll()
        {
            return _converter.Parse(_Repository.FindAll());
        }

        public ContributorsVO FindByCpf(long cpf)
        { 
            var findEmployee = _Repository.FindByCpf(cpf);
            if (findEmployee == null) throw new ExistCpfException("don't exist contributors in CPF");
            return _converter.Parse(findEmployee);
        }
        public ContributorsVO Update(ContributorsVO contributorsVO)
        {
            var contributors = _converter.Parse(contributorsVO);
            contributors.AddressId = contributorsVO.Address.Id;
            if (FindByCpf(contributors.Cpf) != null)
            {
                contributors = _Repository.Update(contributors);
                return _converter.Parse(contributors);
            }
            return null;
        }
    }
}
