namespace WpfApp1
{
    using System;

    using Newtonsoft.Json;

    using WpfApp1.ViewModels;

    internal class ViewModelToIntConverter : JsonConverter<IntegerViewModel>
    {
        public override void WriteJson(JsonWriter writer, IntegerViewModel value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }

        public override IntegerViewModel ReadJson(JsonReader reader, Type objectType, IntegerViewModel existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return new IntegerViewModel()
            {
                Value = Convert.ToInt32(reader.Value)
            };
        }
    }
}