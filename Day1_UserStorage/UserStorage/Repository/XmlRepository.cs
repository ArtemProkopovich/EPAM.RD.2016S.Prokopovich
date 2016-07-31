using System;
using System.Collections.Generic;
using UserStorage.Interfacies;
using UserStorage.Entity;
using System.Xml.Serialization;
using System.IO;

namespace UserStorage.Repository
{
    [Serializable]
    public class XmlRepository : Repository<User>
    {
        public MemoryRepository Repository { get; private set; }
        public string FilePath { get; private set; }

        public XmlRepository(string filePath)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users);
        }

        public XmlRepository(string filePath, IEnumerable<int> idSequence)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users, idSequence);
        }

        public XmlRepository(string filePath, IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users, idSequence, validationFuncs);
        }

        public override Repository<User> Clone()
        {
            return new XmlRepository(FilePath, idSequence, validationFuncs);
        }

        public override void Delete(User user)
        {
            Repository.Delete(user);
        }

        public override IEnumerable<User> GetAll()
        {
            return Repository.GetAll();
        }

        public override User GetById(int id)
        {
            return Repository.GetById(id);
        }

        public override IEnumerable<User> SearchAll(Func<User, bool> searchCriteria)
        {
            return Repository.SearchAll(searchCriteria);
        }

        public override void Save()
        {
            SaveToXml();
        }

        protected override int AddItem(User item)
        {
            return Repository.Add(item);
        }

        private IEnumerable<User> LoadFromXml()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read);
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
                throw new RepositoryException("Error when loading a file", ex);
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
                fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(typeof(List<User>), new Type[] { typeof(User) });
                xs.Serialize(fs, Repository.GetAll());
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error when saving a file", ex);
            }
            finally
            {
                fs?.Dispose();
            }
        }

    }
}
