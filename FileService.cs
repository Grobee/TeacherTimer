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

        public async Task WriteJsonAsync(Work work)
        {
            var serializer = new DataContractJsonSerializer(typeof(Work));
            using(var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(JSONFILENAME, CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, work);
            }
        }

        public async Task<Work> ReadJsonAsync()
        {
            Work work;

            var serializer = new DataContractJsonSerializer(typeof(Work));
            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME))
                {
                    work = (Work)serializer.ReadObject(stream);
                }
                return work;
            }
            catch (FileNotFoundException)
            {
                return new Work()
                {
                    ElapsedHours = 0,
                    ElapsedMinutes = 0,
                    HoursDone = 0,
                    InProgress = false,
                    LongestStreak = 0,
                    StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)
                };
            }
        }
    }
}
