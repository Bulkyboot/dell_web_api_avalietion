using Dell.Lead.WeApi.Data.Converter.Converter;
using Dell.Lead.WeApi.Data.VO;
using Dell.Lead.WeApi.Models;
using Dell.Lead.WeApi.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dell.Lead.WeApi.Repositories.Implementation
{
    //
    public class ContributorsRepositoryImplementation : IContributorsRepository
    {
        private readonly SqlServerContext _context;
        private readonly ContributorsConverter _converter;

        public ContributorsRepositoryImplementation(SqlServerContext context)
        {
            _context = context;
            _converter = new ContributorsConverter();
        }

        public ContributorsRepositoryImplementation()
        {

        }

        public Contributors Create(Contributors contributors)
        {
            _context.Contributors.Add(contributors);
            _context.SaveChanges();

            return contributors;
        }

        public void Delete(long cpf)
        {
            var result = _context.Contributors.SingleOrDefault(e => e.Cpf.Equals(cpf));
            if (result != null)
            {
                try
                {
                    _context.Addresses.Remove(result.Address);
                    _context.Contributors.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<Contributors> FindAll()
        {
            return _context.Contributors.Include(a => a.Address).OrderBy(a => a.Name).ToList();
        }

        public Contributors FindByCpf(long cpf)
        {
            return _context.Contributors.Include(a => a.Address).Where(e => e.Cpf.Equals(cpf)).FirstOrDefault();
        }
        public Contributors Update(Contributors contributors)
        {
            if (Exists(contributors.Cpf))
            {
                var result = _context.Contributors.Include(a => a.Address).Where(e => e.Cpf.Equals(contributors.Cpf)).FirstOrDefault();
                if (result != null)
                {
                    try
                    {
                        _context.Entry(result).CurrentValues.SetValues(contributors);
                        _context.Entry(result.Address).CurrentValues.SetValues(contributors.Address);
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return result;
                }

            }
            return null;
        }

        public bool Exists(long cpf)
        {
            return _context.Contributors.Any(e => e.Cpf.Equals(cpf));
        }
    }
}