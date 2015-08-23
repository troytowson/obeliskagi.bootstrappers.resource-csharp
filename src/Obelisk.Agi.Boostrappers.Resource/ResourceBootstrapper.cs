using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;

namespace Obelisk.Agi.Bootstrappers.Resouce
{
    /// <summary>
    /// Represents a bootstrapper that will load all resources with specific keys
    /// </summary>
    public class ResourceBootstrapper : ObeliskBootstrapper<IDictionary<string, Type>>
    {
        public static string DefaultResourceName = "fastagi-mapping.resources";
        
        /// <summary>
        /// Gets the name of the resource file.
        /// </summary>
        public virtual string ResourceName
        {
            get
            {
                return DefaultResourceName;
            }
        }

        /// <summary>
        /// Gets the container for the channel.
        /// </summary>
        protected override IDictionary<string, Type> GetChannelContainer(IDictionary<string, Type> container)
        {
            return container;
        }

        /// <summary>
        /// Gets the specific script from the provided container.
        /// </summary>
        protected override IObeliskScript GetScript(IDictionary<string, Type> container, string script)
        {
            try
            {
                Type scriptType;
                if (!container.TryGetValue(script, out scriptType))
                    throw new Exception(String.Format("The type '{0}' could not be found.", script));

                return Activator.CreateInstance(scriptType) as IObeliskScript;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create an instance of the script.", ex);
            }
        }

        /// <summary>
        /// Gets a new container.
        /// </summary>
        protected override IDictionary<string, Type> GetContainer()
        {
            return new Dictionary<string, Type>();
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected override void Configure(IDictionary<string, Type> container)
        {
            using (var reader = new ResourceReader(String.Concat(AppDomain.CurrentDomain.BaseDirectory, ResourceName)))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    var scriptType = Type.GetType(entry.Value.ToString());
                    container.Add(entry.Key.ToString(), scriptType);
                }
            }
        }
    }
}