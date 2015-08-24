using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.Converters
{
    public class TournamentConverter : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected Tournament Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("Type");
            switch (type)
            {
                case "Elimination":
                    return new EliminationTournament();
                case "PoolElimination":
                    return new PoolEliminationTournament();
                case "Pool":
                    return new PoolTournament();
                case "Round":
                    return new RoundTournament();
            }
            throw new ApplicationException("Type de tournoi non reconnu");
        }
        protected string GetType(Tournament tournoi)
        {
            if (tournoi is EliminationTournament)
                return "Elimination";
            if (tournoi is PoolEliminationTournament)
                return "PoolElimination";
            if (tournoi is PoolTournament)
                return "Pool";
            if (tournoi is RoundTournament)
                return "Round";
            
            throw new ApplicationException("Type de tournoi non reconnu");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Tournament).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            Tournament target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}