using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using NLog;

namespace CMD.Appointment.Domain.Services
{
    /// <summary>
    /// Provides implementation for retrieving messages from an XML file based on a given key.
    /// </summary>
    public class MessageService : IMessageService
    {

        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, string> _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// Loads messages from the specified XML file.
        /// </summary>
        /// <param name="configuration">The configuration object used to initialize the service.</param>
        public MessageService(IConfiguration configuration)
        {
            // Load messages from XML file
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var xmlFilePath = Path.Combine(assemblyLocation, "Resources", "AppointmentCustomMessage.xml");
            _messages = LoadMessages(xmlFilePath);
        }

        /// <summary>
        /// Loads messages from an XML file.
        /// </summary>
        /// <param name="xmlFilePath">The path to the XML file containing the messages.</param>
        /// <returns>A dictionary with message keys and their corresponding values.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified XML file is not found.</exception>
        private Dictionary<string, string> LoadMessages(string xmlFilePath)
        {
            var messages = new Dictionary<string, string>();
            if (File.Exists(xmlFilePath))
            {
                var document = XDocument.Load(xmlFilePath);
                messages = document.Descendants("Message")
                                   .ToDictionary(
                                       m => m.Attribute("key")?.Value,
                                       m => m.Value
                                   );
            }
            else
            {
                // Handle the case where the file doesn't exist
                // You might want to log this error or throw an exception
                _logger.Error("Exception messages file not found: {XmlFilePath}", xmlFilePath);
                throw new FileNotFoundException("Exception messages file not found.", xmlFilePath);
            }

            return messages;
        }

        /// <summary>
        /// Retrieves a message corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key associated with the desired message.</param>
        /// <returns>The message associated with the specified key. Returns "Unknown error occurred." if the key is not found.</returns>
        public string GetMessage(string key)
        {
            return _messages.TryGetValue(key, out var message) ? message : "Unknown error occurred.";
        }
    }
}
