# Azure Data Lake Store Binding for Azure Functions

The following binding can be used on Azure Functions v1 and v2. 

## Instructions

To the output binding add the following attribute

```c#
[DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid")]out DataLakeStoreOutput dataLakeStoreOutput
```

To use the input binding simple add 'FileName' to bring in a specific file

```c#
[DataLakeStore(AccountFQDN = @"fqdn", ApplicationId = @"applicationid", ClientSecret = @"clientsecret", TenantID = @"tentantid", FileName = "/mydata/testfile.txt")]Stream myfile
```

## Binding Requirements 

1. [Azure Data Lake Store](https://azure.microsoft.com/en-us/services/data-lake-store/)
2. Setup [Service to Service Auth](https://docs.microsoft.com/en-us/azure/data-lake-store/data-lake-store-service-to-service-authenticate-using-active-directory) using Azure AD
3. Add the application settings noted below. 

### local.settings.json expected content
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "fqdn": "<FQDN for your Azure Lake Data Store>",
    "tentantid": "<Azure Active Directory Tentant for Authentication>",
    "clientsecret": "<Azure Active Directory Client Secret>",
    "applicationid": "<Azure Active Directory Application ID>",
    "blobconn": "<Azure Blobg Storage Connection String for testing Blob Trigger>"
  }
}


```