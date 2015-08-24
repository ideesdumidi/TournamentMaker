using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TournamentMaker.Models;

namespace TournamentMaker.Converters
{
    public class TournamentModelConverter : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected TournamentModel Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("Type");
            switch (type)
            {
                case "Elimination":
                    return new EliminationTournamentModel();
                case "PoolElimination":
                    return new PoolEliminationTournamentModel();
                case "Pool":
                    return new PoolTournamentModel();
                case "Round":
                    return new RoundTournamentModel();
            }
            throw new ApplicationException("Type de tournoi non reconnu");
        }
        protected string GetType(TournamentModel tournoi)
        {
            if (tournoi is EliminationTournamentModel)
                return "Elimination";
            if (tournoi is PoolEliminationTournamentModel)
                return "PoolElimination";
            if (tournoi is PoolTournamentModel)
                return "Pool";
            if (tournoi is RoundTournamentModel)
                return "Round";
            
            throw new ApplicationException("Type de tournoi non reconnu");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TournamentModel).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            TournamentModel target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ((TournamentModel)value).Type = GetType((TournamentModel)value);
            serializer.Serialize(writer, value);
        }
    }
}