using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Permutate<T> : List<T>
    {
        public IEnumerable<IEnumerable<T>> Get()
        {
            foreach(var res in Get(this))
            {
                yield return res;
            }
            yield break;
        }
        private IEnumerable<IEnumerable<T>> Get(IEnumerable<T> set, IEnumerable<T> subset = null) 
        {
            subset = subset ?? new T[] { };
            if(!set.Any())
            {
                yield return subset;
            }
            for (int i = 0; i < set.Count(); i++)
            {
                IEnumerable<T> newSubset = set.Take(i).Concat(set.Skip(i + 1));
                foreach(IEnumerable<T> permutation in Get(newSubset, subset.Concat(set.Skip(i).Take(1)))) {
                    yield return permutation;
                }
            }
        }
    }
}
