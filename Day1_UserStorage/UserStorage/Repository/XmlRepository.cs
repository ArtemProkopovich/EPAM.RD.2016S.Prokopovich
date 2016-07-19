using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;
using System.Xml.Serialization;
using System.IO;

namespace UserStorage.Repository
{
    [Serializable]
    public class XmlRepository : Repository<User>
    {
        public MemoryRepository repository { get; private set; }
        public string filePath { get; private set; }

        public XmlRepository(string filePath)
        {
            this.filePath = filePath;
            var users = LoadFromXml();
            repository = new MemoryRepository(users);
        }

        public XmlRepository(string filePath, IEnumerable<int> idSequence)
        {
            this.filePath = filePath;
            var users = LoadFromXml();
            repository = new MemoryRepository(users, idSequence);
        }

        public XmlRepository(string filePath, IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs)
        {
            this.filePath = filePath;
            var users = LoadFromXml();
            repository = new MemoryRepository(users, idSequence, validationFuncs);
        }

        public override Repository<User> Clone()
        {
            return new XmlRepository(filePath, idSequence, validationFuncs);
        }

        public override void Delete(User user)
        {
            repository.Delete(user);
        }

        public override IEnumerable<User> GetAll()
        {
            return repository.GetAll();
        }

        public override User GetById(int id)
        {
            return repository.GetById(id);
        }

        public override IEnumerable<User> SearchAll(params Func<User, bool>[] criterias)
        {
            return repository.SearchAll(criterias);
        }

        public override IEnumerable<User> SearchAll(Func<User, bool> criteria)
        {
            return repository.SearchAll(criteria);
        }

        public override User SearchFirst(Func<User, bool> criteria)
        {
            return repository.SearchFirst(criteria);
        }

        public override void Save()
        {
            SaveToXml();
        }

        protected override int AddItem(User item)
        {
            return repository.Add(item);
        }

        private IEnumerable<User> LoadFromXml()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
                if (fs.Position != fs.Length)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<User>), new Type[] { typeof(User) });
                    IEnumerable<User> result = (List<User>)xs.Deserialize(fs);
                    return result;
                }
                return new List<User>();
            }
            catch (Exception ex)
            {
                throw new RepositoryException();
            }
            finally
            {
                fs?.Dispose();
            }
        }


        public void SaveToXml()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(typeof(List<User>), new Type[] { typeof(User) });
                xs.Serialize(fs, repository.GetAll());
            }
            catch (Exception ex)
            {
                throw new RepositoryException();
            }
            finally
            {
                fs?.Dispose();
            }
        }

    }
}
