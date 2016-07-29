using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UserStorage.Service;
using UserStorage.Entity;
using System.Text.RegularExpressions;

namespace UserStorage.Serialization
{
    public class JsonSerializer : ISerializer<ServiceMessage>
    {
        public ServiceMessage DeserializeObject(Stream stream)
        {
            string str = "";
            using (var memoryStream = new MemoryStream())
            {
                //stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(memoryStream);
                str = Encoding.Unicode.GetString(memoryStream.ToArray());
            }
            var result = new ServiceMessage();
            str = str.Trim('{', '}', ' ');
            result.Operation = ReadOperation(str);
            result.user = ReadUser(str);
            return result;
        }

        public void SerializeObject(ServiceMessage obj, Stream stream)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(WriteMessage(obj));
            stream.Write(buffer, 0, buffer.Length);
        }

        #region ReadMethods

        private Operation ReadOperation(string str)
        {
            int index = str.IndexOf(',');
            string valueString = str.Substring(0, index);
            return (Operation)int.Parse(ReadValue(valueString));
        }

        private User ReadUser(string str)
        {
            var array = str.Split(',', '{');
            User result = new User();
            foreach(var s in array)
            {
                if (s.IndexOf("FirstName") > 0)
                    result.FirstName = ReadValue(s);
                if (s.IndexOf("LastName") > 0)
                    result.LastName = ReadValue(s);
                if (s.IndexOf("Id") > 0 && s.IndexOf("PersonalId") < 0)
                    result.Id = int.Parse(ReadValue(s));
                if (s.IndexOf("Gender") > 0)
                    result.Gender = (Gender)int.Parse(ReadValue(s));
                if (s.IndexOf("PersonalId") > 0)
                    result.PersonalId = ReadValue(s);
                if (s.IndexOf("BirthDate") > 0)
                    result.BirthDate = Convert.ToDateTime(ReadValue(s));                
            }
            result.Visas = ReadVisas(str);
            return result;
        }

        private Visa[] ReadVisas(string str)
        {
            List<Visa> visas = new List<Visa>();
            int startIndex = str.IndexOf('[');
            if (startIndex > 0)
            {
                int finIndex = str.IndexOf(']');
                string value = str.Substring(startIndex, finIndex - startIndex).Trim(' ', '[', ']');
                var array = value.Split('}');
                foreach(string match in array)
                {
                    if(!string.IsNullOrWhiteSpace(match))
                        visas.Add(ReadVisa(match.Trim('{', '}', ' ')));
                }
            }
            return visas.ToArray();
        }

        private Visa ReadVisa(string str)
        {
            var array = str.Split(',');
            Visa result = new Visa();
            foreach (var s in array)
            {
                if (s.IndexOf("Country") > 0)
                    result.Country = ReadValue(s);
                if (s.IndexOf("StartTime") > 0)
                    result.StartTime = Convert.ToDateTime(ReadValue(s));
                if (s.IndexOf("EndTime") > 0)
                    result.EndTime = Convert.ToDateTime(ReadValue(s));
            }
            return result;
        }

        private string ReadValue(string str)
        {
            int startIndex = str.IndexOf(":");
            string value = str.Substring(startIndex + 1).Trim('"', ' ', ',');
            return value;
        }

        #endregion

        #region WriteMethods
        private string WriteVisaArray(Visa[] array)
        {
            StringBuilder result = new StringBuilder();
            result.Append("\"Visas:\"[");
            foreach (var visa in array)
            {
                result.Append(WriteVisa(visa)).Append(",");
            }
            result.Remove(result.Length - 1, 1);
            result.Append("]");
            return result.ToString();
        }

        private string WriteValue(string name, string value)
        {
            return $"\"{name}\":\"{value}\"";
        }

        private string WriteVisa(Visa visa)
        {
            return $"{{\"StartTime\":\"{visa.StartTime}\",\"EndTime\":\"{visa.EndTime}\",\"Country\":\"{visa.Country}\"}}";
        }

        private string WriteUser(User user)
        {
            StringBuilder result = new StringBuilder();
            result.Append("\"User\":{");
            result.Append(WriteValue("Id", user.Id.ToString())).Append(",");
            if (user.BirthDate != null)
                result.Append(WriteValue("BirthDate", user.BirthDate.ToString())).Append(",");
            if(user.FirstName!=null)
                result.Append(WriteValue("FirstName", user.FirstName)).Append(",");
            if(user.LastName!=null)
                result.Append(WriteValue("LastName", user.LastName)).Append(",");
            if(user.Gender!=null)
                result.Append(WriteValue("Gender", ((int)user.Gender).ToString())).Append(",");
            if (user.PersonalId != null)
                result.Append(WriteValue("PersonalId", user.PersonalId)).Append(",");
            if (user.Visas != null && user.Visas.Length > 0)
                result.Append(WriteVisaArray(user.Visas));
            result.Append("}");
            return result.ToString();
        }

        private string WriteMessage(ServiceMessage msg)
        {
            StringBuilder result = new StringBuilder();
            result.Append("{");
            result.Append(WriteValue("Operation", ((int)msg.Operation).ToString())).Append(",");
            result.Append(WriteUser(msg.user));
            result.Append("}");
            return result.ToString();
        }
    }
#endregion
}
