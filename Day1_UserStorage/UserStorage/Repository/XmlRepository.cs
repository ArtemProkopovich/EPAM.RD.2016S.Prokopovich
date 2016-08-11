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
        private MemoryRepository Repository { get; set; }
        private string FilePath { get; set; }

        /// <summary>
        /// Create xml repository
        /// </summary>
        /// <param name="filePath">Path to file that save state of repository</param>
        public XmlRepository(string filePath)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users);
        }

        /// <summary>
        /// Create xml repository
        /// </summary>
        /// <param name="filePath">Path to file that save state of repository</param>
        /// <param name="idSequence">Sequence for generating id's</param>
        public XmlRepository(string filePath, IEnumerable<int> idSequence)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users, idSequence);
        }

        /// <summary>
        /// Create xml repository
        /// </summary>
        /// <param name="filePath">Path to file that save state of repository</param>
        /// <param name="idSequence">Sequence for generating id's</param>
        /// <param name="validationFuncs">Functions for validation adding users</param>
        public XmlRepository(string filePath, IEnumerable<int> idSequence, params Func<User, bool>[] validationFuncs)
        {
            this.FilePath = filePath;
            var users = LoadFromXml();
            Repository = new MemoryRepository(users, idSequence, validationFuncs);
        }

        /// <summary>
        /// Clone state of repository
        /// </summary>
        /// <returns></returns>
        public override Repository<User> Clone()
        {
            return new XmlRepository(FilePath, idSequence, validationFuncs);
        }

        /// <summary>
        /// Delete item from repository
        /// </summary>
        /// <param name="user"></param>
        public override void Delete(User user)
        {
            Repository.Delete(user);
        }

        /// <summary>
        /// Search all elements in repository that fir to criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public override IEnumerable<User> SearchAll(Func<User, bool> searchCriteria)
        {
            return Repository.SearchAll(searchCriteria);
        }

        /// <summary>
        /// Save state of repository to xml file
        /// </summary>
        public override void Save()
        {
            SaveToXml();
        }

        /// <summary>
        /// Add item to repository
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override int AddItem(User item)
        {
            return Repository.Add(item);
        }

        /// <summary>
        /// Load state of repository to xml file
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Save state of repository to xml file
        /// </summary>
        public void SaveToXml()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                XmlSerializer xs = new XmlSerializer(typeof(List<User>), new Type[] { typeof(User) });
                xs.Serialize(fs, Repository.SearchAll(e => true));
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
