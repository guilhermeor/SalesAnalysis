using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAnalysis.Utils
{
    public interface ICacheManager<T>
    {
        void Set(string key, object data);
        void Set2(string key, object data);
        IEnumerable<string> GetAllKeys();
        IEnumerable<string> GetAllKeys2();
        Task<T> Get(string key);
        void Remove(string key);
        void Remove2(string key);
        bool Any();
        bool Any2();
    }
}
