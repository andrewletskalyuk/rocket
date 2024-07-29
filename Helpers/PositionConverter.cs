using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Armstrong.Core.Services.CurveDrawing.Models.CurveSeries;

namespace Armstrong.Core.Services.CurveDrawing.Helpers
{
    public class PositionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var positions = value as IList<Position>;
            if (positions != null)
            {
                writer.WriteStartArray();
                foreach (var position in positions)
                    WritePosition(writer, position);
                writer.WriteEndArray();
            }
            else
            {
                WritePosition(writer, value as Position);
            }
        }

        private static void WritePosition(JsonWriter writer, Position position)
        {
            if (position != null)
            {
                writer.WriteStartArray();
                writer.WriteValue(position.X);
                writer.WriteValue(position.Y);
                writer.WriteEndArray();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var position = serializer.Deserialize<List<double>>(reader);
            if (position.Count == 0)
                return new Position();
            return new Position { X = position[0], Y = position[1] };
        }

        public new bool CanRead = true; //Amended for the TC build to compile
        
        
        public override bool CanConvert(Type objectType)
        {
            // Converting to avoid Team City error
            if (objectType == typeof(Position))
            {
                return true;
            }
            return false;
        }

    }
}