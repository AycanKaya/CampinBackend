using System.Text.Json;
using System.Text.Json.Serialization;

namespace CampinWebApi;

public static class JsonConverterExtensions
    {
        
        public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
        {
            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateOnly.Parse(reader.GetString()!);
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                var isoDate = value.ToString("O");
                writer.WriteStringValue(isoDate);
            }
        }   

        public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
        {
            private JsonConverter<TimeSpan> _jsonConverterImplementation;

            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var value = reader.GetString();
                return TimeSpan.Parse(value);
            }
            
            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
            
        }
        
        public class DateTimeToStringConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dateString = reader.GetString();
                var date = DateTime.Parse(dateString);

                return DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            }
            
            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                value = DateTime.SpecifyKind(value, DateTimeKind.Local);
                writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }
        }

        public class DateTimeNullableToStringConverter : JsonConverter<DateTime?>
        {
            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dateString = reader.GetString();
                var date = DateTime.Parse(dateString);

                return DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            }
            
            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    value = DateTime.SpecifyKind(value.Value, DateTimeKind.Local);
                    writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
    }