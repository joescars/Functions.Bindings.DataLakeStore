using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Functions.Bindings.DataLakeStore
{
    /// <summary></summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    [Binding]
    public sealed class DataLakeStoreAttribute : Attribute
    {
#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Gets or sets the ApplicationID setting.
        /// </summary>
        /// <value>
        /// The ApplicationId also Known as ClientID setting.
        /// </value>
        [AppSetting]
#pragma warning restore CS0618 // Type or member is obsolete
        public string ApplicationId { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Gets or sets the client secret setting.
        /// </summary>
        /// <value>
        /// The Client Secret setting.
        /// </value>
        [AppSetting]
#pragma warning restore CS0618 // Type or member is obsolete
        public string ClientSecret { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Gets or sets the TenantID setting.
        /// </summary>
        /// <value>
        /// The TenantID setting.
        /// </value>
        [AppSetting]
#pragma warning restore CS0618 // Type or member is obsolete
        public string TenantID { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Gets or sets the Account FQDN setting.
        /// </summary>
        /// <value>
        /// The DataLake full account FQDN setting.
        /// </value>
        [AppSetting]
#pragma warning restore CS0618 // Type or member is obsolete
        public string AccountFQDN { get; set; }

        /// <summary>
        /// Gets or sets the filename setting.
        /// </summary>
        /// <value>
        /// The full path and filename for input binding.
        /// </value>
        public string FileName { get; set; }

    }
}
