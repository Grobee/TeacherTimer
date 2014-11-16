using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TeacherTimer
{
    class FileService    
    {
        const string JSONFILENAME = "data.json";        

        public async Task WriteJsonAsync(Session work)
        {
            var serializer = new DataContractJsonSerializer(typeof(Session));
            using(var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(JSONFILENAME, CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, work);
            }
        }

        public async Task<Session> ReadJsonAsync()
        {
            Session session;

            var serializer = new DataContractJsonSerializer(typeof(Session));
            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME))
                {
                    session = (Session)serializer.ReadObject(stream);
                }
                return session;
            }
            catch (FileNotFoundException)
            {
                return new Session()
                {
                    HoursDone = 0,
                    ElapsedHours = 0,
                    ElapsedMinutes = 0,
                    PreviousElapsedHours = 0,
                    InProgress = false,
                    LongestStreak = 0,
                    StartTime = DateTime.MinValue
                };
            }
        }
    }
}
